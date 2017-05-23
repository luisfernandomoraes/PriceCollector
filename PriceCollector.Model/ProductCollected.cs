using System;
using SQLite.Net.Attributes;

namespace PriceCollector.Model
{
    public class ProductCollected:IModel
    {
        [PrimaryKey, AutoIncrement]

        public int ID { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public string ImageProduct { get; set; }
        public int IDSupermarket { get; set; }
        public DateTime CollectDate { get; set; }
        public decimal PriceCurrent { get; set; }
        public decimal PriceCollected { get; set; }


    }
}