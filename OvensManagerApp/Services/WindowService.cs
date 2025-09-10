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
        // map ViewModel type to View type and create/show the window
        if (viewModel is OvensDashboardWindowViewModel ovensDashboardWindowVm) { 
            var ovensDashboardWindow = new OvensDashboardWindow { DataContext = ovensDashboardWindowVm };
            ovensDashboardWindow.Show();
        }
        
    }

    public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
    {
        // showing modal dialogs
        if (viewModel is OvensDashboardWindowViewModel ovensDashboardWindowVm)
        {
            var ovensDashboardWindow = new OvensDashboardWindow { DataContext = ovensDashboardWindowVm };
            ovensDashboardWindow.Show();
        }
        return null;
    }
}
