﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rb.Forms.Barcode.Pcl;

namespace PriceCollector.ViewModel
{
    public class ScannerViewModel : INotifyPropertyChanged
    {
        private String barcode = "";
        private bool initialized = false;
        private bool preview = true;
        private bool torch = false;
        private bool decoder = true;
        private bool _isEnable;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PreviewActivatedCommand { get; private set; }
        public ICommand BarcodeChangedCommand { get; private set; }
        public ICommand BarcodeDecodedCommand { get; private set; }
        public ICommand TogglePreviewCommand { get; private set; }
        public ICommand ToggleTorchCommand { get; private set; }
        public ICommand ToggleDecoderCommand { get; private set; }


        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                OnPropertyChanged();
            }
        }

        public String Barcode
        {
            get { return barcode; }
            set
            {
                barcode = value;
                OnPropertyChanged();
            }
        }

        public bool Initialized
        {
            get { return initialized; }
            set
            {
                initialized = value;
                OnPropertyChanged();
            }
        }


        public bool Preview
        {
            get { return preview; }
            set
            {
                preview = value;
                OnPropertyChanged();
            }
        }

        public bool Torch
        {
            get { return torch; }
            set
            {
                torch = value;
                OnPropertyChanged();
            }
        }


        public bool Decoder
        {
            get { return decoder; }
            set
            {
                decoder = value;
                OnPropertyChanged();
            }
        }

        public ScannerViewModel()
        {
            IsEnable = true;
            PreviewActivatedCommand = new Command(() => { Initialized = true; });
            BarcodeChangedCommand = new Command<Barcode>(UpdateBarcode);
            BarcodeDecodedCommand = new Command<Barcode>(logBarcode);
            TogglePreviewCommand = new Command(() => { Preview = !Preview; });
            ToggleTorchCommand = new Command(() => { Torch = !Torch; });
            ToggleDecoderCommand = new Command(() => { Decoder = !Decoder; });
        }



        private void logBarcode(Barcode barcode)
        {
            Debug.WriteLine("Decoded barcode [{0} - {1}]", barcode?.Result, barcode?.Format);
            if (barcode?.Result != null)
                MessagingCenter.Send<ScannerViewModel, Barcode>(this, "BarcodeChanged", barcode);
            IsEnable = false;
        }

        private void UpdateBarcode(Barcode barcode)
        {
            Barcode = String.Format("Last Barcode: [{0} - {1}]", barcode?.Result, barcode?.Format);
            IsEnable = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, args);
            }
        }
    }
}