using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Model.Properties;

namespace PriceCollector.Model
{
    public class Product:IModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public decimal PriceCurrent { get; set; }
        public decimal PriceCollected { get; set; }
        public Category CategoryProduct { get; set; }
        public string ImageProduct { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
