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
    public static bool IsDevelop { get; set; } = false;
    public static int TestingTimerInterval { get; set; } = 100; // in milliseconds
}

public static class VirtualDataGenerator
{
    // csharpier-ignore
    private static Dictionary<int, VirtualOvenStatus> _ovensData = new()
    {
        // Oven address and temperature
        { 8*1, new VirtualOvenStatus(){Temperature= 10.0f , _isRising = true  } },
        { 8*2, new VirtualOvenStatus(){Temperature= 50.2f , _isRising = false  } },
        { 8*3, new VirtualOvenStatus(){Temperature= 120.5f, _isRising = true  } },
        { 8*4, new VirtualOvenStatus(){Temperature= 170.8f, _isRising = false  } },
        { 8*5, new VirtualOvenStatus(){Temperature= 210.3f, _isRising = true  } },
        { 8*6, new VirtualOvenStatus(){Temperature= 240.1f, _isRising = false  } },
        { 8*7, new VirtualOvenStatus(){Temperature= 270.7f, _isRising = true   } },
        { 8*8, new VirtualOvenStatus(){Temperature= 310.6f, _isRising = false } },
        { 8*9, new VirtualOvenStatus(){Temperature= 350.9f, _isRising = true  } },
        { 8*10,new VirtualOvenStatus(){Temperature= 390.0f, _isRising = false } },
        { 8*11,new VirtualOvenStatus(){Temperature= 430.8f, _isRising = true  } },
        { 8*12,new VirtualOvenStatus(){Temperature= 470.2f, _isRising = false  } },
        { 8*13,new VirtualOvenStatus(){Temperature= 510.6f, _isRising = true   } },
        { 8*14,new VirtualOvenStatus(){Temperature= 550.4f, _isRising = false  } },
        { 8*15,new VirtualOvenStatus(){Temperature= 600.1f, _isRising = true   } },
        { 8*16,new VirtualOvenStatus(){Temperature= 650.7f, _isRising = false } },
    };

    struct VirtualOvenStatus
    {
        public float Temperature;
        public OperatingModes _status = OperatingModes.Stopped;
        public int _step = 1;
        public CycleSteps _cycleStep = CycleSteps.Idle;
        internal bool _isRising;

        public VirtualOvenStatus() { }
    }

    private static int _step = 1;
    private static bool _isStarted = false;
    private static OperatingModes _status = OperatingModes.Stopped;
    private static bool _isRising = true;
    public static CycleSteps _cycleStep = CycleSteps.Idle;
    static System.Timers.Timer _timer;

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
            ovenData._isRising = true;
        }
        UpdateVirtualOvenInternalStatus(null, null);
    }

    private static void UpdateVirtualOvenInternalStatus(object? sender, EventArgs e)
    {
        if (_isStarted)
        {
            foreach (var oven in _ovensData.Keys)
            {
                var ovenData = _ovensData[oven];
                if (ovenData._isRising)
                {
                    if (ovenData._cycleStep != CycleSteps.Heating)
                    {
                        ovenData._cycleStep = CycleSteps.Heating;
                    }
                    ovenData.Temperature++;
                    ovenData._status = OperatingModes.Working;

                    if (ovenData.Temperature > 750)
                    {
                        ovenData._isRising = false;
                        ovenData._status = OperatingModes.ProgramIsCompleted;
                    }
                }
                else
                {
                    ovenData.Temperature--;

                    if (ovenData.Temperature > 430)
                    {
                        if (ovenData._cycleStep != CycleSteps.CoolingDown)
                        {
                            ovenData._cycleStep = CycleSteps.CoolingDown;
                        }
                    }
                    if (ovenData.Temperature < 430)
                    {
                        if (ovenData._cycleStep != CycleSteps.CanOpen)
                        {
                            ovenData._cycleStep = CycleSteps.CanOpen;
                        }
                    }
                    if (ovenData.Temperature < 70)
                    {
                        if (ovenData._cycleStep != CycleSteps.ReadytoUnload)
                        {
                            ovenData._cycleStep = CycleSteps.ReadytoUnload;
                        }
                    }
                    if (ovenData.Temperature < 1)
                    {
                        ovenData._isRising = true;
                        ovenData._status = OperatingModes.Stopped;
                        ovenData._step = 1;
                    }
                }
                _ovensData[oven] = ovenData;
            }
        }
    }

    public static float GetTestingTemperature(int oven)
    {
        if (_isStarted)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " Data: " + _ovensData[oven].Temperature);
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
            var dataFromDevice = (int)_ovensData[oven]._status;
            
            /*var dataFromDevice = await Task.Run(() =>
            {
                var reveivedData = (int)_ovensData[oven]._status;
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " Data: " + reveivedData);
                Thread.Sleep(50); // Simulate some delay
                return reveivedData;
            });*/

            return dataFromDevice;
        }
        else
        {
            // Start asynchronously and return after the delay
            Start(TestingHelper.TestingTimerInterval);
            
            return (int)_ovensData[oven]._status;
        }
    }


    internal static int GetTestingStepOfProgram(int oven)
    {
        if (_isStarted)
        {
            return _ovensData[oven]._step;
        }
        else
        {
            Start(TestingHelper.TestingTimerInterval);
            return _ovensData[oven]._step;
        }
    }
}
