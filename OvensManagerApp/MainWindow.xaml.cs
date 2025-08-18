using OvensManagerApp.MyControls;
using OvensManagerApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OvensManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private OvensDashboardWindow _dashboard;
        private static double _dashboardFontSize=50;

        public    double DashboardFontSize
        {
            get => _dashboardFontSize; set
            {
                _dashboardFontSize = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


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

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (_dashboard != null)
            {
                _dashboard.MaximizeWindow();
            }
        }

        private void btnIncreaseFontSize_Click(object sender, RoutedEventArgs e)
        {
            // set the font size
            if (DashboardFontSize < 70 && _dashboard is not null)
            {
                _dashboard.DashboardFontSize += 10;
                DashboardFontSize = _dashboard.DashboardFontSize;
            }
        }

        private void btnDecreaseFontSize_Click(object sender, RoutedEventArgs e)
        {
            // set the font size
            if (DashboardFontSize>10 && _dashboard is not null)
            {
                _dashboard.DashboardFontSize -= 10;
                DashboardFontSize = _dashboard.DashboardFontSize;
            }
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_dashboard != null)
            {
                _dashboard.Close();
                _dashboard = null;
            }
        }
    }
}
