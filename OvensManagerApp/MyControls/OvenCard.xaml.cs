using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OvensManagerApp.MyControls;

public partial class OvenCard : UserControl
{
    public OvenCard()
    {
        InitializeComponent();
    }

    public int OvenNumber
    {
        get => (int)GetValue(OvenNumberProperty);
        set => SetValue(OvenNumberProperty, value);
    }
    public static readonly DependencyProperty OvenNumberProperty =
        DependencyProperty.Register(nameof(OvenNumber), typeof(int), typeof(OvenCard), new PropertyMetadata(0));

    public float OvenTemperature
    {
        get => (float)GetValue(OvenTemperatureProperty);
        set => SetValue(OvenTemperatureProperty, value);
    }
    public static readonly DependencyProperty OvenTemperatureProperty =
        DependencyProperty.Register(nameof(OvenTemperature), typeof(float), typeof(OvenCard), new PropertyMetadata(0f));

    public string OvenStatus
    {
        get => (string)GetValue(OvenStatusProperty);
        set => SetValue(OvenStatusProperty, value);
    }
    public static readonly DependencyProperty OvenStatusProperty =
        DependencyProperty.Register(nameof(OvenStatus), typeof(string), typeof(OvenCard), new PropertyMetadata(string.Empty));

    public TimeSpan RunTime
    {
        get => (TimeSpan)GetValue(RunTimeProperty);
        set => SetValue(RunTimeProperty, value);
    }
    public static readonly DependencyProperty RunTimeProperty =
        DependencyProperty.Register(nameof(RunTime), typeof(TimeSpan), typeof(OvenCard), new PropertyMetadata(TimeSpan.MinValue));

    public Brush BackgroundColor
    {
        get => (Brush)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }
    public static readonly DependencyProperty BackgroundColorProperty =
        DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(OvenCard), new PropertyMetadata(Brushes.Transparent));

    public double OvenFontSize
    {
        get => (double)GetValue(OvenFontSizeProperty);
        set => SetValue(OvenFontSizeProperty, value);
    }
    public static readonly DependencyProperty OvenFontSizeProperty =
        DependencyProperty.Register(nameof(OvenFontSize), typeof(double), typeof(OvenCard), new PropertyMetadata(50.0));

    public Brush FontColor
    {
        get => (Brush)GetValue(FontColorProperty);
        set => SetValue(FontColorProperty, value);
    }
    public static readonly DependencyProperty FontColorProperty =
        DependencyProperty.Register(nameof(FontColor), typeof(Brush), typeof(OvenCard), new PropertyMetadata(Brushes.Black));
}

