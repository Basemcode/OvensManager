using Microsoft.Extensions.Configuration;
using OvensManagerApp.Models;
using OvensManagerApp.MyControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using WpfScreenHelper;
using OvensManagerApp.Services;
using CommunityToolkit.Mvvm.Input;
using OvensManagerApp.Interfaces;
using System.Windows.Media;
using OvensManagerApp.Enums;

namespace OvensManagerApp.ViewModels;

public class OvensDashboardWindowViewModel: ViewModelBase, INotifyPropertyChanged,IClosable
{
    private ObservableCollection<Oven> _ovens;

    public ObservableCollection<Oven> Ovens
    {
        get { return _ovens; }
        set
        {
            _ovens = value;
            OnPropertyChanged();
        }
    }

    // For closing the window
    public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
    public RelayCommand<IClosable> WindowClosedCommand { get; set; }


    private WindowState _dashboardWindowState;

    public WindowState DashboardWindowState
    {
        get { return _dashboardWindowState; }
        set { _dashboardWindowState = value;
            OnPropertyChanged();
        }
    }

    private bool _started;
    public double _dashboardFontSize = 50;
    public double DashboardFontSize
    {
        get { return _dashboardFontSize; }
        set
        {
            _dashboardFontSize = value;
            OnPropertyChanged();
        }
    }


    public OvensDashboardWindowViewModel()
    {
        LoadOvens();
        this.CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
        _ovens = new ObservableCollection<Oven>(LoadOvens());
        DashboardWindowState = WindowState.Maximized;
    }


    public async void Start()
    {
        _started = true;
        while (_started)
        {
            foreach (var oven in Ovens)
            {
                try
                {
                    var temperature = OvenDataService.Instance.GetOvenTemperature(oven.Address);
                    oven.Temperature = temperature;
                    Console.WriteLine(oven.Number+" : " + oven.Temperature);

                    var operationMode = OvenDataService.Instance.GetOvenOperatingMode(oven.Address);
                    oven.OvenStatus = Enum.GetName(typeof(OperatingModes), operationMode);
                    Console.WriteLine(oven.Number + " : " + oven.OvenStatus);

                    var stepOfProgram = OvenDataService.Instance.GetOvenStepOfProgram(oven.Address);
                    oven.StepOfProgram = stepOfProgram;
                    Console.WriteLine(oven.Number + " : " + oven.StepOfProgram);

                    if ((oven.OvenStatus == "Working" || oven.OvenStatus == "ProgramIsCompleted") && temperature < 760)
                    {
                        Brush appBrush = (Brush)Application.Current.FindResource("RedGradient");
                        oven.BackgroundColor = appBrush;
                    }
                    if ((oven.OvenStatus == "Stopped" || oven.OvenStatus == "ProgramIsCompleted") && temperature < 430 )
                    {
                        Brush appBrush = (Brush)Application.Current.FindResource("GreenGradient");
                        oven.BackgroundColor = appBrush;
                    }

                    if ((oven.OvenStatus == "Working" && oven.StepOfProgram == 2)
                        ||((oven.OvenStatus == "Stopped" || oven.OvenStatus == "ProgramIsCompleted") &&( oven.StepOfProgram == 1 || oven.StepOfProgram == 5) && temperature > 430 && temperature < 760))
                    {
                        Brush appBrush = (Brush)Application.Current.FindResource("YellowGradient");
                        oven.BackgroundColor = appBrush;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading data from oven {oven.Address}. " + e.Message);
                    oven.OvenStatus = "No Connection!"; 
                    oven.BackgroundColor= Brushes.Black; // Indicate error with Black color
                    oven.FontColor = Brushes.White; // Set font color to white for better visibility

                }
                
                oven.RunTime = TimeSpan.FromHours(2);
                await Task.Delay(50);
            }
            //delay 5 seconds
            await Task.Delay(5000);

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



    private void CloseWindow(IClosable window)
    {
        if (window != null)
        {
            window.Close();
        }
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}
