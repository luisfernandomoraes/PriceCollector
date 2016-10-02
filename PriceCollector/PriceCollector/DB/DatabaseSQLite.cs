using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.DB;
using PriceCollector.Model;

using SQLite.Net;
using Xamarin.Forms;


namespace PriceCollector.DB
{
    public class DatabaseSQLite<T> where T : class, IModel
    {
        private readonly object _locker = new object();

        readonly SQLiteConnection _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        public DatabaseSQLite()
        {
            _database = DependencyService.Get<ISQLite>().GetConnection();
            // create the tables
            _database.CreateTable<T>();
        }

        public IEnumerable<T> GetItems()
        {
            lock (_locker)
            {
                return (from i in _database.Table<T>()
                        select i).ToList();
            }
        }

        /*public IEnumerable<T> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<T>("SELECT * FROM [T] WHERE [Done] = 0");
            }
        }*/

        public T GetItem(int id)
        {
            lock (_locker)
            {
                return _database.Table<T>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int SaveItem(T item)
        {
            lock (_locker)
            {
                if (item.ID != 0)
                {
                    _database.Update(item);
                    return item.ID;
                }
                else
                {
                    return _database.Insert(item);
                }
            }
        }

        public void Update(T item)
        {
            lock (_locker)
            {
                _database.Update(item);
            }
        }

        public int DeleteItem(int id)
        {
            lock (_locker)
            {
                return _database.Delete<T>(id);
            }
        }
    }
}