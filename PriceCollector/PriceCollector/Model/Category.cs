
using SQLite;
using SQLite.Net.Attributes;

namespace PriceCollector.Model
{
    public class Category:IModel
    {
        [PrimaryKey, AutoIncrement]

        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}