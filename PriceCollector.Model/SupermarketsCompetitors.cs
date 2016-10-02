using SQLite;

namespace PriceCollector.Model
{
    public class SupermarketsCompetitors:IModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}