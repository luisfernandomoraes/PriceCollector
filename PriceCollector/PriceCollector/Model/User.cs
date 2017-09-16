namespace PriceCollector.Model
{
    public class User: IModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string  Password { get; set; }

    }
}
