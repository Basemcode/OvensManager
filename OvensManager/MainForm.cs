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
        new Oven() { Number = 2, Id = 120 },
        new Oven() { Number = 3, Id = 104 },
    };

    public MainForm()
    {
        InitializeComponent();
    }

    private async void StartReading()
    {
        started = true;
        var numberOfReadings = 0;
        while (started)
        {
            OvenInfo ovenInfo0 = await OvenDataService.GetOvenInfo(ovens[0].Id);
            
                if (ovenInfo0.Registers != null)
                {
                    lblReg0.Text = ovenInfo0.CurrentTemperature.ToString();
                    lblReg1.Text = ovenInfo0.CurrentProgram.ToString();
                    lblReg2.Text = ovenInfo0.CurrentStep.ToString();
                    lblReg3.Text = ovenInfo0.OperatingMode.ToString();

                    numberOfReadings++;
                    lblStatus.Text = numberOfReadings.ToString();
                }
            
            

            OvenInfo ovenInfo1 = await OvenDataService.GetOvenInfo(ovens[1].Id);
            try
            {
                if (ovenInfo1.Registers != null)
                {
                    lblReg4.Text = ovenInfo1.CurrentTemperature.ToString();
                    lblReg5.Text = ovenInfo1.CurrentProgram.ToString();
                    lblReg6.Text = ovenInfo1.CurrentStep.ToString();
                    lblReg7.Text = ovenInfo1.OperatingMode.ToString();

                    numberOfReadings++;
                    lblStatus.Text = numberOfReadings.ToString();
                }
            }
            catch (Exception)
            {
                //ToDo: Handle exception if needed
            }

            await Task.Delay(500);
        }
    }

    /// <summary>
    ///     Simple Modbus serial RTU master read holding registers example.
    /// </summary>
    public static ushort[] ModbusSerialRTUMasterReadRegisters(byte ovenId)
    {
        try
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
                ushort numRegisters = 13; // maximum 13

                // read first 13 registers
                ushort[] registers0to12 = master.ReadInputRegisters(
                    ovenId,
                    startAddress,
                    numRegisters
                );

                startAddress = 15;
                numRegisters = 3; // maximum 13

                // empty items in the array that represent the registers that we will not read
                // to prevent errors
                ushort[] registers13to14 = [0, 0];

                // read next 3 registers
                ushort[] registers15to17 = master.ReadInputRegisters(
                    ovenId,
                    startAddress,
                    numRegisters
                );

                var registers13to17 = registers13to14.Concat(registers15to17);
                var registers = registers0to12.Concat(registers13to17);
                return registers.ToArray();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + " ovenId: " + ovenId);
            throw;
        }
    }

    private void btnStartReading_Click(object sender, EventArgs e)
    {
        StartReading();
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        started = false;
        lblStatus.Text = "Stopped!";
    }
}
