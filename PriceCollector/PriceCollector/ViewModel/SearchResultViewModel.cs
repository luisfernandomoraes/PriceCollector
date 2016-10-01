using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Annotations;
using PriceCollector.Api.WebAPI.Products;
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

        #endregion

        #region Ctor

        public SearchResultViewModel(string barcode)
        {
            _productApi = DependencyService.Get<IProductApi>();
            Task.Run(()=>LoadProduct(barcode));
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
        #endregion

        #region Methods


        private async Task LoadProduct(string barcode)
        {
            try
            {
                var productApiResponse = await _productApi.GetProduct("http://www.acats.scannprice.srv.br/api/",barcode);
                if(!productApiResponse.Success)
                    return;
                var p = productApiResponse.Result;
                Name = p.Name;
                Barcode = p.BarCode;
                PriceCurrent = p.PriceCurrent;

                var urlImage = $@"http://imagens.scannprice.com.br/Produtos/{p.BarCode}.jpg";
                if (await _productApi.HasImage(urlImage))
                    ImageProduct = "NoImagemTarge.png";
                else
                    ImageProduct = urlImage;

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task InitializationAsync()
        {
            CanShowProductImage = await _productApi.HasImage(ImageProduct);
            if (!CanShowProductImage)
            {
                ImageProduct = "NoImagemTarge.png";
            }
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
