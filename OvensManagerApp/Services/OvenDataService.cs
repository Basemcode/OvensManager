using System.IO.Ports;
using Microsoft.Extensions.Configuration;
using OvensManagerApp.Configuration;
using OvensManagerApp.Helpers;
using OwenioNet;
using OwenioNet.DataConverter.Converter;
using OwenioNet.IO;
using OwenioNet.Types;

namespace OvensManagerApp.Services;

public class OvenDataService
{
    private static OvenDataService _instance;
    private static SerialPort _serialPort;
    private static IOwenProtocolMaster _owenProtocolMaster;
    private static bool _initialized = false;
    private static readonly object lockObject = new object(); // for thread safe locking

    private OvenDataService() { }

    // Public static method to get the single instance
    public static OvenDataService Instance
    {
        get
        {
            Console.WriteLine("OvenDataService Instance requested on thread: " + Thread.CurrentThread.ManagedThreadId);
            lock (lockObject) // Acquire lock
            {
                Console.WriteLine("Lock acquired by thread: " + Thread.CurrentThread.ManagedThreadId);
                if (_instance == null)
                {
                    _instance = new OvenDataService();
                }

                return _instance;
            }
        }
    }

    // Initialize the serial port and Modbus master
    private static void Initialize(
        int baudRate = 9600,
        Parity parity = Parity.None,
        int dataBits = 8,
        StopBits stopBits = StopBits.One
    )
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();

        var config = configuration.GetSection("OvenServiceConfig").Get<OvenServiceConfig>();

        if (_serialPort == null)
        {
            // Create and configure the serial port
            _serialPort = new SerialPort(config?.PortName, baudRate, parity, dataBits, stopBits);
            _serialPort.ReadTimeout = 1000; // in millisecond
            _serialPort.Open();

            // Create the OwenProtocolMaster object using SerialPort
            _owenProtocolMaster = OwenProtocolMaster.Create(
                new SerialPortAdapter(_serialPort),
                null
            );
            _initialized = true;
        }
    }

    public float GetOvenTemperatureAsync(int ovenAddress)
    {
        if (Helpers.TestingHelper.IsDevelop)
        {
            return VirtualDataGenerator.GetTestingTemperature(ovenAddress);
        }

        if (!_initialized)
        {
            Initialize();
        }

        try
        {
            // Run the oven data fetch operation in a background thread
            var dataFromDevice = _owenProtocolMaster.OwenRead(
                    ovenAddress,
                    AddressLengthType.Bits8,
                    "read"
                );

            // for debugging
            // Console.WriteLine($"oven num: {ovenAddress / 8} on thread: {Thread.CurrentThread.ManagedThreadId} Data: {dataFromDevice}");

            // Convert the data from the device to a float value
            var converterFloat = new ConverterFloatTimestamp();
            var valueFromDevice = converterFloat.ConvertBack(dataFromDevice);
            return valueFromDevice.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to read oven Temperature: " + e.Message);
            throw new InvalidOperationException(
                "Failed to read oven temperature. Please check the connection and configuration.",
                e
            );
        }
    }

    public int GetOvenOperatingModeAsync(int ovenAddress)
    {
        if (Helpers.TestingHelper.IsDevelop)
        {
            var temp =VirtualDataGenerator.GetTestingOperatingModeAsync(ovenAddress);
            return temp;
        }

        if (!_initialized)
        {
            Initialize();
        }

        try
        {
            // Run the oven data fetch operation in a background thread
            var dataFromDevice = _owenProtocolMaster.OwenRead(ovenAddress, AddressLengthType.Bits8, "r.St");

            // Convert the data from the device to an int value
            var converterU = new ConverterU(2);
            var valueFromDevice = converterU.ConvertBack(dataFromDevice);
            return (int)valueFromDevice;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to read oven Operating Mode: " + e.Message);
            throw new InvalidOperationException(
                "Failed to read oven Operating Mode. Please check the connection and configuration.",
                e
            );
        }
    }

    public int GetOvenStepOfProgramAsync(int ovenAddress)
    {
        if (Helpers.TestingHelper.IsDevelop)
        {
            return VirtualDataGenerator.GetTestingStepOfProgram(ovenAddress);
        }

        if (!_initialized)
        {
            Initialize();
        }

        try
        {
            // Run the oven data fetch operation in a background thread
            var dataFromDevice = _owenProtocolMaster.OwenRead(ovenAddress, AddressLengthType.Bits8, "r.StP");
                    
            // Convert the data from the device to an int value
            var converterU = new ConverterU(2);
            var valueFromDevice = converterU.ConvertBack(dataFromDevice);
            return (int)valueFromDevice;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to read oven Step of Program: " + e.Message);
            throw new InvalidOperationException(
                "Failed to read oven Step of Program. Please check the connection and configuration.",
                e
            );
        }
    }
}
