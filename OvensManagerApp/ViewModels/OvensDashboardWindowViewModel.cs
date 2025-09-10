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

public class OvensDashboardWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private bool _isStarted; // indicates if the dashboard has started updating data
    private bool _isGettingData = false; // flag to indicate if the process of getting data from ovens is running
    public double _dashboardFontSize = 50; // default font size
    private int _infoRequestDelayTime = 3000; // delay between each request for data from ovens in milliseconds
    private DispatcherTimer? _runTimeTimer; // Timer for updating oven runtime counter
    private System.Timers.Timer? _dataUpdatingTimer; // Timer for updating ovens data
    IConfigurationRoot _configuration; // configuration object to load settings from appsettings.json
    private ObservableCollection<Oven> _ovens; // collection of ovens to be displayed on the dashboard interface with the ability to update the interface using binding
    private static Dictionary<string, string>? _soundFiles; // dictionary to hold sound file paths from appsettings.json
    
    #region properties
    /// <summary>
    /// ovens list that will hold all ovens data and will be used to update interface using binding
    /// </summary>
    public ObservableCollection<Oven> Ovens
    {
        get { return _ovens; }
        set
        {
            _ovens = value;
            OnPropertyChanged();
        }
    }

    private WindowState _dashboardWindowState; // state of the dashboard window (maximized, minimized, normal) for binding
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
    #endregion

    public OvensDashboardWindowViewModel()
    {
        _configuration = LoadConfigs();
        _soundFiles = LoadSoundsPaths();
        _ovens = new ObservableCollection<Oven>(LoadOvens());
        DashboardWindowState = WindowState.Maximized;

        StartRunTimeTimer();
    }

    private Dictionary<string, string>? LoadSoundsPaths()
    {
         return _configuration.GetSection("SoundFiles").Get<Dictionary<string, string>>();
    }

    internal void Start()
    {
        _isStarted = true;
        StartDataUpdatingTimer();
        StartUpdatingInterface();
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


    public void StartDataUpdatingTimer()
    {
        // This timer will run on its own thread to update values of ovens with
        // the new values that will get from the ovens controllers
        // Setup timer
        _dataUpdatingTimer = new();
        _dataUpdatingTimer.Interval = _infoRequestDelayTime; // update every second
        _dataUpdatingTimer.AutoReset = true;
        _dataUpdatingTimer.Elapsed += (s, e) =>
        {
            if (_isStarted == false)
            {
                _dataUpdatingTimer.Stop();
            }
            if (_isGettingData == false) // if the process of getting data is not running, start it
            {
                UpdateOvensData();
            }
        };

        _dataUpdatingTimer.Start();
    }

    // Update ovens data from the controllers , should be run in a separate thread by the timer (on timer's thread)
    public void UpdateOvensData()
    {
        _isGettingData = true;
        foreach (var oven in Ovens)
        {
            try
            {
                // Read temperature from the oven
                var temperature = OvenDataService.Instance.GetOvenTemperatureAsync(oven.Address);
                oven.Temperature = temperature;

                // Read operating mode from the oven
                var operationMode = OvenDataService.Instance.GetOvenOperatingModeAsync(
                    oven.Address
                );
                oven.OvenStatus = Enum.GetName(typeof(OperatingModes), operationMode);

                // Read step of program from the oven
                var stepOfProgram = OvenDataService.Instance.GetOvenStepOfProgramAsync(
                    oven.Address
                );
                oven.StepOfProgram = stepOfProgram;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading data from oven {oven.Address}. " + e.Message);
                oven.OvenStatus = "No Connection!";
                oven.BackgroundColor = ResourcesHelper.blackBrush; // Indicate error with Black color
                oven.FontColor = Brushes.White; // Set font color to white for better visibility
                // throw new Exception(e.Message);
            }

            //await Task.Delay(10);
        }
        _isGettingData = false;
    }

    public void StartUpdatingInterface()
    {
        if (_isStarted)
        {
            foreach (var oven in Ovens)
            {
                try
                {
                    // Update oven status based on temperature and operating mode
                    if (
                        oven.OvenStatus == "Working"
                        && oven.Temperature <= oven.TargetTemp
                        && oven.StepOfProgram == 1
                    )
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
                        if (oven.Temperature <= 70) // ready to unload temperature
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
                        else if (oven.Temperature < oven.OpeningTemp)
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
                                SoundService.Instance.PlaySound(_soundFiles?["SoundsFilesMainPath"]+oven.Number+".wav");

                                oven.ResetTimer();
                            }
                        }
                    }

                    if (
                        (oven.OvenStatus == "Working" && oven.StepOfProgram == 2)
                        || (
                            (
                                oven.OvenStatus == "Stopped"
                                || oven.OvenStatus == "ProgramIsCompleted"
                            )
                            && (oven.StepOfProgram == 1 || oven.StepOfProgram == 5)
                            && oven.Temperature > oven.OpeningTemp
                            && oven.Temperature < oven.TargetTemp
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

    #region Load Configs
    public List<Oven> LoadOvens()
    {
        return _configuration.GetSection("Ovens").Get<List<Oven>>() ?? new List<Oven>();
    }

    private static IConfigurationRoot LoadConfigs()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
    #endregion
}
