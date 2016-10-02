using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PriceCollector.Model;

namespace PriceCollector.DB
{
    public class DBContext
    {
        #region Fields

        private static AppConfig _appConfig;
        private static DatabaseSQLite<AppConfig> _appConfigurationDataBase;
        private static DatabaseSQLite<Model.ProductCollected> _productCollectedDataBase;

        #endregion

        #region Properties

        public static AppConfig CurrentAppConfiguration
        {
            get
            {
                if (_appConfig == null)
                {
                    LoadConfiguration();
                }

                return _appConfig;
            }

        }
        private static DatabaseSQLite<AppConfig> AppConfigurationDataBase
        {
            get
            {
                if (_appConfigurationDataBase == null)
                {
                    _appConfigurationDataBase = new DatabaseSQLite<AppConfig>();
                }
                return _appConfigurationDataBase;
            }
        }
        public static DatabaseSQLite<ProductCollected> ProductCollectedDataBase
        {
            get
            {
                if (_productCollectedDataBase == null)
                {
                    _productCollectedDataBase = new DatabaseSQLite<ProductCollected>();
                }
                return _productCollectedDataBase;
            }
        }
        #endregion

        #region Methods

        public static void LoadConfiguration()
        {
            try
            {
                var configs = AppConfigurationDataBase.GetItems();
                var appConfigs = configs as IList<AppConfig> ?? configs.ToList();

                if (!appConfigs.Any())
                {
                    var defaultConfigs = new AppConfig
                    {
# if DEBUG
                        WebApiAddress = "http://www.acats.scannprice.srv.br/api/"
#else
                        WebApiAddress = "http://www.acats.scannprice.srv.br/api/"
#endif
                    };

                    AppConfigurationDataBase.SaveItem(defaultConfigs);
                    _appConfig = defaultConfigs;
                }
                else
                    _appConfig = appConfigs.First();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        #endregion
    }
}