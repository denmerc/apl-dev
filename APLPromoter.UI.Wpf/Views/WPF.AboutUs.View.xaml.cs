using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AboutUsView.xaml
    /// </summary>
    public partial class AboutUsView : IViewFor<AboutUsViewModel>
    {
        public AboutUsView()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            browserHost.Navigate("http://www.advancedpricinglogic.com");
        }
        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(AboutUsViewModel), typeof(AboutUsView), new PropertyMetadata(null));


        public AboutUsViewModel ViewModel
        {
            get
            {
                return (AboutUsViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (AboutUsViewModel)value; }
        }
    }
}
