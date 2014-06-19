using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;



namespace APLPromoter.UI.Wpf.Behaviors
{
    public class SelectAllTextOnFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += AssociatedObjectGotKeyboardFocus;
            AssociatedObject.GotMouseCapture += AssociatedObjectGotMouseCapture;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObjectPreviewMouseLeftButtonDown;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= AssociatedObjectGotKeyboardFocus;
            AssociatedObject.GotMouseCapture -= AssociatedObjectGotMouseCapture;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObjectPreviewMouseLeftButtonDown;
        }

        private void AssociatedObjectGotKeyboardFocus(object sender,
            System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            AssociatedObject.SelectAll();
        }

        private void AssociatedObjectGotMouseCapture(object sender,
            System.Windows.Input.MouseEventArgs e)
        {
            AssociatedObject.SelectAll();
        }

        private void AssociatedObjectPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!AssociatedObject.IsKeyboardFocusWithin)
            {
                AssociatedObject.Focus();
                e.Handled = true;
            }
        }
    }



    public static class TextBoxFocusBehavior
    {
        public static string GetWatermarkText(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkText);
        }

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkText, value);
        }

        public static bool GetIsWatermarkEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWatermarkEnabled);
        }

        public static void SetIsWatermarkEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWatermarkEnabled, value);
        }

        public static readonly DependencyProperty IsWatermarkEnabled =
            DependencyProperty.RegisterAttached("IsWatermarkEnabled",
            typeof(bool), typeof(TextBoxFocusBehavior),
            new UIPropertyMetadata(false, OnIsWatermarkEnabled));

        public static readonly DependencyProperty WatermarkText =
            DependencyProperty.RegisterAttached("WatermarkText",
            typeof(string), typeof(TextBoxFocusBehavior),
            new UIPropertyMetadata(string.Empty, OnWatermarkTextChanged));

        private static void OnWatermarkTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = (string)e.NewValue;
            }
        }

        private static void OnIsWatermarkEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                bool isEnabled = (bool)e.NewValue;
                if (isEnabled)
                {
                    //tb.GotFocus += OnInputTextBoxGotFocus;
                    tb.LostFocus += OnInputTextBoxLostFocus;
                }
                else
                {
                    //tb.GotFocus -= OnInputTextBoxGotFocus;
                    tb.LostFocus -= OnInputTextBoxLostFocus;
                }
            }
        }

        private static void OnInputTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as TextBox;
            if (tb != null)
            {
                if (!string.IsNullOrEmpty(tb.Text))
                    tb.Style = null;
                    //tb.Text = GetWatermarkText(tb);
            }
        }

        private static void OnInputTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as TextBox;
            if (tb != null)
            {
                if (tb.Text == GetWatermarkText(tb))
                    tb.Text = string.Empty;
            }
        }
    }

    public static class PasswordBoxFocusBehavior
    {
        public static string GetWatermarkText(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkText);
        }

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkText, value);
        }

        public static bool GetIsWatermarkEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWatermarkEnabled);
        }

        public static void SetIsWatermarkEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWatermarkEnabled, value);
        }

        public static readonly DependencyProperty IsWatermarkEnabled =
            DependencyProperty.RegisterAttached("IsWatermarkEnabled",
            typeof(bool), typeof(PasswordBoxFocusBehavior),
            new UIPropertyMetadata(false, OnIsWatermarkEnabled));

        public static readonly DependencyProperty WatermarkText =
            DependencyProperty.RegisterAttached("WatermarkText",
            typeof(string), typeof(PasswordBoxFocusBehavior),
            new UIPropertyMetadata(string.Empty, OnWatermarkTextChanged));

        private static void OnWatermarkTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox tb = sender as PasswordBox;
            if (tb != null)
            {
                tb.Password = (string)e.NewValue;
            }
        }

        private static void OnIsWatermarkEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox tb = sender as PasswordBox;
            if (tb != null)
            {
                bool isEnabled = (bool)e.NewValue;
                if (isEnabled)
                {
                    //tb.GotFocus += OnInputPasswordBoxGotFocus;
                    tb.LostFocus += OnInputPasswordBoxLostFocus;
                }
                else
                {
                    //tb.GotFocus -= OnInputPasswordBoxGotFocus;
                    tb.LostFocus -= OnInputPasswordBoxLostFocus;
                }
            }
        }

        private static void OnInputPasswordBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as PasswordBox;
            if (tb != null)
            {
                if (!string.IsNullOrEmpty(tb.Password))
                {
                    tb.Style = null;
                }
                //if (string.IsNullOrEmpty(tb.Password))
                //    tb.Password = GetWatermarkText(tb);
            }
        }

        private static void OnInputPasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as PasswordBox;
            var style = tb.Style;
            if (tb != null)
            {
                if (tb.Password == GetWatermarkText(tb))
                    tb.Password = string.Empty;
            }
        }
    }
}



