using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using OvensManagerApp.Enums;
using OwenioNet.Types;

namespace OvensManagerApp.Helpers;

public class TestingHelper
{
    public static bool IsDevelop { get; set; } = true; //  true => enable testing mode
    public static int TestingTimerInterval { get; set; } = 100; // in milliseconds
}

// Simulate data from ovens for testing purposes
public static class VirtualDataGenerator
{
    // csharpier-ignore
    private static Dictionary<int, VirtualOvenStatus> _ovensData = new()
    {
        // Oven address and temperature and changing diriction
        { 8*1, new VirtualOvenStatus(){Temperature= 10.0f , IsRising = true  } },
        { 8*2, new VirtualOvenStatus(){Temperature= 50.2f , IsRising = false  } },
        { 8*3, new VirtualOvenStatus(){Temperature= 120.5f, IsRising = true  } },
        { 8*4, new VirtualOvenStatus(){Temperature= 170.8f, IsRising = false  } },
        { 8*5, new VirtualOvenStatus(){Temperature= 210.3f, IsRising = true  } },
        { 8*6, new VirtualOvenStatus(){Temperature= 240.1f, IsRising = false  } },
        { 8*7, new VirtualOvenStatus(){Temperature= 270.7f, IsRising = true   } },
        { 8*8, new VirtualOvenStatus(){Temperature= 310.6f, IsRising = false } },
        { 8*9, new VirtualOvenStatus(){Temperature= 350.9f, IsRising = true  } },
        { 8*10,new VirtualOvenStatus(){Temperature= 390.0f, IsRising = false } },
        { 8*11,new VirtualOvenStatus(){Temperature= 430.8f, IsRising = true  } },
        { 8*12,new VirtualOvenStatus(){Temperature= 470.2f, IsRising = false  } },
        { 8*13,new VirtualOvenStatus(){Temperature= 510.6f, IsRising = true   } },
        { 8*14,new VirtualOvenStatus(){Temperature= 550.4f, IsRising = false  } },
        { 8*15,new VirtualOvenStatus(){Temperature= 600.1f, IsRising = true   } },
        { 8*16,new VirtualOvenStatus(){Temperature= 650.7f, IsRising = false } },
    };

    private static bool _isStarted = false;
    static System.Timers.Timer? _timer;

    // Internal structure to hold oven status
    struct VirtualOvenStatus
    {
        public float Temperature;
        public OperatingModes Status = OperatingModes.Stopped;
        public int Step = 1;
        public CycleSteps CycleStep = CycleSteps.Idle;
        internal bool IsRising;

        public VirtualOvenStatus() { }
    }

    // Start the timer to update oven statuses
    public static void Start(int interval)
    {
        _timer = new System.Timers.Timer();
        _timer.Elapsed += UpdateVirtualOvenInternalStatus;
        _timer.Interval = interval;
        _timer.AutoReset = true;
        _timer.Start();
        _isStarted = true;

        foreach (var oven in _ovensData.Keys)
        {
            var ovenData = _ovensData[oven];
            ovenData.IsRising = true;
        }
        UpdateVirtualOvenInternalStatus(null, null);
    }

    // holds the logic of updating oven statuses
    private static void UpdateVirtualOvenInternalStatus(object? sender, EventArgs e)
    {
        if (_isStarted)
        {
            foreach (var oven in _ovensData.Keys)
            {
                var ovenData = _ovensData[oven];
                if (ovenData.IsRising)
                {
                    if (ovenData.CycleStep != CycleSteps.Heating)
                    {
                        ovenData.CycleStep = CycleSteps.Heating;
                    }
                    ovenData.Temperature++;
                    ovenData.Status = OperatingModes.Working;

                    if (ovenData.Temperature > 750)
                    {
                        ovenData.IsRising = false;
                        ovenData.Status = OperatingModes.ProgramIsCompleted;
                    }
                }
                else
                {
                    ovenData.Temperature--;

                    if (ovenData.Temperature > 430)
                    {
                        if (ovenData.CycleStep != CycleSteps.CoolingDown)
                        {
                            ovenData.CycleStep = CycleSteps.CoolingDown;
                        }
                    }
                    if (ovenData.Temperature < 430)
                    {
                        if (ovenData.CycleStep != CycleSteps.CanOpen)
                        {
                            ovenData.CycleStep = CycleSteps.CanOpen;
                        }
                    }
                    if (ovenData.Temperature < 70)
                    {
                        if (ovenData.CycleStep != CycleSteps.ReadytoUnload)
                        {
                            ovenData.CycleStep = CycleSteps.ReadytoUnload;
                        }
                    }
                    if (ovenData.Temperature < 1)
                    {
                        ovenData.IsRising = true;
                        ovenData.Status = OperatingModes.Stopped;
                        ovenData.Step = 1;
                    }
                }
                _ovensData[oven] = ovenData;
            }
        }
    }

    // 
    public static float GetTestingTemperature(int oven)
    {
        if (_isStarted)
        {
            Thread.Sleep(50); // Simulate some delay
            return _ovensData[oven].Temperature;
        }
        else
        {
            Start(TestingHelper.TestingTimerInterval);
            return _ovensData[oven].Temperature;
        }
    }


    public static int GetTestingOperatingModeAsync(int oven)
    {
        if (_isStarted)
        {
            var dataFromDevice = (int)_ovensData[oven].Status;

            return dataFromDevice;
        }
        else
        {
            // Start asynchronously and return after the delay
            Start(TestingHelper.TestingTimerInterval);
            
            return (int)_ovensData[oven].Status;
        }
    }


    internal static int GetTestingStepOfProgram(int oven)
    {
        if (_isStarted)
        {
            return _ovensData[oven].Step;
        }
        else
        {
            Start(TestingHelper.TestingTimerInterval);
            return _ovensData[oven].Step;
        }
    }
}
