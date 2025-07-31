using System.IO.Ports;
using NModbus;
using NModbus.Serial;

namespace OvensManager;

public partial class MainForm : Form
{
    private const string PrimarySerialPortName = "COM15";
    private bool started = false;
    List<Oven> ovens = new List<Oven>()
    {
        new Oven() { Number = 2, Id = 16 },
        new Oven() { Number = 3, Id = 24 },
    };

    public MainForm()
    {
        InitializeComponent();
    }

    private void StartReading() { }

    /// <summary>
    ///     Simple Modbus serial RTU master read holding registers example.
    /// </summary>
    public static ushort[] ModbusSerialRTUMasterReadRegisters(byte ovenId)
    {
        using (SerialPort port = new SerialPort(PrimarySerialPortName))
        {
            // configure serial port
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.ReadTimeout = 1000;
            port.Open();

            var factory = new ModbusFactory();
            IModbusSerialMaster master = factory.CreateRtuMaster(port);

            ushort startAddress = 0;
            ushort numRegisters = 18;

            // read five registers
            ushort[] registers = master.ReadInputRegisters(ovenId, startAddress, numRegisters);

            return registers;
        }
    }

    private async void btnStartReading_Click(object sender, EventArgs e)
    {
        started = true;
        var numberOfReadings = 0;
        while (started)
        {
            PLCValues pLCValues = new PLCValues();
            pLCValues.Registers = ModbusSerialRTUMasterReadRegisters(16);
            if (pLCValues.Registers != null)
            {
                lblReg0.Text = pLCValues.CurrentTemperature.ToString();
                lblReg1.Text = pLCValues.CurrentProgram.ToString();
                lblReg2.Text = pLCValues.CurrentStep.ToString();
                lblReg3.Text = pLCValues.OperatingMode.ToString();

                numberOfReadings++;
                lblStatus.Text = numberOfReadings.ToString();
            }
            await Task.Delay(500);
        }
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        started = false;
        lblStatus.Text = "Stopped!";
    }
}
