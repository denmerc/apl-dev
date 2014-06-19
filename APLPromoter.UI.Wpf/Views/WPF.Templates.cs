using APLPromoter.Client.Entity;
using APLPromoter.UI.Wpf.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace APLPromoter.UI.Wpf.Views
{
    public class MasterDetailTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AnalyticDetailTemplate { get; set; }
        public DataTemplate PriceRoutineDetailTemplate { get; set; }
        public DataTemplate HomeDetailTemplate { get; set; }



    }

   
}
