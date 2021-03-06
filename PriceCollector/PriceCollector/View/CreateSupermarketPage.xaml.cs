﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriceCollector.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSupermarketPage : ContentPage
    {
        public CreateSupermarketPage()
        {
            InitializeComponent();
            BindingContext = new CreateSupermarketViewModel(this);
        }
    }
}
