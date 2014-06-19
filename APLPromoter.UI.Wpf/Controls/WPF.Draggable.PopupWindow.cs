using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Threading;

namespace APLPromoter.UI.Wpf.Controls
{
		[ContentProperty( "Window" )]
	public class DragablePopupWindow : TriggerAction<FrameworkElement>
	{
		public event EventHandler Activated = delegate { } , Deactivated = delegate { };
		
		#region Properties
		/// <summary>
		/// Get and Set the Horizontal Offset.
		/// </summary>
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(int), typeof(DragablePopupWindow), new UIPropertyMetadata(0));
		public int HorizontalOffset
		{
			get { return (int)GetValue(HorizontalOffsetProperty); }
			set { SetValue(HorizontalOffsetProperty, value); }
		}
		
		/// <summary>
		/// get and Set the UIElement used to place the popup window.
		/// </summary>
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(DragablePopupWindow), new UIPropertyMetadata(PlacementMode.Top));
		public PlacementMode Placement
		{
			get { return (PlacementMode)GetValue(PlacementProperty); }
			set { SetValue(PlacementProperty, value); }
		}
		
		/// <summary>
		/// Get and Set the Placement of the Popup with respect to Placement Target element.
		/// </summary>
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(DragablePopupWindow), new UIPropertyMetadata(null));
		public UIElement PlacementTarget
		{
			get { return (UIElement)GetValue(PlacementTargetProperty); }
			set { SetValue(PlacementTargetProperty, value); }
		}
		
		/// <summary>
		/// Get and set the Window resize direction.
		/// </summary>
		public static readonly DependencyProperty ShiftResizeDirectionProperty = DependencyProperty.Register("ShiftResizeDirection", typeof(bool), typeof(DragablePopupWindow), new UIPropertyMetadata(false));
		public bool ShiftResizeDirection
		{
			get { return (bool)GetValue(ShiftResizeDirectionProperty); }
			set { SetValue(ShiftResizeDirectionProperty, value); }
		}
		
		/// <summary>
		/// Get and Set the Vertical Offset.
		/// </summary>
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(int), typeof(DragablePopupWindow), new UIPropertyMetadata(0));
		public int VerticalOffset
		{
			get { return (int)GetValue(VerticalOffsetProperty); }
			set { SetValue(VerticalOffsetProperty, value); }
		}
		
		public static readonly DependencyProperty WindowProperty = DependencyProperty.Register( "Window", typeof(Window), typeof(DragablePopupWindow) );
		public Window Window
		{
			get { return (Window)GetValue( WindowProperty ); }
			set { SetValue( WindowProperty, value ); }
		}

		public static readonly DependencyProperty OwnerWindowProperty = DependencyProperty.Register( "OwnerWindow", typeof(Window), typeof(DragablePopupWindow), null );
		public Window OwnerWindow
		{
			get { return (Window)GetValue( OwnerWindowProperty ); }
			set { SetValue( OwnerWindowProperty, value ); }
		}
		#endregion

		#region Event Handlers
		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			// MessagingService State is checked beacuse if the MainWindow is minimized, then we won't get exact placement.
			switch ( Window.Owner.WindowState )
			{
				case WindowState.Minimized:
					Window.Owner.StateChanged += OwnerStateChanged;
					break;
				default:
					SetWindowPosition();
					break;
			}

			if ( ShiftResizeDirection )
			{
				Window.SizeChanged += ( s, a ) => Window.Left += a.PreviousSize.Width > a.NewSize.Width ? a.PreviousSize.Width - a.NewSize.Width : - ( a.NewSize.Width - a.PreviousSize.Width );
			}
		}

		private void OwnerStateChanged(object sender, EventArgs e)
		{
			Window.Owner.StateChanged -= OwnerStateChanged;
			SetWindowPosition();
		}

		private void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate
			{
				Deactivate();
				return null;
			}, null); 
			cancelEventArgs.Cancel = true;
		}

		#endregion

		#region Functions

		private void Activate()
		{
			Window.WindowState = WindowState.Normal;
			Window.Show();
			Window.Activate();
			
			switch ( Window.Owner.WindowState )
			{
				case WindowState.Minimized:
					Window.Owner.Show();
					break;
			}
			Activated( this, EventArgs.Empty );
		}

		private void Deactivate()
		{
			if ( Window.IsVisible )
			{
				Window.Hide();
				Window.Owner.Activate();
			}
			Deactivated( this, EventArgs.Empty );
		}

		private static bool DetermineParameter(object parameter)
		{
			var result = !(parameter is DependencyPropertyChangedEventArgs) || Convert.ToBoolean( ((DependencyPropertyChangedEventArgs)parameter).NewValue );
			return result;
		}

		protected override void Invoke( object parameter )
		{
            if ( !DesignerProperties.GetIsInDesignMode( AssociatedObject ?? new DependencyObject() ) )
            {
			    var determined = DetermineParameter( parameter );
			    if ( determined && !Window.IsVisible )
			    {
				    Activate();
			    }
			    else if ( !determined && Window.IsVisible )
			    {
				    Deactivate();
			    }
            }
		}

		protected override void OnAttached()
		{
			AssociatedObject.Unloaded += ( s, a ) => Deactivate();
			Window.Loaded += WindowLoaded; // This is done because window is taking a default size, if content is less than the default size.
			Window.Closing += WindowOnClosing;

			if ( OwnerWindow != null )
			{
				if ( OwnerWindow.IsLoaded )
				{
					Window.Owner = OwnerWindow;
				}
				else
				{
					OwnerWindow.Loaded += InstanceOnLoaded;
				}
			}

			base.OnAttached();
		}

		private void InstanceOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			OwnerWindow.Loaded -= InstanceOnLoaded;                                                                                                                               
			Window.Owner = OwnerWindow;
		}

		private void PlacementTargetLayoutUpdated(object sender, EventArgs e)
		{
			PlacementTarget.LayoutUpdated -= PlacementTargetLayoutUpdated;
			CustomPopup.SetWindowLocation( Placement, Window.Owner, Window, PlacementTarget, VerticalOffset, HorizontalOffset );
		}

		private void SetWindowPosition()
		{
			if ( PlacementTarget != null )
			{
				if ( PlacementTarget.IsVisible )
				{
					CustomPopup.SetWindowLocation( Placement, Window.Owner, Window, PlacementTarget, VerticalOffset, HorizontalOffset );
				}
				else
				{
					PlacementTarget.LayoutUpdated += PlacementTargetLayoutUpdated;
				}
			}
		}

		#endregion
	}
}