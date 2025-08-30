using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using OvensManagerApp.Enums;
using OvensManagerApp.Interfaces;
using OvensManagerApp.Models;
using OvensManagerApp.Resources;
using OvensManagerApp.Services;

namespace OvensManagerApp.ViewModels;

public class OvensDashboardWindowViewModel : ViewModelBase, INotifyPropertyChanged, IClosable
{
    private bool _started;
    public double _dashboardFontSize = 50;
    private int infoRequestDelayTime = 5000;
    private DispatcherTimer _runTimeTimer; // Timer for updating oven runtime
    private System.Timers.Timer _dataUpdatingTimer; // Timer for updating oven runtime
    
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
        set
        {
            _dashboardWindowState = value;
            OnPropertyChanged();
        }
    }

    

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

        StartRunTimeTimer();
    }

    private void StartRunTimeTimer()
    {
        // Setup timer
        _runTimeTimer = new DispatcherTimer();
        _runTimeTimer.Interval = TimeSpan.FromSeconds(1); // update every second
        _runTimeTimer.Tick += (s, e) =>
        {
            StartUpdatingInterface();
            foreach (var oven in Ovens)
            {
                oven.UpdateRuntime();
            }
        };

        foreach (var oven in Ovens)
        {
            oven.ResetTimer();
        }

        _runTimeTimer.Start();
    }

    internal void Start()
    {
        _started = true;
        StartDataUpdatingTimer();
        StartUpdatingInterface();
    }

    public void StartDataUpdatingTimer()
    {
        // This timer will run on its own thread to update values of ovens with
        // the new values that will get from the ovens controllers 
        // Setup timer
        _dataUpdatingTimer = new();
        _dataUpdatingTimer.Interval = infoRequestDelayTime; // update every second
        _dataUpdatingTimer.AutoReset = true;
        _dataUpdatingTimer.Elapsed += (s, e) =>
        {
            if (_started == false)
            {
                _dataUpdatingTimer.Stop();
            }
            UpdateOvensData();
        };
        

        _dataUpdatingTimer.Start();
    }

    // Update ovens data from the controllers , should be run in a separate thread by the timer (on timer's thread)
    public async void UpdateOvensData()
    {
        foreach (var oven in Ovens)
        {
            try
            {
                // Read temperature from the oven
                var temperature = OvenDataService.Instance.GetOvenTemperatureAsync(
                    oven.Address
                );
                oven.Temperature = temperature;
                Console.WriteLine(
                    DateTime.Now.ToShortTimeString() + " " + oven.Number + " : " + oven.Temperature
                );

                // Read operating mode from the oven
                var operationMode = OvenDataService.Instance.GetOvenOperatingModeAsync(
                    oven.Address
                );
                oven.OvenStatus = Enum.GetName(typeof(OperatingModes), operationMode);
                /*Console.WriteLine(
                    DateTime.Now.ToShortTimeString() + " " + oven.Number + " : " + oven.OvenStatus
                );*/

                // Read step of program from the oven
                var stepOfProgram = OvenDataService.Instance.GetOvenStepOfProgramAsync(
                    oven.Address
                );
                oven.StepOfProgram = stepOfProgram;
                /*Console.WriteLine(
                    DateTime.Now.ToShortTimeString()
                        + " "
                        + oven.Number
                        + " : "
                        + oven.StepOfProgram
                );*/

                // Update oven status based on temperature and operating mode
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading data from oven {oven.Address}. " + e.Message);
                oven.OvenStatus = "No Connection!";
                oven.BackgroundColor = ResourcesHelper.blackBrush; // Indicate error with Black color
                oven.FontColor = Brushes.White; // Set font color to white for better visibility
                // throw new Exception(e.Message);
            }

            //await Task.Delay(50);
        }
    }

    public void StartUpdatingInterface()
    {
        if (_started)
        {
            foreach (var oven in Ovens)
            {
                try
                {
                    if (oven.OvenStatus == "Working" && oven.Temperature <= 900 && oven.StepOfProgram == 1)
                    {
                        if (
                            oven.BackgroundColor != ResourcesHelper.redBrush
                            || oven.CycleStep != CycleSteps.Heating
                        )
                        {
                            oven.CycleStep = CycleSteps.Heating;
                            oven.BackgroundColor = ResourcesHelper.redBrush;
                            oven.FontColor = Brushes.Black;
                            oven.ResetTimer();
                        }
                    }

                    if ((oven.OvenStatus == "Stopped" || oven.OvenStatus == "ProgramIsCompleted"))
                    {
                        if (oven.Temperature <= 70)
                        {
                            if (
                                oven.BackgroundColor != ResourcesHelper.blueBrush
                                || oven.CycleStep != CycleSteps.ReadytoUnload
                            )
                            {
                                oven.CycleStep = CycleSteps.ReadytoUnload;
                                oven.BackgroundColor = ResourcesHelper.blueBrush;
                                oven.FontColor = Brushes.Black;
                                oven.ResetTimer();
                            }
                        }
                        else if (oven.Temperature < 430)
                        {
                            if (
                                oven.BackgroundColor != ResourcesHelper.greenBrush
                                || oven.CycleStep != CycleSteps.CanOpen
                            )
                            {
                                oven.CycleStep = CycleSteps.CanOpen;
                                oven.BackgroundColor = ResourcesHelper.greenBrush;
                                oven.FontColor = Brushes.Black;
                                SoundService.Instance.PlaySound(SoundsList.OvenCanBeOpened);
                                oven.ResetTimer();
                            }
                        }
                    }

                    if (
                        (oven.OvenStatus == "Working" && oven.StepOfProgram == 2)
                        || (
                            (oven.OvenStatus == "Stopped" || oven.OvenStatus == "ProgramIsCompleted")
                            && (oven.StepOfProgram == 1 || oven.StepOfProgram == 5)
                            && oven.Temperature > 430
                            && oven.Temperature < 760
                        )
                    )
                    {
                        if (
                            oven.BackgroundColor != ResourcesHelper.yellowBrush
                            || oven.CycleStep != CycleSteps.CoolingDown
                        )
                        {
                            oven.CycleStep = CycleSteps.CoolingDown;
                            oven.BackgroundColor = ResourcesHelper.yellowBrush;
                            oven.FontColor = Brushes.Black;
                            oven.ResetTimer();
                        }
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        
    }

    #region LoadOvens
    public List<Oven> LoadOvens()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetSection("Ovens").Get<List<Oven>>() ?? new List<Oven>();
    }
    #endregion

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
