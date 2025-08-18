using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Extensions.Configuration;
using OvensManagerApp.Models;
using OvensManagerApp.MyControls;
using OvensManagerApp.Services;
using OvensManagerApp.ViewModels;
using WpfScreenHelper;

namespace OvensManagerApp.Views;

public partial class OvensDashboardWindow : Window
{
    public OvensDashboardWindow()
    {
        InitializeComponent();
        this.DataContext = new OvensDashboardWindowViewModel();
    }

    // TODO: Implement the logic of MVVM
    private void MoveToSecondScreen()
    {
        // get all the available screens
        var screens = WpfScreenHelper.Screen.AllScreens.ToList();
        Screen screen;

        // check if we have more than one screen
        if (screens.Count < 2)
        {
            screen = screens[0];
        }
        else
        {
            screen = screens[1];
        }

        // Set the position to the selected screen with maximaized size
        Left = screen.WpfBounds.Left;
        Top = screen.WpfBounds.Top;
        Height = screen.WpfBounds.Height;
        Width = screen.WpfBounds.Width;
    }

    public void MaximizeWindow()
    {
        WindowState = WindowState.Normal;
        Thread.Sleep(100);
        WindowState = WindowState.Maximized;
    }
}
