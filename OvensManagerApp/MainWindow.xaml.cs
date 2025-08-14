using OvensManagerApp.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfScreenHelper;

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
