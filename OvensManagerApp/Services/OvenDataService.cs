using Microsoft.Extensions.Configuration;
using OvensManagerApp.Configuration;
using OwenioNet;
using OwenioNet.DataConverter.Converter;
using OwenioNet.IO;
using OwenioNet.Types;
using System.IO.Ports;

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
            lock (lockObject) // Acquire lock
            {
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

    public float GetOvenTemperature(int ovenAddress)
    {
        if (Helpers.TestingHelper.IsDevelop) { return Random.Shared.Next(50, 760); }
        if (!_initialized) { Initialize(); }

        // Read data from a device
        try
        {
        byte[] dataFromDevice = _owenProtocolMaster.OwenRead(
            ovenAddress,
            AddressLengthType.Bits8,
            "read"
        );
        

        // Convert the data from the device to a float value
        var converterFloat = new ConverterFloatTimestamp();
        var valueFromDevice = converterFloat.ConvertBack(dataFromDevice);
        return valueFromDevice.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to read oven Temperature. Please check the connection and configuration. " + e.Message);
            throw new InvalidOperationException("Failed to read oven temperature. Please check the connection and configuration.");
        }
    }

    public int GetOvenOperatingMode(int ovenAddress)
    {
        if (Helpers.TestingHelper.IsDevelop) { return Random.Shared.Next(0, 7); }
        if (!_initialized) { Initialize(); }

        // Read data from a device
        try
        {
            byte[] dataFromDevice = _owenProtocolMaster.OwenRead(
                ovenAddress,
                AddressLengthType.Bits8,
                "r.St"
            );


            // Convert the data from the device to a int value
            var converterU = new ConverterU(2);
            var valueFromDevice = converterU.ConvertBack(dataFromDevice);
            return (int)valueFromDevice;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to read oven Operating Mode. Please check the connection and configuration. "+e.Message);
            throw new InvalidOperationException("Failed to read oven Operating Mode. Please check the connection and configuration.");
        }
    }

    public int GetOvenStepOfProgram(int ovenAddress)
    {
        if (Helpers.TestingHelper.IsDevelop) { return Random.Shared.Next(1, 5); }
        if (!_initialized) { Initialize(); }

        // Read data from a device
        try
        {
            byte[] dataFromDevice = _owenProtocolMaster.OwenRead(
                ovenAddress,
                AddressLengthType.Bits8,
                "r.StP"
            );


            // Convert the data from the device to a int value
            var converterU = new ConverterU(2);
            var valueFromDevice = converterU.ConvertBack(dataFromDevice);
            return (int)valueFromDevice;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to read oven Step of program. Please check the connection and configuration. " + e.Message);
            throw new InvalidOperationException("Failed to read oven Step of program. Please check the connection and configuration.");
        }
    }
}
