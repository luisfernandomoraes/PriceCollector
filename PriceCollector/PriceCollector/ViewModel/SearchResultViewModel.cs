﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Api.WebAPI.Products;
using PriceCollector.Model;
using PriceCollector.ViewModel.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class SearchResultViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string _barcode;
        private IProductApi _productApi;
        private int _id;
        private string _name;
        private decimal _priceCurrent;
        private decimal _priceCollected;
        private string _imageProduct;
        private bool _canShowProductImage;
        private IToastNotificator _notificator;
        private ProductCollected _product;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor used for creation of object.
        /// </summary>
        /// <param name="barcode"></param>
        public SearchResultViewModel(string barcode)
        {
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            _barcode = barcode;
        }

        /// <summary>
        /// Constructor used in edition of object.
        /// </summary>
        /// <param name="product">The product that want it to edit</param>
        public SearchResultViewModel(ProductCollected product)
        {
            this._product = product;
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
        }

        #endregion

        #region Commands

        public ICommand SaveCommand => new Command(Save);

        private async void Save(object obj)
        {
            try
            {
                if (!IsValid())
                {
                    await _notificator.Notify(ToastNotificationType.Warning, Utils.Constants.AppName,
                        "Atenção, o preencha o preço do produto.", TimeSpan.FromSeconds(3));
                    return;
                }


                // Making the collected product object.
                var productCollected = new ProductCollected
                {
                    PriceCurrent = PriceCurrent,
                    BarCode = Barcode,
                    CollectDate = DateTime.Now,
                    IDSupermarket = 0, //TODO: Rever
                    PriceCollected = PriceCollected,
                    ProductName = Name
                };


                ProductCollected = productCollected;


                // Checking if already exists a product with the same bar code in database.
                var productsCollected = DB.DBContext.ProductCollectedDataBase.GetItems();
                var productByQrcode = productsCollected.FirstOrDefault(x => x.BarCode == Barcode);
                if (productByQrcode != null)
                {
                    // If yes, just update in local database.
                    productCollected.ID = productByQrcode.ID;
                    DB.DBContext.ProductCollectedDataBase.Update(productCollected);
                    await PopupNavigation.PopAsync();
                    MessagingCenter.Send(this, "LoadData");
                    await _notificator.Notify(ToastNotificationType.Success, Utils.Constants.AppName,
                        $"Produto \"{productCollected.ProductName}\" atualizado com sucesso!", TimeSpan.FromSeconds(3));
                }
                else
                {
                    // Otherwise, I persist in local database's device. 
                    var id = DB.DBContext.ProductCollectedDataBase.SaveItem(productCollected);
                    if (id > 0)
                    {
                        UpdateCollectedProdutcList();
                        await PopupNavigation.PopAsync();
                        await _notificator.Notify(ToastNotificationType.Success, Utils.Constants.AppName,
                            "Coleta realizada com sucesso!", TimeSpan.FromSeconds(3));
                    }
                    else
                    {
                        await PopupNavigation.PopAsync();
                        await _notificator.Notify(ToastNotificationType.Error, Utils.Constants.AppName, "Ocorreu um erro ao salvar, por favor tente novamente mais tarde.", TimeSpan.FromSeconds(3));
                    }
                }



            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, Utils.Constants.AppName, "Ocorreu um erro, por favor tente novamente mais tarde.", TimeSpan.FromSeconds(3));
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        protected void UpdateCollectedProdutcList()
        {
            MessagingCenter.Send(this, "LoadData");
        }

        #endregion

        #region Properties

        public int ID
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Barcode
        {
            get { return _barcode; }
            set
            {
                if (value == _barcode) return;
                _barcode = value;
                OnPropertyChanged();
            }
        }


        public decimal PriceCurrent
        {
            get { return _priceCurrent; }
            set
            {
                if (value == _priceCurrent) return;
                _priceCurrent = value;
                OnPropertyChanged();
            }
        }

        public decimal PriceCollected
        {
            get { return _priceCollected; }
            set
            {
                if (value == _priceCollected) return;
                _priceCollected = value;
                OnPropertyChanged();
            }
        }

        public string ImageProduct
        {
            get { return _imageProduct; }
            set
            {
                if (value == _imageProduct) return;
                _imageProduct = value;
                OnPropertyChanged();
            }
        }

        public bool CanShowProductImage
        {
            get
            {
                return _canShowProductImage;
            }
            set
            {
                _canShowProductImage = value;
                OnPropertyChanged();

            }
        }

        public ProductCollected ProductCollected
        {
            get => _product;
            set => _product = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metodo responsavel por carregar informações do produto coletado.
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="productCollected"></param>
        /// <returns></returns>
        private async Task LoadProduct(string barcode, ProductCollected productCollected = null)
        {
            try
            {
                if (productCollected == null) // Produto ainda não coletado.
                {
                    var productApiResponse =
                        await _productApi.GetProduct("http://www.acats.scannprice.srv.br/api/", barcode);
                    Barcode = barcode;
                    if (!productApiResponse.Success)
                    {
                        // Caso o código de barras não exista nos registros da api.
                        ImageProduct = "NoImagemTarget.png";
                        return;
                    }
                    var p = productApiResponse.Result;
                    Name = p.Name;
                    Barcode = p.BarCode;
                    PriceCurrent = p.PriceCurrent;
                    PriceCollected = 0;
                }
                else // Edição do produto ja coletado.
                {
                    Name = productCollected.ProductName;
                    Barcode = productCollected.BarCode;
                    PriceCurrent = productCollected.PriceCurrent;
                    PriceCollected = productCollected.PriceCollected;
                }
                var urlImage = $@"http://imagens.scannprice.com.br/Produtos/{Barcode}.jpg";
                if (await _productApi.HasImage(urlImage))
                    ImageProduct = "NoImagemTarget.png";
                else
                    ImageProduct = urlImage;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadAsync()
        {
            if (_product != null)
            {
                await LoadProduct(string.Empty, _product);
            }
            else
            {
                await LoadProduct(_barcode, null);
            }
        }
        private bool IsValid()
        {
            if (PriceCollected == 0)
                return false;
            return true;
        }

        #endregion

        #region PropertyChange Event

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
