using System;
using System.Windows;
using System.Windows.Controls;

namespace WBS.Wpf.Extensions
{
    public static class WebBrowserExtensions
    {
        public static readonly DependencyProperty BindableSourceProperty =
             DependencyProperty.RegisterAttached("BindableSource", typeof(object), typeof(WebBrowserExtensions), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static object GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, object value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject dependancyObject, DependencyPropertyChangedEventArgs propertyChangedevent)
        {
            WebBrowser browser = dependancyObject as WebBrowser;

            if (browser == null)
                return;

            Uri uri = null;

            if (propertyChangedevent.NewValue is string)
            {
                var uriString = propertyChangedevent.NewValue as string;
                uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            }
            else if (propertyChangedevent.NewValue is Uri)
            {
                uri = propertyChangedevent.NewValue as Uri;
            }
            browser.Source = uri;
        }
    }
}
