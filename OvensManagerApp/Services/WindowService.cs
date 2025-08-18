using OvensManagerApp.Interfaces;
using OvensManagerApp.ViewModels;
using OvensManagerApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvensManagerApp.Services;

internal class WindowService: IWindowService
{
    public void ShowWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
    {
        // Logic to map ViewModel type to View type and create/show the window
        // Example (simplified):
        if (viewModel is OvensDashboardWindowViewModel ovensDashboardWindowVm) { 
            var ovensDashboardWindow = new OvensDashboardWindow { DataContext = ovensDashboardWindowVm };
            ovensDashboardWindow.Show();
        }
        
    }

    public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
    {
        // Similar logic for showing modal dialogs
        // Example (simplified):
        if (viewModel is OvensDashboardWindowViewModel ovensDashboardWindowVm)
        {
            var ovensDashboardWindow = new OvensDashboardWindow { DataContext = ovensDashboardWindowVm };
            ovensDashboardWindow.Show();
        }
        return null;
    }
}
