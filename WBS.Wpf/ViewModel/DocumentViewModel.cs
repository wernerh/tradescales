using System;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Wpf.ViewModel
{
    /// <summary>
    /// </summary>
    public class DocumentViewModel : PaneViewModel
    {

        #region Properties

        private string _Name;
        /// <summary>
        /// </summary>
        public string Name
        {
            get { return (IsDirty) ? _Name + "*" : _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private bool _IsDirty;
        /// <summary>
        /// </summary>
        public bool IsDirty
        {
            get { return _IsDirty; }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("Name");
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DocumentViewModel class.
        /// </summary>
        /// <param name="name"></param>
        public DocumentViewModel(string name)
        {
            Name = name;
        }

        public DocumentViewModel()
        {

        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        public virtual void UpdateWeight(double weightReading, bool isReceiving)
        {
            throw new ArgumentException("Not Implemented");
        }

        public virtual void ReloadEntities()
        {
            throw new ArgumentException("Not Implemented");
        }
        #endregion
    }
}
