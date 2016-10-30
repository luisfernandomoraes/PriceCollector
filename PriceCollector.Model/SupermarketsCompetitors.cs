using SQLite;

namespace PriceCollector.Model
{
    public class SupermarketsCompetitors:IModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
    }
}