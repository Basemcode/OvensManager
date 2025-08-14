using System.Windows;
using System.Windows.Controls;

namespace OvensManagerApp.Controls
{
    public partial class OvenControl : UserControl
    {
        public OvenControl() { InitializeComponent(); }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
                typeof(OvenControl), new PropertyMetadata(new CornerRadius(12)));
    }
}