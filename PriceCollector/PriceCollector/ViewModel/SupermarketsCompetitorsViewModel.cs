using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PriceCollector.Annotations;
using PriceCollector.Model;

namespace PriceCollector.ViewModel
{
    public class SupermarketsCompetitorsViewModel:INotifyPropertyChanged
    {
        private ICollection<SupermarketsCompetitors> _supermarketsCompetitorses;

        public ICollection<SupermarketsCompetitors> SupermarketsCompetitorses
        {
            get { return _supermarketsCompetitorses; }
            set
            {
                if (Equals(value, _supermarketsCompetitorses)) return;
                _supermarketsCompetitorses = value;
                OnPropertyChanged();
            }
        }

        public SupermarketsCompetitorsViewModel()
        {
            LoadAsync();
        }

        private void LoadAsync()
        {
            SupermarketsCompetitorses = new List<SupermarketsCompetitors>()
            {
                new SupermarketsCompetitors()
                {
                    Name = ""
                }
            };
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}