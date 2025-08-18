using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Extensions.Configuration;
using OvensManagerApp.Models;
using OvensManagerApp.MyControls;
using OvensManagerApp.Services;
using WpfScreenHelper;

namespace OvensManagerApp.Views;

/// <summary>
/// Interaction logic for OvensDashboardWindow.xaml
/// </summary>
public partial class OvensDashboardWindow : Window, INotifyPropertyChanged
{
    private ObservableCollection<Oven> _ovens;

    public ObservableCollection<Oven> Ovens
    {
        get { return _ovens; }
        set { _ovens = value;
            OnPropertyChanged();
        }
    }

    public  Dictionary<int, OvenCard> ovenCards;
    private bool _started;
    public double _dashboardFontSize = 50;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public double DashboardFontSize
    {
        get { return _dashboardFontSize; }
        set { _dashboardFontSize = value;
            OnPropertyChanged();
        }
        
    }


    public OvensDashboardWindow()
    {
        MoveToSecondScreen();
        InitializeComponent();
        DataContext = this;
        _ovens = new ObservableCollection<Oven>(LoadOvens());
        // Assuming OvenCard has a public property `OvenNumber`
        ovenCards = new Dictionary<int, OvenCard>();

        /*foreach (var child in OvensGrid.Children)
        {
            if (child is OvenCard ovenCard)
            {
                ovenCards[ovenCard.OvenNumber] = ovenCard;
            }
        }*/
    }

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

    public async void Start()
    {
        _started = true;
        while (_started)
        {
            foreach (var oven in _ovens)
            {
                var temperature = OvenDataService.Instance.GetOvenTemperature(oven.Address);
                ovenCards[oven.Number].OvenTemperature = temperature;
                ovenCards[oven.Number].RunTime =TimeSpan.FromHours(2);
            }  
               //delay 5 seconds
               await Task.Delay(1000);
            
        }
    }

    public List<Oven> LoadOvens()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetSection("Ovens").Get<List<Oven>>() ?? new List<Oven>();
    }
}
