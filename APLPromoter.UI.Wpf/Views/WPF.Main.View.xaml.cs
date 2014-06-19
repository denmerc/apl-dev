using APLPromoter.UI.Wpf.ViewModel;
using APLPromoter.Client.Entity;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Reactive.Linq;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using APLPromoter.Core.Reactive;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using System.Threading;
using APLPromoter.Core.Reactive.Events;
using System.Windows.Controls;
using System.Windows.Media;
using APLPromoter.UI.Wpf.Extensions;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class 
        MainView : IViewFor<MainViewModel>
    {
        public MainView()
        {   
            
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            Publisher = ((ViewModelLocator)App.Current.Resources["Locator"]).EventPublisher;
            var XamlHelper = new XamlHelper();

            //xTreeView.WhenAny(x => x.SelectedItem, x => { return x.GetValue(); })
            //    .Subscribe(x =>
            //    {
            //        var navNode = x as Navigator;

            //        if (navNode != null)
            //        {
            //            Publisher.Publish(navNode);
            //        }


            //    });


            this.Bind(ViewModel, x => x.SelectedViewModel, x => x.xDetailView.ViewModel);
            //this.WhenAny(x => x.xTreeView.SelectedItem, x => { return x.GetValue(); })
            //    .Subscribe( x=> 
            //    {
            //        var navNode = x as Navigator;

            //        if (navNode != null)
            //        {
            //            Publisher.Publish(navNode);
            //        }

                    //if (x is Analytic)
                    //{
                    //    var a = (Analytic) x;
                    //    var vm = new AnalyticViewModel(a);
                    //    this.ViewModel.SelectedViewModel = vm;
                    //    this.xDetailView.ViewModel = vm;
                    //}
                    //if (x is PriceRoutine)
                    //{
                    //    var p = (PriceRoutine) x;
                    //    var vm = new PriceRoutineViewModel(p);
                    //    this.ViewModel.SelectedViewModel = vm;
                    //    this.xDetailView.ViewModel = vm;
                    //}    
                        
                //});
                //.Select(x => new AnalyticViewModel(x))
                //.BindTo(this, x => x.ViewModel.SelectedViewModel);

            this.ObservableForProperty(x => x.ViewModel.SelectedViewModel)
                .Select(x => x.GetValue())
                .Subscribe(x =>
                {
                    this.xDetailView.ViewModel = x;
                });

            this.ObservableForProperty(x => x.ViewModel.ExplorerViewModel)
                .Select(x => x.GetValue())
                .Subscribe(x =>
                {
                    if (x != null)
                    {
                        ExplorerRadPane.IsHidden = false;
                    }
                    else
                    {
                        ExplorerRadPane.IsHidden = true;
                    }
                });


            

            
            //this.WhenAny(x => ViewModel.SelectedViewModel.ValidationErrors,  x => x.PropertyName)
            //    .Subscribe(
            //        prop => 
                        
            //        { FlyOut.Begin(this); }
            //    );


            Publisher.GetEvent<bool>()
                .Subscribe(x =>
                {
                    if (x == true)
                        RightPane.IsPinned = true;
                    else
                     RightPane.IsPinned = false;


                });

            Publisher.GetEvent<ErrorExistsEvent>()
                .Subscribe(_ => {
                    RightPane.IsPinned = true;
                });

            
            Publisher.GetEvent<WindowResizedEvent>()
                .Subscribe(_ =>
                {


                    ContentPresenter cpPlanningTree2 = XamlHelper.FindVisualChild<ContentPresenter>(ExplorerItemsControl);
                    if (cpPlanningTree2 != null)
                    {
                        DataTemplate dtPlanningTree2 = cpPlanningTree2.ContentTemplate;
                        var xTreeView2 = (TreeView)dtPlanningTree2.FindName("xTreeView", cpPlanningTree2);

                        double totalHeaderHeight = (3 * 32) + 125;
                        //double totalHeaderHeight = (1 * 32) + 75;
                        xTreeView2.Height = Application.Current.MainWindow.ActualHeight - totalHeaderHeight;
                    }

                    int noSteps;
                    if (this.ViewModel.SelectedViewModel.Workflow == null)
                    {
                        noSteps = 1;
                    }
                    else
                    {
                        noSteps = this.ViewModel.SelectedViewModel.Workflow.Steps.Count();
                    }
                    ContentPresenter cp = XamlHelper.FindVisualChild<ContentPresenter>(itemsControl);
                    if (cp != null && RightPane.IsPinned)
                    {
                        DataTemplate dt = cp.ContentTemplate;

                        if (dt != null)
                        {
                            
                        var sp = (StackPanel) dt.FindName("stack", cp);
                        sp.Height = Application.Current.MainWindow.ActualHeight - (noSteps * 30);



                        }

                    }


                    this.ViewModel.SelectedViewModel.RaisePropertyChanged("Workflow");
                });
        }

        
        public Boolean HasErrors { get; set; }

        public Storyboard FlyOut { get; set; }
        public Storyboard FlyOutRev { get; set; }

        public MainViewModel ViewModel
        {
            get
            {
                return (MainViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty,
                    value);
            }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(MainViewModel), typeof(MainView), new PropertyMetadata(null));

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }
        public IEventAggregator Publisher { get; set; }
        private void RadPane_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ViewModel.RaisePropertyChanged("Workflow");
        }

        //private void MainRadDocking_PaneStateChange(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    //foreach (RadPane pane in MainRadDocking.Panes)
        //    //{
        //    //    if (pane.DataContext == null)
        //    //        pane.DataContext = this.DataContext;
        //    //}
        //    //Publisher.Publish(new WindowResizedEvent());
        //}
        private void MessageCenterDocking_OnPreviewShowCompass(object sender, PreviewShowCompassEventArgs e)
        {
            
            e.Compass.IsLeftIndicatorVisible = false;
            e.Compass.IsCenterIndicatorVisible = false;
            e.Compass.IsTopIndicatorVisible = false;
            e.Compass.IsBottomIndicatorVisible = false;
            e.Compass.IsRightIndicatorVisible = true;

        }

        private void RightContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void xTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var navNode = (Navigator)e.NewValue;

            if (navNode != null)
            {
                Publisher.Publish(navNode);
            }

        }

        private void ExpanderElement_Expanded(object sender, RoutedEventArgs e)
        {

        }

        public class ExplorerExpandedEvent
        {

            public object Expander { get; set; }
        }


        private void ExpanderElement_Collapsed(object sender, RoutedEventArgs e)
        {
            Expander exp = sender as Expander;
            exp.IsExpanded = true;
        }

    }
}
