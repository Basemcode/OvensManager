using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using OvensManagerApp.Services;
using OvensManagerApp.ViewModels;
using System.Windows;

namespace OvensManagerApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly WindowService _windowService = new();
    private OvensDashboardWindowViewModel? _dashboardVm;

    [ObservableProperty]
    private double dashboardFontSize = 50;

    [RelayCommand]
    private void OpenDashboard()
    {
        if (_dashboardVm is null)
        {
            // resolve from DI so OvenLogService is injected
            _dashboardVm = ((App)Application.Current).Services.GetRequiredService<OvensDashboardWindowViewModel>();
            _windowService.ShowWindow(_dashboardVm);
        }
    }

    [RelayCommand]
    private void StartOnDashboard()
    {
        _dashboardVm?.Start();
    }

    [RelayCommand]
    private void MaximizeDashboard()
    {
        if (_dashboardVm != null)
        {
            _dashboardVm.DashboardWindowState = WindowState.Maximized;
        }
    }

    [RelayCommand]
    private void IncreaseFontSize()
    {
        if (DashboardFontSize < 70 && _dashboardVm is not null)
        {
            _dashboardVm.DashboardFontSize += 10;
            DashboardFontSize = _dashboardVm.DashboardFontSize;
        }
    }

    [RelayCommand]
    private void DecreaseFontSize()
    {
        if (DashboardFontSize > 10 && _dashboardVm is not null)
        {
            _dashboardVm.DashboardFontSize -= 10;
            DashboardFontSize = _dashboardVm.DashboardFontSize;
        }
    }

    [RelayCommand]
    private void ExitApp()
    {
        Application.Current.Shutdown();
    }
}
