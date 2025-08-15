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
            _serialPort = new SerialPort(config.PortName, baudRate, parity, dataBits, stopBits);
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
        if (!_initialized) { Initialize(); }

        // Read data from a device
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
}
