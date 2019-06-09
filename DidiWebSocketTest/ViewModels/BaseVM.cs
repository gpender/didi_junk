using System;
using System.ComponentModel;

namespace DidiWebSocketTest.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public BaseVM() { }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
