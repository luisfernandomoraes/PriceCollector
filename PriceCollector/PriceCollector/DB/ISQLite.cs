using SQLite.Net;

namespace PriceCollector.DB
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}