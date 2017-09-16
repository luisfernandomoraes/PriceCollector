﻿using SQLite;
using SQLite.Net.Attributes;

namespace PriceCollector.Model
{
    public class AppConfig : IModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string WebApiAddress { get; set; }
    }
}