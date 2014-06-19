using APLPromoter.Client.Entity;
using APLPromoter.Core.Reactive;
using APLPromoter.Core.Reactive.Events;
using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace APLPromoter.UI.Wpf
{
	/// <summary>
	/// Interaction logic for WPF_Administration_Frame.xaml
	/// </summary>
	public partial class AdministrationFrame : IViewFor<AdminViewModel>
	{
        IEventAggregator Publisher = ((ViewModelLocator)App.Current.Resources["Locator"]).EventPublisher;
		public AdministrationFrame()
		{
			this.InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            Publisher.GetEvent<WorkflowStepType>()
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        //case Navigator.NavigatorViewType.PlanningDesignerUsers:
                        //    TileAdminUsers.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case Navigator.NavigatorViewType.PlanningDesignerPricelist:
                        //    TileAdminPriceLists.TileState = TileViewItemState.Maximized;
                        //    break;
                            
                        //case Navigator.NavigatorViewType.PlanningDesignerOptimization:
                        //    TileAdminOptimization.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case Navigator.NavigatorViewType.PlanningDesignerRounding:
                        //    TileAdminRounding.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case Navigator.NavigatorViewType.PlanningDesignerProcess:
                        //    TileAdminProcessing.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case Navigator.NavigatorViewType.PlanningDesignerMessage:
                        //    TileAdminMessaging.TileState = TileViewItemState.Maximized;
                        //    break;

                        //case WorkflowStepType.PlanningAdministrationUserMaintenance:
                        //    TileAdminUsers.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case WorkflowStepType.PlanningAdministrationPricelists:
                        //    TileAdminPriceLists.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case WorkflowStepType.PlanningAdministrationOptimization:
                        //    TileAdminOptimization.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case WorkflowStepType.PlanningAdministrationRounding:
                        //    TileAdminRounding.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case WorkflowStepType.PlanningAdministrationFilters:
                        //    TileAdminFilters.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case WorkflowStepType.PlanningAdministrationRollback:
                        //    TileAdminRollback.TileState = TileViewItemState.Maximized;
                        //    break;
                        //case WorkflowStepType.PlanningAdministrationProcesses:
                        //    TileAdminProcessing.TileState = TileViewItemState.Maximized;
                        //    break;


                    }           
                });
		}
        
        public AdminViewModel ViewModel
        {
            get
            {
                return (AdminViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (AdminViewModel)value; }
        }
        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(AdminViewModel), typeof(AdministrationFrame), new PropertyMetadata(null));

        private void RadTileView_TilesStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var tile = ((RadTileViewItem)(((RadTileView)e.Source).MaximizedItem));
            if (tile != null)
            {
                //Publisher.Publish<RadTileViewItem>(tile);
            }
        }


        private void RadTileView_TilesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tile = ((RadTileViewItem)e.Source);
            if (tile != null)
            {
                //Publisher.Publish<RadTileViewItem>(tile);
            }
            
        }

	}
}