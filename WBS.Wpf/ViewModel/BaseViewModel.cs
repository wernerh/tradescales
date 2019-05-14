using System.ComponentModel;

namespace WBS.Wpf.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property Changed Event.     
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
