using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace APLPromoter.UI.Wpf.Controls
{
	public class APLWindowBase : Window
	{
		public enum enumAPLWindowStyle
		{ 
			Application,
			ToolWindow,
			WorkspaceWindow
		}
		
		//private UIElement _titlebarBg;
        private ContentControl _titlebarBg;
        private ContentControl _workspaceWindowDragGrip;
        private Button _minimizeWindowButton;
        private Button _maximizeWindowButton;
        private Button _restoreWindowButton;
        private Button _closeWindowButton;
        private Button _workspaceWindowCloseButton;
        private Point _lastLeftMouseDownPosition = new Point();
        private bool _isMouseDownOnTitlebar;
        private bool _mouseWasDoubleClicked;

        private WindowView _windowView;
		
		 #region Custom Events

        public static readonly RoutedEvent APLLogoMouseDownEvent = EventManager.RegisterRoutedEvent(
            "APLLogoMouseDown", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(APLWindowBase));

        public event RoutedEventHandler ALPLogoMouseDown
        {
            add { AddHandler(APLLogoMouseDownEvent, value); }
            remove { RemoveHandler(APLLogoMouseDownEvent, value); }
        }


        public static readonly RoutedEvent APLLogoMouseEnterEvent = EventManager.RegisterRoutedEvent(
            "APLLogoMouseEnter", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(APLWindowBase));

        public event RoutedEventHandler APLLogoMouseEnter
        {
            add { AddHandler(APLLogoMouseEnterEvent, value); }
            remove { RemoveHandler(APLLogoMouseEnterEvent, value); }
        }


        public static readonly RoutedEvent APLLogoMouseLeaveEvent = EventManager.RegisterRoutedEvent(
            "APLLogoMouseLeave", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(APLWindowBase));

        public event RoutedEventHandler APLLogoMouseLeave
        {
            add { AddHandler(APLLogoMouseLeaveEvent, value); }
            remove { RemoveHandler(APLLogoMouseLeaveEvent, value); }
        }
        
        #endregion

        [Category("Common Properties"), Description("Detail text that appears next to window title on mouse hover")]
        public string TitleHoverDetail
        {
            get { return (string)GetValue(TitleHoverDetailProperty); }
            set { SetValue(TitleHoverDetailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleHoverDetail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleHoverDetailProperty =
            DependencyProperty.Register("TitleHoverDetail", typeof(string), typeof(APLWindowBase), new UIPropertyMetadata(""));


        [Category("Common Properties")]
        public Visibility TitleHoverDetailVisibility
        {
            get { return (Visibility)GetValue(TitleHoverDetailVisibilityProperty); }
            set { SetValue(TitleHoverDetailVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleHoverDetailVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleHoverDetailVisibilityProperty =
            DependencyProperty.Register("TitleHoverDetailVisibility", typeof(Visibility), typeof(APLWindowBase), new UIPropertyMetadata(Visibility.Visible));


        [Category("Common Properties")]
        public double TitleTextOpacity
        {
            get { return (double)GetValue(TitleTextOpacityProperty); }
            set { SetValue(TitleTextOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleTextOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleTextOpacityProperty =
            DependencyProperty.Register("TitleTextOpacity", typeof(double), typeof(APLWindowBase), new UIPropertyMetadata((double)1.0, null, TitleTextOpacityCoerceCallback));

        private static object TitleTextOpacityCoerceCallback(DependencyObject obj, object o) 
        {
            // Coerce to use valid range only
            //
            double v = (double)o;
            v = v < 0 ? 0 : v;
            v = v > 1 ? 1 : v;
            return v;
        }

        [Category("Appearance")]
        public ImageSource WindowBackgroundImage
        {
            get { return (ImageSource)GetValue(WindowBackgroundImageProperty); }
            set { SetValue(WindowBackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowBackgroundImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowBackgroundImageProperty =
            DependencyProperty.Register("WindowBackgroundImage", typeof(ImageSource), typeof(APLWindowBase),
                new UIPropertyMetadata(null));

        // TODO: Hide the inherited WindowStyle from the Blend property menu
        //
        [Category("Appearance"), Description("Replaces the WindowStyle")]
        public enumAPLWindowStyle APLWindowStyle
        {
            get { return (enumAPLWindowStyle)GetValue(APLWindowStyleProperty); }
            set { SetValue(APLWindowStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty APLWindowStyleProperty =
            DependencyProperty.Register("APLWindowStyle", typeof(enumAPLWindowStyle), typeof(APLWindowBase),
                new UIPropertyMetadata(enumAPLWindowStyle.ApplicationWindow));
                // Note: If you change the ApplicationWindow default, be sure to add its Property Trigger handling in XAML.

        [Category("Appearance"), Description("Apply Halloween Theme on Application APL Window Style. Very SCARY!!!")]
        public bool HalloweenTheme
        {
            get { return (bool)GetValue(HalloweenThemeProperty); }
            set { SetValue(HalloweenThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HalloweenThemeProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HalloweenThemeProperty =
            DependencyProperty.Register("HalloweenTheme", typeof(bool), typeof(APLWindowBase), 
                new UIPropertyMetadata(false));


        
        public APLWindowBase()
        {
            this.Loaded += new RoutedEventHandler(eventHandler_Loaded);
        }

        public WindowView WindowView
        {
            get { return _windowView; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _workspaceWindowDragGrip = Template.FindName("PART_DragGrip", this) as ContentControl;

            if (_workspaceWindowDragGrip != null)
            {
                _workspaceWindowDragGrip.MouseMove += (sender, args) =>
                {
                    if (args.LeftButton == MouseButtonState.Pressed)
                    {
                        if (WindowState == WindowState.Maximized)
                        {
                            WindowState = WindowState.Normal;
                            Top = 0;
                            Left = 0;
                        }

                        try
                        {
                            DragMove();
                        }
                        catch (System.Exception ex)
                        {
                            // A Quirk on DragMove() where it sometimes complains about mouse is 
                            // not down regardless of the MouseButtonState.Pressed above
                            //
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                        }
                    }
                };
            }

            // Support window dragging via titlebar.
            // Template MUST contain control named 'PART_APLTitlebar'.
            //
            _titlebarBg = Template.FindName("PART_APLTitlebar", this) as ContentControl;

            if (_titlebarBg != null)
            {
                _titlebarBg.PreviewMouseLeftButtonDown += (sender, args) =>
                {
                    _lastLeftMouseDownPosition = args.GetPosition(this);
                    _isMouseDownOnTitlebar = true;
                    // This _isMouseDownOnTitlebar flag is needed to prevent the following quirk:
                    // - While the Main Window is maximized, open a Main Menu SubItem
                    // - Navigate the mouse to click somewhere in titlebar (just once) while the submenu remains opened
                    // - Result: Window is restored to its original size
                    // The above triggers the mouse move event before the PreviewMouseLeftButtonDown;
                    // thus, the mouse-position-move-while-pressed check fails.
                };

                _titlebarBg.PreviewMouseLeftButtonUp += (sender, args) =>
                {
                    _isMouseDownOnTitlebar = false;
                };

                _titlebarBg.MouseLeftButtonDown += (sender, args) => 
                {
                    if (_mouseWasDoubleClicked == false)
                    {
                        try
                        {
                            DragMove();
                        }
                        catch (System.Exception ex)
                        {
                            // A Quirk on DragMove(), in a very rare occassion, complains about mouse is 
                            // not down regardless of the MouseButtonState.Pressed above
                            //
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                        }
                    }
                    else if (WindowState == WindowState.Normal)
                    {
                        _mouseWasDoubleClicked = false;
                    }
                };

                _titlebarBg.MouseDoubleClick += (sender, args) => 
                {
                    _mouseWasDoubleClicked = true;
                    // May, 2010:
                    // This flag is used to block Drag during the mouse double click.
                    //
                    // Had tried resetting the double-click flag on Window StateChanged, but
                    // that event will happen before the window is in the desired final state.
                    //
                    // An alternative implementation would be to reset this flag after x ms
                    // via a timer event. However, the current implementation works fine.
                    //

                    if (WindowState == WindowState.Normal)
                    {
                        WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        WindowState = WindowState.Normal;
                    }
                };
                
                _titlebarBg.MouseMove += (sender, args) =>
                {
                    // supports dragging while window is maxed
                    //
                    if ( (args.LeftButton == MouseButtonState.Pressed) && (WindowState == WindowState.Maximized) )
                    {
                        // ignore mouse move during the resizing due to double click
                        //
                        if ( ( _mouseWasDoubleClicked == false ) && 
                             ( _isMouseDownOnTitlebar == true) )
                        {
                            // Ensure the mouse move occurs while it is pressed
                            //
                            Point currentLeftMouseDownPosition = args.GetPosition(this);
                            if ((Math.Abs(currentLeftMouseDownPosition.X - _lastLeftMouseDownPosition.X) > 5) ||
                                 (Math.Abs(currentLeftMouseDownPosition.Y - _lastLeftMouseDownPosition.Y) > 5))
                            {
                                WindowState = WindowState.Normal;
                                Top = 0;
                                try
                                {
                                    DragMove();
                                }
                                catch (System.Exception ex)
                                {
                                    // A Quirk on DragMove() where it sometimes complains about mouse is 
                                    // not down regardless of the MouseButtonState.Pressed above
                                    //
                                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                                }
                            }
                        }
                        else
                        {
                            _mouseWasDoubleClicked = false;
                        }
                        
                    }
                };
            }

            // supports APL Logo Popup Menu
            //
            APLLogo Alogo = Template.FindName("PART_APLLogo", this) as APLLogo;

            if (Alogo != null)
            {
                Alogo.MouseDown += (sender, args) =>
                {
                    RoutedEventArgs newEventArgs = new RoutedEventArgs(APLWindowBase.APLLogoMouseDownEvent);
                    RaiseEvent(newEventArgs);
                };

                Alogo.MouseEnter += (sender, args) =>
                {
                    RoutedEventArgs newEventArgs = new RoutedEventArgs(APLWindowBase.APLLogoMouseEnterEvent);
                    RaiseEvent(newEventArgs);
                };

                Alogo.MouseLeave += (sender, args) =>
                {
                    RoutedEventArgs newEventArgs = new RoutedEventArgs(APLWindowBase.APLLogoMouseLeaveEvent);
                    RaiseEvent(newEventArgs);
                };
            }

            // Handle Min/Max/Restore button clicks
            //
            _minimizeWindowButton = Template.FindName("PART_MinimizeWindowButton", this) as Button;

            if (_minimizeWindowButton != null)
            {
                _minimizeWindowButton.Click += (sender, args) => { WindowState = WindowState.Minimized; };
            }

            _maximizeWindowButton = Template.FindName("PART_MaximizeWindowButton", this) as Button;

            if (_maximizeWindowButton != null)
            {
                _maximizeWindowButton.Click += (sender, args) => { WindowState = WindowState.Maximized; };
            }

            _restoreWindowButton = Template.FindName("PART_RestoreWindowButton", this) as Button;

            if (_restoreWindowButton != null)
            {
                _restoreWindowButton.Click += (sender, args) => { WindowState = WindowState.Normal; };
            }

            _closeWindowButton = Template.FindName("PART_CloseWindowButton", this) as Button;

            if (_closeWindowButton != null)
            {
                _closeWindowButton.Click += (sender, args) => { Close(); };
            }

            _workspaceWindowCloseButton = Template.FindName("PART_WorkspaceWindowCloseButton", this) as Button;

            if (_workspaceWindowCloseButton != null)
            {
                _workspaceWindowCloseButton.Click += (sender, args) => { Close(); };
            }
        }

        private void eventHandler_Loaded(object sender, RoutedEventArgs e)
        {
            // This data source should already be set in XAML.
            // Note: Some Console might not have WindowView type set as its Context. Leave it null.
            // 
            _windowView = this.DataContext as WindowView;
            
            if (_windowView != null)
            {
                _windowView.Window = this;
            }

            onLoaded();
        }
	}
}