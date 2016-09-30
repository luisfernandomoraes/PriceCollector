
namespace PriceCollector.Model
{
    public class Category:IModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}