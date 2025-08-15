using OvensManagerApp.Views;
using System.Windows;

namespace OvensManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OvensDashboardWindow _dashboard;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (_dashboard is null)
            {
                _dashboard = new OvensDashboardWindow();
                _dashboard.Closed += _dashboard_Closed;
                _dashboard.Show();
            }
        }

        private void _dashboard_Closed(object? sender, EventArgs e)
        {
            _dashboard = null;
        }

        private void StartOnDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (_dashboard != null)
            {
                _dashboard.Start();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_dashboard != null)
            {
                _dashboard.MaximizeWindow();
            }
        }
    }
}
