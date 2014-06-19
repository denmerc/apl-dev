using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace APLPromoter.UI.Wpf.Controls
{
	    public class PopupWindow : FrameworkElement
    	{
        #region Fields

        private Window window;

        #endregion

        #region Constructors

        public PopupWindow()
        {
            this.Unloaded += new RoutedEventHandler(PopupWindow_Unloaded);
        }

        #endregion

        #region Properties

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public string WindowTitle
        {
            get { return (string)GetValue(WindowTitleProperty); }
            set { SetValue(WindowTitleProperty, value); }
        }

        public DataTemplate Template
        {
            get { return (DataTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }

        public bool ShowInTaskbar
        {
            get { return (bool)GetValue(ShowInTaskbarProperty); }
            set { SetValue(ShowInTaskbarProperty, value); }
        }

        #endregion

        #region Dependancy Properties

        /// <summary>
        /// This property is used to open and close the winodw.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(PopupWindow), new UIPropertyMetadata(false, CallbackOnIsOpen));

        /// <summary>
        /// Title of the Window created.
        /// </summary>
        public static readonly DependencyProperty WindowTitleProperty =
            DependencyProperty.Register("WindowTitle", typeof(string), typeof(PopupWindow), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// DataTemplate of the window created.
        /// </summary>
        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.Register("Template", typeof(DataTemplate), typeof(PopupWindow), new UIPropertyMetadata(null));

        /// <summary>
        /// Get and set ShowInTaskbar property
        /// </summary>        
        public static readonly DependencyProperty ShowInTaskbarProperty =
            DependencyProperty.Register("ShowInTaskbar", typeof(bool), typeof(PopupWindow), new UIPropertyMetadata(true));



        #endregion

        #region Static
        
        private static void CallbackOnIsOpen(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PopupWindow popup = sender as PopupWindow;

            if (popup != null)
            {
                popup.Activate(popup.IsOpen);
            }
        }

        #endregion

        #region Methods

        #region Event Handlers
        
        private void PopupWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (window != null)
            {
                UnSubscribeWindowEvents();
                window.Close();
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            if (IsOpen)
            {
                SetCurrentValue(IsOpenProperty, false);
            }
        }
        
        #endregion

        #region PrivateMethods

        private void UnSubscribeWindowEvents()
        {
            window.Closed -= new EventHandler(window_Closed);
        }
        #endregion

        #region Public Methods

        public void Activate()
        {
            Activate(true);
        }

        public void Activate(bool activate)
        {
            if (activate)
            {
                if (window == null || !window.IsLoaded)
                {
                    window = new Window();
                    window.Owner = App.Current.MainWindow;
                    window.Style = App.Current.FindResource("CustomWindowStyle") as Style;
                    window.Title = WindowTitle;
                    window.ShowInTaskbar = this.ShowInTaskbar;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.DataContext = this.DataContext;
                    window.ContentTemplate = this.Template;
                    window.Show();
                    window.Closed += new EventHandler(window_Closed);
                }
                else
                {
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                }
            }
            else
            {
                UnSubscribeWindowEvents();
                window.Close();
                window = null;
            }
        }

        #endregion

        #endregion
    }
}