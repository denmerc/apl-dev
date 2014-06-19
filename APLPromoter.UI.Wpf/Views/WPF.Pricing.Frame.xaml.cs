using APLPromoter.UI.Wpf.ViewModel;
using System.Windows;
using ReactiveUI;
using Telerik.Windows.Controls;
using APLPromoter.Core.Reactive;

namespace APLPromoter.UI.Wpf
{
	/// <summary>
	/// Interaction logic for WPF_Pricing_Frame.xaml
	/// </summary>
    /// 
	public partial class PricingFrame : IViewFor<PriceRoutineViewModel>
	{
        IEventAggregator Publisher = ((ViewModelLocator)App.Current.Resources["Locator"]).EventPublisher;
		public PricingFrame()
		{
			this.InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);


		}

        public PriceRoutineViewModel ViewModel
        {
            get
            {
                return (PriceRoutineViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (PriceRoutineViewModel)value; }
        }
        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(PriceRoutineViewModel), typeof(PricingFrame), new PropertyMetadata(null));
        
        
        
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