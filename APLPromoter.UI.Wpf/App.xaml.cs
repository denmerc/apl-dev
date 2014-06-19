
using APLPromoter.UI.Wpf;
using APLPromoter.UI.Wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using APLPromoter.UI.Wpf.Properties;
using System.Windows.Markup;
using System.IO;
using System.Xml;






namespace APLPromoter.UI.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Locator = new ViewModel.ViewModelLocator(); //Sets up export for main window

            if(Settings.Default.UserHeaderColor != string.Empty)
            { 
                StringReader reader = new StringReader(Settings.Default.UserHeaderColor);
                XmlReader xreader = XmlReader.Create(reader);
                Locator.MainViewModel.UserSettings.HeaderBrush = (LinearGradientBrush)XamlReader.Load(xreader);
            }

            if (Settings.Default.UserBackgroundColor != string.Empty)
            {
                StringReader reader = new StringReader(Settings.Default.UserBackgroundColor);
                XmlReader xreader = XmlReader.Create(reader);
                Locator.MainViewModel.UserSettings.BackgroundBrush = (LinearGradientBrush)XamlReader.Load(xreader);
            }

            if (Settings.Default.UserTextBrush != string.Empty)
            {
                StringReader reader = new StringReader(Settings.Default.UserTextBrush);
                XmlReader xreader = XmlReader.Create(reader);
                Locator.MainViewModel.UserSettings.TextBrush = (SolidColorBrush)XamlReader.Load(xreader);
            }
            if (Settings.Default.UserBorderBrush != string.Empty)
            {
                StringReader reader = new StringReader(Settings.Default.UserBorderBrush);
                XmlReader xreader = XmlReader.Create(reader);
                Locator.MainViewModel.UserSettings.BorderBrush = (SolidColorBrush)XamlReader.Load(xreader);
            }

            App.Current.Resources.Add("Locator", Locator);
            base.OnStartup(e);

            


            
        }
        public ViewModelLocator Locator { get; set; }
        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

        }

        protected override void OnExit(ExitEventArgs e)
        {
            
            var headerBrushString = XamlWriter.Save(Locator.MainViewModel.UserSettings.HeaderBrush);
            Settings.Default.UserHeaderColor = headerBrushString;

            var backgroundBrushString = XamlWriter.Save(Locator.MainViewModel.UserSettings.BackgroundBrush);
            Settings.Default.UserBackgroundColor = backgroundBrushString;

            var textBrushString = XamlWriter.Save(Locator.MainViewModel.UserSettings.TextBrush);
            Settings.Default.UserTextBrush = textBrushString;

            var borderBrushString = XamlWriter.Save(Locator.MainViewModel.UserSettings.BorderBrush);
            Settings.Default.UserBorderBrush = borderBrushString;

            Settings.Default.Save();
            ViewModelLocator.Cleanup();
            base.OnExit(e);
        }

      
    }
}
