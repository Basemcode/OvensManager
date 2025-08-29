using System.Collections.ObjectModel;
using System.ComponentModel;
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
    private DispatcherTimer _runTimeTimer; // Timer for updating oven runtime
    private DispatcherTimer _dataUpdatingTimer; // Timer for updating oven runtime

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

    private bool _started;
    public double _dashboardFontSize = 50;
    private int infoRequestDelayTime = 5000;

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
        // Setup timer
        _dataUpdatingTimer = new DispatcherTimer();
        _dataUpdatingTimer.Interval = TimeSpan.FromMilliseconds(infoRequestDelayTime); // update every second
        _dataUpdatingTimer.Tick += (s, e) =>
        {
            if (_started == false)
            {
                _dataUpdatingTimer.Stop();
            }
            UpdateOvensData();
        };
        _started = true;

        _dataUpdatingTimer.Start();
    }

    public async void UpdateOvensData()
    {
        foreach (var oven in Ovens)
        {
            try
            {
                // Read temperature from the oven
                var temperature = await OvenDataService.Instance.GetOvenTemperatureAsync(
                    oven.Address
                );
                oven.Temperature = temperature;
                /*Console.WriteLine(
                    DateTime.Now.ToShortTimeString() + " " + oven.Number + " : " + oven.Temperature
                );*/

                // Read operating mode from the oven
                var operationMode = await OvenDataService.Instance.GetOvenOperatingModeAsync(
                    oven.Address
                );
                oven.OvenStatus = Enum.GetName(typeof(OperatingModes), operationMode);
                /*Console.WriteLine(
                    DateTime.Now.ToShortTimeString() + " " + oven.Number + " : " + oven.OvenStatus
                );*/

                // Read step of program from the oven
                var stepOfProgram = await OvenDataService.Instance.GetOvenStepOfProgramAsync(
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
                if (oven.OvenStatus == "Working" && temperature <= 900 && oven.StepOfProgram == 1)
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
                    if (temperature <= 70)
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
                    else if (temperature < 430)
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
                        && temperature > 430
                        && temperature < 760
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
                Console.WriteLine($"Error reading data from oven {oven.Address}. " + e.Message);
                oven.OvenStatus = "No Connection!";
                oven.BackgroundColor = ResourcesHelper.blackBrush; // Indicate error with Black color
                oven.FontColor = Brushes.White; // Set font color to white for better visibility
                // throw new Exception(e.Message);
            }

            //await Task.Delay(50);
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
