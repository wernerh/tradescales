﻿using System.Windows;
using System.Windows.Controls;
using TradeScales.Wpf.ViewModel;
using TradeScales.Wpf.ViewModel.Tools;

namespace TradeScales.Wpf.View.Pane
{
    /// <summary>
    /// Provides a way to apply styles based on custom logic.
    /// </summary>
    public class PaneStyleSelector : StyleSelector
    {

        #region Properties

        /// <summary>
        /// Tool one style
        /// </summary>
        public Style ToolOneStyle { get; set; }

        /// <summary>
        /// Tool two style
        /// </summary>
        public Style ToolTwoStyle { get; set; }

        /// <summary>
        /// Tool three style
        /// </summary>
        public Style ToolThreeStyle { get; set; }

        /// <summary>
        /// Document style
        /// </summary>
        public Style DocumentStyle { get; set; }

        /// <summary>
        /// Start up page style
        /// </summary>
        public Style StartPageStyle { get; set; }

        /// <summary>
        /// Tickets style
        /// </summary>
        public Style TicketsStyle { get; set; }

        /// <summary>
        /// Weighbridge certificate style
        /// </summary>
        public Style PdfDocumentStyle { get; set; }

        /// <summary>
        /// New ticket style
        /// </summary>
        public Style NewTicketStyle { get; set; }

        /// <summary>
        /// Edit ticket style
        /// </summary>
        public Style EditTicketStyle { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, returns a Style based on custom logic.
        /// </summary>
        /// <param name="item">The content</param>
        /// <param name="container">The element to which the style will be applied</param>
        /// <returns>Returns an application-specific style to apply; otherwise, null.</returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {

            if (item is ToolOneViewModel)
            {
                return ToolOneStyle;
            }

            if (item is ToolTwoViewModel)
            {
                return ToolTwoStyle;
            }


            if (item is ToolThreeViewModel)
            {
                return ToolThreeStyle;
            }

            if (item is EditTicketViewModel)
            {
                return EditTicketStyle;
            }

            if (item is NewTicketViewModel)
            {
                return NewTicketStyle;
            }

            if (item is PdfDocumentViewModel)
            {
                return PdfDocumentStyle;
            }

            if (item is TicketListViewModel)
            {
                return TicketsStyle;
            }

            if (item is StartPageViewModel)
            {
                return StartPageStyle;
            }

            if (item is DocumentViewModel)
            {
                return DocumentStyle;
            }

            return base.SelectStyle(item, container);

        }

        #endregion

    }
}
