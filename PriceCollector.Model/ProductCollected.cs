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
        /// <summary>
        /// GAMBIARRA ALERT(FIXME): Essa propriedade é usada pra corrigir o comportamento correto da exibição do produto, no caso de ele não estar cadastrado no banco de dados do usuario ou o coletor não dispor de rede no momento da coleta, sendo assim, exibido o codigo de barras ao invés do nome do produto.
        /// </summary>
        public string ProductNameDisplayed => string.IsNullOrEmpty(ProductName) ? BarCode : ProductName;
    }
}