using OvensManagerApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvensManagerApp.Helpers;

public class TestingHelper
{
    public static bool IsDevelop { get; set; } = true;
}

public static class VirtualOvenController
{
    private static float _temperature = 1.0f;
    private static int _step = 1; 
    private static OperatingModes _status = OperatingModes.Stopped; 
    private static bool _isRising = true;

    public static void Start()
    {
        
    }

    public static float GetTemperature()
    {

    }
    public static int GetTestingOperatingMode()
    {
       
    }

    internal static int GetTestingStepOfProgram()
    {
       
    }
}
