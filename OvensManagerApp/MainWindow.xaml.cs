using CommunityToolkit.Mvvm.Input;
using OvensManagerApp.Interfaces;
using OvensManagerApp.MyControls;
using OvensManagerApp.Services;
using OvensManagerApp.ViewModels;
using OvensManagerApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OvensManagerApp;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private OvensDashboardWindowViewModel _dashboardVm;
    private static double _dashboardFontSize = 50;
    private WindowService _windowService = new WindowService(); 
    public double DashboardFontSize
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
        if (_dashboardVm is null)
        {
            _dashboardVm = new OvensDashboardWindowViewModel();
            _windowService.ShowWindow(_dashboardVm);
        }
    }


    private void StartOnDashboard_Click(object sender, RoutedEventArgs e)
    {
        if (_dashboardVm != null)
        {
            _dashboardVm.Start();
        }
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (_dashboardVm != null)
        {
            _dashboardVm.DashboardWindowState = WindowState.Maximized;
            //_dashboard.MaximizeWindow();
        }
    }

    private void btnIncreaseFontSize_Click(object sender, RoutedEventArgs e)
    {
        // set the font size
        if (DashboardFontSize < 70 && _dashboardVm is not null)
        {
            _dashboardVm.DashboardFontSize += 10;
            DashboardFontSize = _dashboardVm.DashboardFontSize;
        }
    }

    private void btnDecreaseFontSize_Click(object sender, RoutedEventArgs e)
    {
        // set the font size
        if (DashboardFontSize>10 && _dashboardVm is not null)
        {
            _dashboardVm.DashboardFontSize -= 10;
            DashboardFontSize = _dashboardVm.DashboardFontSize;
        }
       
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
