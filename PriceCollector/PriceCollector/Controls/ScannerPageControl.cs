using System;
using PriceCollector.View;

namespace PriceCollector.Controls
{
    public class ScannerPageControl
    {
        private static readonly Lazy<ScannerPageControl> Lazy =
            new Lazy<ScannerPageControl>(() => new ScannerPageControl());

        public static ScannerPageControl Instance => Lazy.Value;

        private ScannerPage _scannerPage;

        private ScannerPageControl()
        {
        }

        public ScannerPage CreateScannerPage()
        {
            _scannerPage?.DisableScanner();
            _scannerPage = new ScannerPage();

            return _scannerPage;
        }
    }
}