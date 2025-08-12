using System;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using OwenioNet;
using OwenioNet.DataConverter;
using OwenioNet.DataConverter.Converter;
using OwenioNet.IO;
using OwenioNet.Types;

namespace OwenioNetExampleApp;

class Program
{
    static string filePath = "console_output.txt";
   
    static void Main(string[] args)
    {
        //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        StreamWriter writer = new StreamWriter(filePath);
        writer.AutoFlush = true;
        var serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        while (true)
        {
            GetTemp(serialPort, writer, 8);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 32);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 96);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 64);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 192);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 88);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 216);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 104);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 232);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 120);
            Thread.Sleep(500);
            GetTemp(serialPort, writer, 248);
            
            Console.WriteLine();
            Thread.Sleep(1000);
        }
        //SendManual();
        //ConvertData();
    }

    private static void GetTemp(SerialPort serialPort,StreamWriter writer, int deviceAddress)
    {
        // Initialize the SerialPort for communication

        serialPort.ReadTimeout = 1000;
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
        }
        // Create the OwenProtocolMaster object using SerialPort
        var owenProtocol = OwenProtocolMaster.Create(new SerialPortAdapter(serialPort), null);

        // Read data from a device
        byte[] dataFromDevice = owenProtocol.OwenRead(
            deviceAddress,
            AddressLengthType.Bits8,
            "read"
        );

        // Convert data using the converter (e.g., for a float)
        /*    var converterDecS = new ConverterDecDotS();
            var valueFromDevice1 = converterDecS.ConvertBack(dataFromDevice);
            writer.WriteLine($"Value from device: {valueFromDevice1}");
            Console.WriteLine($"Value from device: {valueFromDevice1}");*/

        // Convert data using the converter (e.g., for a float)
        var converterFloat = new ConverterFloatTimestamp();
        var valueFromDevice = converterFloat.ConvertBack(dataFromDevice);
        writer.WriteLine($"{DateTime.Now.ToString("T")},{deviceAddress},{(int)valueFromDevice.Value}");
        Console.WriteLine($"{DateTime.Now.ToString("T")},{deviceAddress},{(int)valueFromDevice.Value}");
    }

    private static void SendManual()
    {
        // Set the serial port parameters
        SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        port.ReadTimeout = 2000; // 1 second timeout

        try
        {
            port.Open();
            while (true)
            {
                // Command to read temperature from device address 01
                string command = "#IGHGONOKQOPI\r";
                port.Write(command);
                Console.Write("Response: ");
                string result = "";
                while (true)
                {
                    // Read response
                    var response = port.ReadChar(); // assumes \r or \n is used as terminator
                    result = result + (char)response;
                    if (response == '\r')
                    {
                        break;
                    }
                }
                Console.Write(result);
                Console.WriteLine();
                Console.ReadKey();
            }
        }
        finally
        {
            if (port.IsOpen)
                port.Close();
        }
    }

    static void ConvertData()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string command = "#IGHGONOKQOPI\r";
        var convertor = new OwenioNet.DataConverter.Converter.ConverterAscii();

        var hex = convertor.Convert("#IGHGONOKQOPI\r");
        Console.WriteLine(hex[0]);
    }
}
