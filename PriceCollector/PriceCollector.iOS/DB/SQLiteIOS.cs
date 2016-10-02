using System.IO;
using PriceCollector.DB;
using PriceCollector.iOS.DB;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteIOS))]
namespace PriceCollector.iOS.DB
{
    public class SQLiteIOS : ISQLite
    {

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "pricecollectordb.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, "..", "Library", sqliteFilename);
            // Create the connection
            var conn = new SQLiteConnection(new SQLitePlatformIOS(), path);
            // Return the database connection
            return conn;
        }
    }
}