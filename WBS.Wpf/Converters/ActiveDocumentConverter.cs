﻿using System;
using System.Globalization;
using System.Windows.Data;
using WBS.Wpf.ViewModel;
using WBS.Wpf.ViewModel.Tools;

namespace WBS.Wpf.Converters
{
    /// <summary>
    /// Converter that checks for valid active document values.
    /// </summary>
    public class ActiveDocumentConverter : IValueConverter
    {
        #region Public Methods

        /// <summary>
        /// Convert active content
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">Culture info</param>
        /// <returns>Active content</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DocumentViewModel || value is StartPageViewModel || value is ToolOneViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DocumentViewModel || value is StartPageViewModel || value is ToolOneViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        #endregion
    }
}
