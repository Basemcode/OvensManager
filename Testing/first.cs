using System;
using System.IO.Ports;

class Testing
{
    static void Main1()
    {
        SendManual();
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
                    if (response == '\r') { break; }

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
}
