using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using OvensManagerApp.Helpers;
using OvensManagerApp.Resources;

namespace OvensManagerApp.MyControls;

public partial class OvenCard : UserControl
{
    private DispatcherTimer _flashTimer;
    public OvenCard()
    {
        InitializeComponent();

        // Initialize the timer to stop the animation after 10 seconds
        _flashTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(10)
        };
        _flashTimer.Tick += FlashTimer_Tick;
    }

    // This method is called when the timer ticks (after 10 seconds)
    private void FlashTimer_Tick(object? sender, EventArgs e)
    {
        // Stop the flashing animation after 10 seconds
        var storyboard = this.Resources["FlashBackgroundStoryboard"] as Storyboard;
        storyboard?.Pause();
        flashOverlay.Opacity = 0;  // Make the overlay invisible

        // Stop the timer since we don't need it anymore
        _flashTimer.Stop();
    }

    public int OvenNumber
    {
        get => (int)GetValue(OvenNumberProperty);
        set => SetValue(OvenNumberProperty, value);
    }
    public static readonly DependencyProperty OvenNumberProperty = DependencyProperty.Register(
        nameof(OvenNumber),
        typeof(int),
        typeof(OvenCard),
        new PropertyMetadata(0)
    );

    public int StepOfProgram
    {
        get => (int)GetValue(StepOfProgramProperty);
        set => SetValue(StepOfProgramProperty, value);
    }
    public static readonly DependencyProperty StepOfProgramProperty = DependencyProperty.Register(
        nameof(StepOfProgram),
        typeof(int),
        typeof(OvenCard),
        new PropertyMetadata(-1)
    );

    public float OvenTemperature
    {
        get => (float)GetValue(OvenTemperatureProperty);
        set => SetValue(OvenTemperatureProperty, value);
    }
    public static readonly DependencyProperty OvenTemperatureProperty = DependencyProperty.Register(
        nameof(OvenTemperature),
        typeof(float),
        typeof(OvenCard),
        new PropertyMetadata(0f)
    );

    public string OvenStatus
    {
        get => (string)GetValue(OvenStatusProperty);
        set => SetValue(OvenStatusProperty, value);
    }
    public static readonly DependencyProperty OvenStatusProperty = DependencyProperty.Register(
        nameof(OvenStatus),
        typeof(string),
        typeof(OvenCard),
        new PropertyMetadata(string.Empty)
    );

    public string OvenStatusInRus
    {
        get { return LanguageHelper.OperatingModesInRus[(string)GetValue(OvenStatusProperty)]; }

        // set => SetValue(OvenStatusInRusProperty, value);
    }
    public static readonly DependencyProperty OvenStatusInRusProperty = DependencyProperty.Register(
        nameof(OvenStatusInRus),
        typeof(string),
        typeof(OvenCard),
        new PropertyMetadata(string.Empty)
    );

    public TimeSpan RunTime
    {
        get => (TimeSpan)GetValue(RunTimeProperty);
        set => SetValue(RunTimeProperty, value);
    }
    public static readonly DependencyProperty RunTimeProperty = DependencyProperty.Register(
        nameof(RunTime),
        typeof(TimeSpan),
        typeof(OvenCard),
        new PropertyMetadata(TimeSpan.Zero)
    );

    // the background color of the card with the trigger of animation
    public Brush BackgroundColor
    {
        get => (Brush)GetValue(BackgroundColorProperty);
        set { SetValue(BackgroundColorProperty, value); }
    }
    public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
        nameof(BackgroundColor),
        typeof(Brush),
        typeof(OvenCard),
        new PropertyMetadata(Brushes.Transparent, OnBackgroundColorChanged)
    );

    private static void OnBackgroundColorChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        var control = d as OvenCard;
        if (control != null && e.NewValue is Brush newBrush)
        {
            // Trigger the flashing animation when background turns green
            if (newBrush is LinearGradientBrush linearGradientBrush && linearGradientBrush == ResourcesHelper.greenBrush)
            {
                var storyboard = control.Resources["FlashBackgroundStoryboard"] as Storyboard;
                control.flashOverlay.Opacity = 0;  // Ensure overlay is invisible when not flashing
                control.flashOverlay.Fill = Brushes.Red;  // Set the overlay color
                control.flashingNumber.Opacity = 0.9;  // Set the overlayNumber opacity to be visible
                control.flashingNumber.Foreground = Brushes.White;  // Set the overlayNumber color
                storyboard?.Begin();

                // Start the timer to stop the animation after 10 seconds
                control._flashTimer.Start();
            }
            else
            {
                // Stop the flashing animation if it's not green
                var storyboard = control.Resources["FlashBackgroundStoryboard"] as Storyboard;
                storyboard?.Pause();
                control.flashOverlay.Opacity = 0;  // Ensure overlay is invisible when not flashing
                control.flashOverlay.Fill = Brushes.Transparent;  // Reset overlay color
                control.flashingNumber.Opacity = 0;  // Set the overlayNumber opacity to be invisible
                control.flashingNumber.Foreground = Brushes.Transparent;  // Reset overlayNumber color
                control._flashTimer.Stop();  // Stop the timer if background is not green
            }
        }
    }


    public double OvenFontSize
    {
        get => (double)GetValue(OvenFontSizeProperty);
        set => SetValue(OvenFontSizeProperty, value);
    }
    public static readonly DependencyProperty OvenFontSizeProperty = DependencyProperty.Register(
        nameof(OvenFontSize),
        typeof(double),
        typeof(OvenCard),
        new PropertyMetadata(50.0)
    );

    public Brush FontColor
    {
        get => (Brush)GetValue(FontColorProperty);
        set => SetValue(FontColorProperty, value);
    }


    public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register(
        nameof(FontColor),
        typeof(Brush),
        typeof(OvenCard),
        new PropertyMetadata(Brushes.Black)
    );
}
