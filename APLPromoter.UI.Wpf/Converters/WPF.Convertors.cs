using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace APLPromoter.UI.Wpf.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;       // Default non-True value results in Collapsed

            if (parameter != null)
            {
                //
                // If parameter was specified, use that for Non-True value results
                //

                if ((string)parameter == "Hidden")
                {
                    result = Visibility.Hidden;
                }
            }

            if (value != null &&
                (Boolean)value)
            {
                result = Visibility.Visible;                // Default visibility is Visible when value is True
            }

            return result;

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Boolean result = false;                         // Default non-True value results in False

            if (value != null &&
                (Visibility)value == Visibility.Visible)
            {
                result = true;
            }

            return result;
        }
    }

    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Visible;         // Default non-True value results in Visible

            if (value != null &&
                (Boolean)value)
            {
                result = Visibility.Collapsed;              // Default True value results in Collapsed

                if (parameter != null)
                {
                    //
                    // If parameter was specified, use that for True value results
                    //

                    if ((string)parameter == "Hidden")
                    {
                        result = Visibility.Hidden;
                    }
                }
            }

            return result;

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Boolean result = false;                         // Default non-True value results in False

            try
            {
                if (value != null &&
                    (Visibility)value != Visibility.Visible)
                {
                    result = true;
        }
    }
            finally { };

            return result;

        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    public class DebugConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.Print("DebugConverter.Convert: value = {0}", value);
            return value;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.Print("DebugConverter.ConvertBack: value = {0}", value);
            return value;
        }
    }

    [ValueConversion(typeof(Color), typeof(Color))]
    public class ColorAlphaConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;

            if (parameter != null)
            {
                double alphaPercentage = Double.Parse((string)parameter);
                if (alphaPercentage >= 0.0 && alphaPercentage <= 255.0)
                {
                    double alphaValue = 255.0 * alphaPercentage;

                    color.A = byte.Parse(alphaValue.ToString());
                }
            }

            return color;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return new SolidColorBrush((Color)value);

            return value;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((SolidColorBrush)value).Color;

            return value;
        }
    }

    
    [ValueConversion(typeof(double), typeof(GridLength))]
    public class DoubleToGridLengthConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            var w = (APLPromoter.Client.Entity.Workflow)value;
            if (w != null)
            {

                double totalHeaderHeight = (w.Steps.Count * 32) + 120;
                    
                //double totalHeaderHeight = 150;
                //return new GridLength(SystemParameters.PrimaryScreenHeight - totalHeaderHeight, GridUnitType.Pixel);

                return Application.Current.MainWindow.ActualHeight - totalHeaderHeight;
            }
            else
                return Application.Current.MainWindow.ActualHeight;       
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Double result = double.NaN;

            if (value == null)
            {
                throw new ArgumentNullException("DoubleToGridLengthConverter value argument cannot be null.");
            }

            try
            {
                // This converter only supports Absolute Pixel grid length conversions.
                //
                GridLength gridLengthValue = (GridLength)value;
                if (gridLengthValue.GridUnitType != GridUnitType.Pixel)
                {
                    throw new ArgumentException("DoubleToGridLengthConverter only supports Absolute Pixel length conversions.");
                }

                result = (double)gridLengthValue.Value;
            }
            catch
            {
                try
                {
                    // Try using default value, if provided
                    //
                    GridLength testGridLength = new GridLength((double)parameter, GridUnitType.Pixel);
                    result = (double)parameter;
                }
                finally
                {
                }
            }

            return result;
        }
    }


    [ValueConversion(typeof(double), typeof(GridLength))]
    public class ExplorerLengthConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {


            //double totalHeaderHeight = 152;
            double totalHeaderHeight = 222;
                return Application.Current.MainWindow.ActualHeight - totalHeaderHeight;

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Double result = double.NaN;

            if (value == null)
            {
                throw new ArgumentNullException("DoubleToGridLengthConverter value argument cannot be null.");
            }

            try
            {
                // This converter only supports Absolute Pixel grid length conversions.
                //
                GridLength gridLengthValue = (GridLength)value;
                if (gridLengthValue.GridUnitType != GridUnitType.Pixel)
                {
                    throw new ArgumentException("DoubleToGridLengthConverter only supports Absolute Pixel length conversions.");
                }

                result = (double)gridLengthValue.Value;
            }
            catch
            {
                try
                {
                    // Try using default value, if provided
                    //
                    GridLength testGridLength = new GridLength((double)parameter, GridUnitType.Pixel);
                    result = (double)parameter;
                }
                finally
                {
                }
            }

            return result;
        }
    }



    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullVisibilityConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;  // Default non-True value results in Collapsed

            if (parameter != null)
            {
                //
                // If parameter was specified, use that for Non-True value results
                //

                if ((string)parameter == "Hidden")
                {
                    result = Visibility.Hidden;
                }
            }

            if (value != null)
            {
                result = Visibility.Visible;  // Default visibility is Visible when value is not null
            }

            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }


    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullInvisibilityConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Visible;  // Default non-True value results in Collapsed



            if (value != null)
            {
                result = Visibility.Collapsed;  // Default visibility is Visible when value is not null
            }

            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }


        [ValueConversion(typeof(object), typeof(bool))]
        public class NullToBoolValueConverter : IValueConverter
        {


            public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
            {
                bool result = value == null ? true : false;
                if (parameter != null)
                    return !result;
                return result;
            }

            public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


    public class EmptyListVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            else
            {
                ICollection list = value as ICollection;
                if (list != null)
                {
                    if (list.Count == 0)
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class EmptyListToBooleanConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            else
            {
                ICollection list = value as ICollection;
                if (list != null)
                {
                    if (list.Count == 0)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
        }

            public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class TextInputToVisibilityConverter : IMultiValueConverter
    {




        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            // Always test MultiValueConverter inputs for non-null
            // (to avoid crash bugs for views in the designer)
            if (values[0] is bool && values[1] is bool)
            {
                bool hasText = !(bool)values[0];
                bool hasFocus = (bool)values[1];

                if (hasFocus || hasText)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}







