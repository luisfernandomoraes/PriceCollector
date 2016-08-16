using System;
using PriceCollector.ViewModel;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _mainPageViewModel;
        public MainPage()
        {
            _mainPageViewModel = new MainPageViewModel();
            InitializeComponent();
            BindingContext = _mainPageViewModel;
        }
    }
}
