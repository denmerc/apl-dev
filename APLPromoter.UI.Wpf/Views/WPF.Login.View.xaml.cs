using System;
using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;
using Telerik.Windows.Controls.Docking;
using APLPromoter.Core.Reactive;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using APLPromoter.Core.Reactive.Events;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : IViewFor<LoginViewModel>
    {
       
       public LoginView()
        {
            InitializeComponent();
            
            
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            //this.Bind(ViewModel, model => model.Password, x => x.password);
            //this.Bind(ViewModel, model => model.LoginName, view => view.userName.Text);
            ////this.OneWayBind(ViewModel, model => model.Message, x => x.message.Content);
            //this.OneWayBind(ViewModel, model => model.InitializationMessage, x => x.xInitializationMessage.Content);
            //this.OneWayBind(ViewModel, x => x.LoginCommand, x => x.login);


            //this.Bind(ViewModel, model => model.Password, x => x.password);
            //this.Bind(ViewModel, model => model.LoginName, view => view.userName.Text);
            //this.OneWayBind(ViewModel, model => model.Message, x => x.message.Content);
            //this.OneWayBind(ViewModel, model => model.InitializationMessage, x => x.xInitializationMessage.Content);
            //this.OneWayBind(ViewModel, x => x.LoginCommand, x => x.login.Command);

            //this.OneWayBind(ViewModel, model => model.IsProgressRunning, x => x.xProgress.IsIndeterminate);
            //this.OneWayBind(ViewModel, model => model.ProgressBrush, x => x.xProgress.Foreground);
            //this.Bind(ViewModel, vm => vm.SplashVisible, view => view.xSplash.Visibility);
            //this.Bind(ViewModel, vm => vm.LoginVisible, view => view.xLogin.Visibility);


            Publisher = ((ViewModelLocator)App.Current.Resources["Locator"]).EventPublisher;

            Publisher.GetEvent<WindowResizedEvent>()
                .Subscribe(_ => this.ViewModel.RaisePropertyChanged("Workflow"));


        }

       void LoginView_Loaded(object sender, RoutedEventArgs e)
       {

       }

        public static readonly DependencyProperty ViewModelProperty =
    DependencyProperty.Register("ViewModel", typeof(LoginViewModel), typeof(LoginView), new PropertyMetadata(null));


        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LoginViewModel)value; }
        }

        public LoginViewModel ViewModel
        {
            get
            {
                return (LoginViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty,
                    value);
            }
        }


        public IEventAggregator Publisher {get; set;}

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            foreach (var item in ViewModel.Workflow.Steps)
            {
                if (((System.Windows.Controls.HeaderedContentControl)(sender)).Header != item.Name)
                {
                    item.IsActive = false;
                }
            }
            this.ViewModel.RaisePropertyChanged("Workflow");
            //IEnumerable<Expander> exps = FindVisualChildren<Expander>(RadPane);


            //int childrenCount = VisualTreeHelper.GetChildrenCount(itemsControl);
            //for (int i = 0; i < childrenCount; i++)
            //{
            //    var child = VisualTreeHelper.GetChild(itemsControl, i);

            //    Expander childType = child as Expander;
            //    if (childType != null)
            //    {
            //        if( childType != (Expander)sender){
            //            childType.IsExpanded = false;
            //        }
            //    }

            //}
         
        }

        public  IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                T childType = child as T;
                if (childType == null)
                {
                    foreach (var other in FindVisualChildren<T>(child))
                        yield return other;
                }
                else
                {
                    yield return (T)child;
                }
            }
        }
    }
    
}
