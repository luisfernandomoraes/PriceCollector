using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Model.Annotations;
using PriceSpy.Model;

namespace PriceCollector.Model
{
    public class Product:IModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public double PriceCurrent { get; set; }
        public double PriceCollected { get; set; }
        public Category CategoryProduct { get; set; }
   
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
