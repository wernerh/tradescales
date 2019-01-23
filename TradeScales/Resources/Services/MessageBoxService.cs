using System;
using System.Windows;
using TradeScales.Resources.MessageBoxes.CustomExceptionMessageBox;
using TradeScales.Resources.MessageBoxes.CustomMessageBox;
using TradeScales.Resources.Services.Interfaces;

namespace TradeScales.Resources.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        #region Public Methods

        /// <summary>
        /// Show exception message box.
        /// </summary>
        /// <param name="exception">The exception to display in the message box</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="image">The image to display with the exception in the message box</param>
        public void ShowExceptionMessageBox(Exception exception, string title, MessageBoxImage image)
        {
            ExceptionMessageBox view = new ExceptionMessageBox(exception, title, image);
            view.ShowDialog();
        }

        /// <summary>
        /// Show general message box.
        /// </summary>
        /// <param name="message">The message to display in the message box</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="button">The button combination to display in the message box</param>
        /// <returns>MessageBoxResult enumeration value</returns>
        public MessageBoxResult ShowMessageBox(string message, string title, MessageBoxButton button)
        {
            MessageBoxViewModel model = new MessageBoxViewModel(message, title, button);
            MessageBoxView view = new MessageBoxView(model);
            view.ShowDialog();
            return model.Result;
        }

        #endregion

    }
}
