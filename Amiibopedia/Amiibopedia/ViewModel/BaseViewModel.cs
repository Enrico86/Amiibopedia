using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Amiibopedia.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy; set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
