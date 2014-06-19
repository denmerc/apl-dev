using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace APLPromoter.UI.Wpf.Controls
{
	public class CustomPopup : FrameworkElement
    {
        #region Fields

        private Window window;
        Timer timer;

        #endregion

        #region Constructors

        public CustomPopup()
        {
            this.Unloaded += new RoutedEventHandler(CustomPopup_Unloaded);
        }

        #endregion

        #region Properties

        public int HorizontalOffset
        {
            get { return (int)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public int VerticalOffset
        {
            get { return (int)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        public UIElement PlacementTarget
        {
            get { return (UIElement)GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }

        public bool ShiftResizeDirection
        {
            get { return (bool)GetValue(ShiftResizeDirectionProperty); }
            set { SetValue(ShiftResizeDirectionProperty, value); }
        }

        public DataTemplate Template
        {
            get { return (DataTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }

        public int StayOpenDuration
        {
            get { return (int)GetValue(StayOpenDurationProperty); }
            set { SetValue(StayOpenDurationProperty, value); }
        }

        #endregion

        #region Dependancy Properties

        /// <summary>
        /// This property is used to open and close the winodw.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(CustomPopup), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, CallbackOnIsOpen));

        /// <summary>
        /// Get and Set the Horizontal Offset.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(int), typeof(CustomPopup), new UIPropertyMetadata(0));

        /// <summary>
        /// Get and Set the Vertical Offset.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(int), typeof(CustomPopup), new UIPropertyMetadata(0));

        /// <summary>
        /// Get and Set the Placement of the Popup with respect to Placement Target element.
        /// </summary>
        public static readonly DependencyProperty PlacementTargetProperty =
            DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(CustomPopup), new UIPropertyMetadata(null));

        /// <summary>
        /// The template for the Popup window.
        /// </summary>
        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.Register("Template", typeof(DataTemplate), typeof(CustomPopup), new UIPropertyMetadata(null));

        /// <summary>
        /// get and Set the UIElement used to place the popup window.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(CustomPopup), new UIPropertyMetadata(PlacementMode.Top));

        /// <summary>
        /// Get and set the Window resize direction.
        /// </summary>
        public static readonly DependencyProperty ShiftResizeDirectionProperty =
            DependencyProperty.Register("ShiftResizeDirection", typeof(bool), typeof(CustomPopup), new UIPropertyMetadata(false));

        /// <summary>
        /// Get and set Stay Open Duration value. If this value is set popup window closes in the time specified. The time is defined in milliseconds
        /// </summary>
        public static readonly DependencyProperty StayOpenDurationProperty =
            DependencyProperty.Register("StayOpenDuration", typeof(int), typeof(CustomPopup), new UIPropertyMetadata(0));

        #endregion

        #region Static

        private static void CallbackOnIsOpen(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CustomPopup view = sender as CustomPopup;

            if (view != null)
            {
                view.Activate(view.IsOpen);
            }
        }

        public static Rect GetTotalScreenArea()
        {
            Rect screenArea = new Rect();

            foreach(Screen s in Screen.AllScreens)
            {
                if (s.Bounds.Height > screenArea.Height)
                {
                    screenArea.Height = s.Bounds.Height;
                }
                screenArea.Width += s.Bounds.Width;
            }
            return screenArea;
        }

        public static bool SetWindowLocation(PlacementMode placement, Window owner, Window popup, UIElement placementTarget, double verticalOffset, double horizontalOffset)
        {
            if (!placementTarget.IsVisible)
                return false;
            try
            {
                if (owner.WindowState != WindowState.Minimized)
                {
                    double top = 0;
                    double left = 0;
                    Point point = new Point(0, 0);
                    Rect screenRect = GetTotalScreenArea();

                    if (placementTarget != null)
                    {
                        point = placementTarget.PointToScreen(new Point(0, 0));
                    }
                    switch (placement)
                    {
                        case PlacementMode.Top:
                            top = point.Y - verticalOffset - popup.RenderSize.Height;
                            left = point.X + horizontalOffset;
                            break;
                        case PlacementMode.Bottom:
                            top = point.Y + verticalOffset + placementTarget.RenderSize.Height + popup.RenderSize.Height;
                            left = point.X + horizontalOffset;
                            break;
                        case PlacementMode.Right:
                            top = point.Y + verticalOffset;
                            left = point.X + horizontalOffset + placementTarget.RenderSize.Width;
                            break;
                        case PlacementMode.Left:
                            top = point.Y + verticalOffset;
                            left = point.X + horizontalOffset - popup.RenderSize.Width;
                            break;
                        case PlacementMode.Absolute:
                            top = verticalOffset;
                            left = horizontalOffset;
                            break;
                        case PlacementMode.Relative:
                            top = point.Y + verticalOffset;
                            left = point.X + horizontalOffset;
                            break;
                    }
                    if (placement != PlacementMode.Absolute && placement != PlacementMode.Relative)
                    {
                        if ((top + popup.RenderSize.Height) > screenRect.Bottom && ((screenRect.Bottom - point.Y - placementTarget.RenderSize.Height) < (point.Y - screenRect.Top)))
                        {
                            top = point.Y - verticalOffset - popup.RenderSize.Height;
                        }
                        if (top < screenRect.Top && ((screenRect.Bottom - point.Y - placementTarget.RenderSize.Height) > (point.Y - screenRect.Top)))
                        {
                            top = point.Y + placementTarget.RenderSize.Height;
                        }
                        if ((left + popup.RenderSize.Width) > screenRect.Right && ((screenRect.Right - point.X - placementTarget.RenderSize.Width) < (point.X - screenRect.Left)))
                        {
                            left = point.X - popup.RenderSize.Width;
                        }
                        if (left < screenRect.Left && ((screenRect.Right - point.X - placementTarget.RenderSize.Width) > (point.X - screenRect.Left)))
                        {
                            left = point.X + placementTarget.RenderSize.Width;
                        }
                    }
                    popup.Left = left;
                    popup.Top = top;
                }
            }
            catch
            {
                //Sometime the Visual of target placement will not be generated
            }
            return true;
        }


        #endregion

        #region Methods

        public void Activate()
        {
            Activate(true);
        }

        public void Activate(bool activate)
        {
            if (activate)
            {
                if (window == null || !window.IsLoaded)
                {
                    window = new Window();
                    window.Owner = App.Current.MainWindow;

                    //This is done because window is taking a default size, if content is less than the default size
                    window.Width = 0;
                    window.Height = 0;
                    window.Style = App.Current.FindResource("BlankWindowStyle") as Style;
                    window.ResizeMode = ResizeMode.NoResize;
                    window.SizeToContent = SizeToContent.WidthAndHeight;
                    window.Content = this.DataContext;
                    window.DataContext = this.DataContext;
                    window.ContentTemplate = this.Template;
                    window.Closed += new System.EventHandler(window_Closed);
                    window.Loaded += new RoutedEventHandler(window_Loaded);
                    window.SizeChanged += new SizeChangedEventHandler(window_SizeChanged);
                    window.Owner.StateChanged += new System.EventHandler(Owner_StateChanged);
                    window.Owner.SizeChanged += new SizeChangedEventHandler(Owner_SizeChanged);
                    window.Owner.LocationChanged += new System.EventHandler(Owner_LocationChanged);
                    window.Show();
                }
                else if (window != null)
                {
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                }
                if (window != null && window.Owner.WindowState == WindowState.Minimized)
                {
                    window.Owner.Show();
                }
            }
            else if (window != null)
            {
                window.Close();
            }
        }

        private void UnSubscribeWindowEvents()
        {
            if (window != null)
            {
                window.SizeChanged -= new SizeChangedEventHandler(window_SizeChanged);
                window.Owner.SizeChanged -= new SizeChangedEventHandler(Owner_SizeChanged);
                window.Owner.LocationChanged -= new System.EventHandler(Owner_LocationChanged);
                window.Owner.StateChanged -= new System.EventHandler(Owner_StateChanged);
                window.Loaded -= new RoutedEventHandler(window_Loaded);
                window.Closed -= new System.EventHandler(window_Closed);
            }
        }

        void Owner_StateChanged(object sender, System.EventArgs e)
        {
            SetWindowLocation(Placement, window.Owner, window, PlacementTarget, VerticalOffset, HorizontalOffset);
        }

        private void CustomPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded += new RoutedEventHandler(CustomPopup_Unloaded);
            if (window != null)
            {
                window.Close();
            }
            this.ClearValue(IsOpenProperty);
        }

        private void Owner_LocationChanged(object sender, System.EventArgs e)
        {
            SetWindowLocation(Placement, window.Owner, window, PlacementTarget, VerticalOffset, HorizontalOffset);
        }

        private void Owner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetWindowLocation(Placement, window.Owner, window, PlacementTarget, VerticalOffset, HorizontalOffset);
        }

        private void SetWindowPosition()
        {
            if (PlacementTarget.IsVisible)
            {
                SetWindowLocation(Placement, window.Owner, window, PlacementTarget, VerticalOffset, HorizontalOffset);
                StartTimer();
            }
            else
            {
                PlacementTarget.LayoutUpdated += new EventHandler(PlacementTarget_LayoutUpdated);
            }
        }

        void PlacementTarget_LayoutUpdated(object sender, EventArgs e)
        {
            PlacementTarget.LayoutUpdated -= new EventHandler(PlacementTarget_LayoutUpdated);
            SetWindowLocation(Placement, window.Owner, window, PlacementTarget, VerticalOffset, HorizontalOffset);
            StartTimer();
        }

        private void window_Closed(object sender, System.EventArgs e)
        {
            UnSubscribeWindowEvents();
            if (timer != null && timer.Enabled)
            {
                timer.Tick -= new System.EventHandler(timer_Tick);
                timer.Stop();
                timer.Dispose();
            }
            if (IsOpen)
            {
                SetCurrentValue(IsOpenProperty, false);
            }
            if (window.Owner != null && window.Owner.IsLoaded)
            {
                window.Owner.Activate();
            }
            window = null;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowPosition();
        }

        private void StartTimer()
        {
            if (StayOpenDuration > 0 && (timer == null || !timer.Enabled))
            {
                timer = new Timer();
                timer.Interval = StayOpenDuration;
                timer.Tick += new System.EventHandler(timer_Tick);
                timer.Start();
            }
        }

        void timer_Tick(object sender, System.EventArgs e)
        {
            timer.Tick -= new System.EventHandler(timer_Tick);
            timer.Stop();
            IsOpen = false;
            timer.Dispose();
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetWindowPosition();
            if (ShiftResizeDirection)
            {
                if (e.PreviousSize.Width > e.NewSize.Width)
                {
                    window.Left += e.PreviousSize.Width - e.NewSize.Width;
                }
                else
                {
                    window.Left -= e.NewSize.Width - e.PreviousSize.Width;
                }
            }
        }

        #endregion
    }
}