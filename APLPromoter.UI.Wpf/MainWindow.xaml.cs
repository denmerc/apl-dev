using APLPromoter.UI.Wpf.ViewModel;
using System.Windows;
using APLPromoter.Core.Reactive;
using APLPromoter.Core.Reactive.Events;
using System.Windows.Input;


namespace APLPromoter.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        IEventAggregator _publisher;
        public MainWindow()
        {
            InitializeComponent();
            var app = App.Current.Resources["Locator"] as ViewModelLocator;
            //app.EventPublisher = _publisher;
            _publisher = app.EventPublisher;
            this.DataContext = app.ViewRouter;
            //this.DataContext = new APLPromoter.UI.Wpf.Views.ViewRouter(); //todo: work around bc using xaml data context, login does not show

            //Loaded += (sender, e) =>
            //        MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _publisher.Publish(new WindowResizedEvent());
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }


    }
}
