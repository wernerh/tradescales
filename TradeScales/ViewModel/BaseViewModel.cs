using System;
using System.ComponentModel;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields

        protected readonly IEntityBaseRepository<Error> _errorsRepository;
        protected readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        //public BaseViewModel(IEntityBaseRepository<Error> errorsRepository, IUnitOfWork unitOfWork)
        //{
        //    _errorsRepository = errorsRepository;
        //    _unitOfWork = unitOfWork;
        //}

        #endregion

        #region Events

        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Methods

        /// <summary>
        /// Property Changed Event.     
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch(Exception exception)
            {
                LogError(exception);
            }   
        }

        private void LogError(Exception exception)
        {
            try
            {
                Error error = new Error()
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    DateCreated = DateTime.Now
                };

                _errorsRepository.Add(error);
                _unitOfWork.Commit();
            }
            catch { }
        }
        #endregion
    }
}
