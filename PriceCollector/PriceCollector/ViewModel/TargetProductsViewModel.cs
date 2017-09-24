using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Toasts;
using PriceCollector.Model;
using PriceCollector.Properties;
using PriceCollector.View;
using PriceCollector.ViewModel.Services;
using PriceCollector.WebAPI.Products;
using Rg.Plugins.Popup.Services;
using Scandit.BarcodePicker.Unified;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
	public class TargetProductsViewModel:INotifyPropertyChanged,IReloadDataViewModel
	{

		#region Fields
		private IProductApi _productApi;
		private static IToastNotificator _notificator = DependencyService.Get<IToastNotificator>();
		private bool _isBusy;
		private ObservableCollection<Product> _products;
		private static Product _product;
		private readonly TargetProductsPage _targetProductsPage;
		#endregion

		#region Properties

		public ObservableCollection<Product> Products
		{
			get { return _products; }
			set
			{
				if (Equals(value, _products)) return;
				_products = value;
				OnPropertyChanged();
			}
		}

		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				if (Equals(value, _isBusy)) return;
				_isBusy = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region Ctor

		public TargetProductsViewModel(TargetProductsPage targetProductsPage)
		{
			_targetProductsPage = targetProductsPage;
			_productApi = DependencyService.Get<IProductApi>();
			
			_isBusy = false;
		    ScanditService.BarcodePicker.DidScan -= MainPageViewModel.BarcodePickerOnDidScan;
			ScanditService.BarcodePicker.DidScan += BarcodePickerOnDidScan;
			MessagingCenter.Subscribe<SearchResultViewModel>(this, "LoadData", async (sender) =>
			{
				await LoadData();
			});

			Task.Run(LoadData);
		}

		#endregion

		#region Methods

		public async Task LoadData()
		{
			try
			{
				IsBusy = true;
				var result = await _productApi.GetProductsToCollect("https://blogmachine.club");
				if (result.Success)
				{
					var productList = new List<Product>();
					var productsInDb = DB.DBContext.ProductCollectedDataBase.GetItems().ToList();
					foreach (var p in result.CollectionResult)
					{
						if(productsInDb.Any(x=>x.BarCode == p.BarCode))
							continue;

						var urlImage = $@"http://imagens.scannprice.com.br/Produtos/{p.BarCode}.jpg";

						if (await _productApi.HasImage(urlImage))
							p.ImageProduct = "NoImagemTarge.png";
						else
							p.ImageProduct = urlImage;

						productList.Add(p);
					}

					Products = new ObservableCollection<Product>(productList);
					IsBusy = false;
				}
			}
			catch (Exception e)
			{
				await _notificator.Notify(ToastNotificationType.Error, @"PriceCollector", "Houve um erro ao carregar os produtos", TimeSpan.FromSeconds(3));
				Debug.WriteLine(e);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task StartBarCodeScannerAsync(Product product)
		{
			
			_product = product;
			await ScanditService.BarcodePicker.StartScanningAsync(true);
		}

	    public static async void BarcodePickerOnDidScan(ScanSession session)
		{
			
			await ScanditService.BarcodePicker.StopScanningAsync();
			string recognizedCode = session.NewlyRecognizedCodes.LastOrDefault()?.Data;
			Device.BeginInvokeOnMainThread(async () =>
			{
				var searchResultPage = new SearchResultPage(recognizedCode);
				if (_product == null)
					return;
				else if (recognizedCode == _product.BarCode)
				{
					if (PopupNavigation.PopupStack.Count < 1)
						await PopupNavigation.PushAsync(searchResultPage);
				}
				else
				{
					await _notificator.Notify(ToastNotificationType.Error, Utils.Constants.AppName,
						$"O código de barras lido {recognizedCode}, não confere com o produto {_product.Name}, por favor tente novamente",
						TimeSpan.FromSeconds(5));
				}
			});

		}
	   
		#endregion

		#region PropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	  
	}
}