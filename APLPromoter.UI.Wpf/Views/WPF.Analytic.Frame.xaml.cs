using APLPromoter.Client.Entity;
using APLPromoter.Core.Reactive;
using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;
using System;
using Telerik.Windows.Controls;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AnalyticFrame.xaml
    /// </summary>
    public partial class AnalyticFrame : IViewFor<AnalyticViewModel>
    {
        IEventAggregator Publisher = ((ViewModelLocator)App.Current.Resources["Locator"]).EventPublisher;
        public AnalyticFrame()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            //this.Bind(ViewModel, x => x.Name, x => x.xName.Text);
            //this.Bind(ViewModel, x => x.Id, x => x.xId.Text);

            //Publisher.GetEvent<WorkflowStepType>()
            //    .Subscribe(x =>
            //    {
            //        switch (x)
            //        {
            //            case WorkflowStepType.PlanningAnalyticsMyAnalytics:
            //                TileMyAnalytics.TileState = TileViewItemState.Maximized;
            //                break;
            //            case WorkflowStepType.PlanningAnalyticsIdentity:
            //                TileAnalyticIdentity.TileState = TileViewItemState.Maximized;
            //                break;
            //            case WorkflowStepType.PlanningAnalyticsFilters:
            //                TileAnalyticFilters.TileState = TileViewItemState.Maximized;
            //                break;
            //            case WorkflowStepType.PlanningAnalyticsPriceLists:
            //                TileAnalyticPriceLists.TileState = TileViewItemState.Maximized;
            //                break;
            //            case WorkflowStepType.PlanningAnalyticsValueDrivers:
            //                TileAnalyticValueDrivers.TileState = TileViewItemState.Maximized;
            //                break;
            //            case WorkflowStepType.PlanningAnalyticsResults:
            //                TileAnalyticResults.TileState = TileViewItemState.Maximized;
            //                break;
            //        }
            //   });
        }

        public AnalyticViewModel ViewModel
        {
            get
            {
                return (AnalyticViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (AnalyticViewModel)value; }
        }
        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(AnalyticViewModel), typeof(AnalyticFrame), new PropertyMetadata(null));

        private void RadTileView_TilesStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var tile = ((RadTileViewItem)(((RadTileView)e.Source).MaximizedItem));
            if (tile != null)
            {
                Publisher.Publish<RadTileViewItem>(tile);
            }
        }
    }
}
