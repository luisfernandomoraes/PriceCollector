﻿using System.ComponentModel;
using System.Threading.Tasks;

namespace PriceCollector.ViewModel.Services
{
    public interface IReloadDataViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// Comportamento comum a todas as viewModels do app.
        /// </summary>
        /// <returns></returns>
        Task LoadData();
    }
}