using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace APLPromoter.UI.WPF.Controls
{
    public class ImageToggleButton : ToggleButton
    {
        [Category("Common Properties")]
        public ImageSource ImageSourceActive
        {
            get { return (ImageSource)GetValue(ImageSourceActiveProperty); }
            set { SetValue(ImageSourceActiveProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActiveProperty =
            DependencyProperty.Register("ImageSourceActive", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceActiveHover
        {
            get { return (ImageSource)GetValue(ImageSourceActiveHoverProperty); }
            set { SetValue(ImageSourceActiveHoverProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActiveHoverProperty =
            DependencyProperty.Register("ImageSourceActiveHover", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceActiveDisabled
        {
            get { return (ImageSource)GetValue(ImageSourceActiveDisabledProperty); }
            set { SetValue(ImageSourceActiveDisabledProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActiveDisabledProperty =
            DependencyProperty.Register("ImageSourceActiveDisabled", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceActivePressed
        {
            get { return (ImageSource)GetValue(ImageSourceActivePressedProperty); }
            set { SetValue(ImageSourceActivePressedProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActivePressedProperty =
            DependencyProperty.Register("ImageSourceActivePressed", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceIdle
        {
            get { return (ImageSource)GetValue(ImageSourceIdleProperty); }
            set { SetValue(ImageSourceIdleProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleProperty =
            DependencyProperty.Register("ImageSourceIdle", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceIdleDisabled
        {
            get { return (ImageSource)GetValue(ImageSourceIdleDisabledProperty); }
            set { SetValue(ImageSourceIdleDisabledProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleDisabledProperty =
            DependencyProperty.Register("ImageSourceIdleDisabled", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceIdleHover
        {
            get { return (ImageSource)GetValue(ImageSourceIdleHoverProperty); }
            set { SetValue(ImageSourceIdleHoverProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleHoverProperty =
            DependencyProperty.Register("ImageSourceIdleHover", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceIdlePressed
        {
            get { return (ImageSource)GetValue(ImageSourceIdlePressedProperty); }
            set { SetValue(ImageSourceIdlePressedProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdlePressedProperty =
            DependencyProperty.Register("ImageSourceIdlePressed", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public Stretch ImageSourceStretch
        {
            get { return (Stretch)GetValue(ImageSourceStretchProperty); }
            set { SetValue(ImageSourceStretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSourceStretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceStretchProperty =
            DependencyProperty.Register("ImageSourceStretch", typeof(Stretch), typeof(ImageToggleButton), new FrameworkPropertyMetadata(Stretch.Uniform));


        [Category("Common Properties")]
        public StretchDirection ImageSourceStretchDirection
        {
            get { return (StretchDirection)GetValue(ImageSourceStretchDirectionProperty); }
            set { SetValue(ImageSourceStretchDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSourceStretchDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceStretchDirectionProperty =
            DependencyProperty.Register("ImageSourceStretchDirection", typeof(StretchDirection), typeof(ImageToggleButton), new FrameworkPropertyMetadata(StretchDirection.Both));
    }
}
