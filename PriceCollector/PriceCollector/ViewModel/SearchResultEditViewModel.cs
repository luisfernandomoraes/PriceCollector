using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Model;
using PriceCollector.ViewModel.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class SearchResultEditViewModel : SearchResultViewModel
    {
        #region Fields

        private IToastNotificator _toastNotificator;

        #endregion

        #region Commands

        public ICommand DeleteCommand => new Command(Delete);

        #endregion

        #region Constructors

        public SearchResultEditViewModel(ProductCollected product) : base(product)
        {
            _toastNotificator = DependencyService.Get<IToastNotificator>();
            
        }

        #endregion

        private void Delete(object obj)
        {
            try
            {
                int result = DB.DBContext.ProductCollectedDataBase.DeleteItem(ProductCollected.ID);
                if (result > 0)
                {
                    _toastNotificator.Notify(ToastNotificationType.Success,"PriceCollector" ,$"Protudo {ProductCollected.ProductName} deletado com sucesso.", TimeSpan.FromSeconds(3));
                }
                else
                {
                    _toastNotificator.Notify(ToastNotificationType.Error, "PriceCollector", $"Ocorreu um erro ao deletar o  produto {ProductCollected.ProductName}", TimeSpan.FromSeconds(3));
                }
                base.UpdateCollectedProdutcList();
                PopupNavigation.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _toastNotificator.Notify(ToastNotificationType.Error, "Erro :(", e.ToString(), TimeSpan.FromSeconds(3));
            }
        }
    }
}
