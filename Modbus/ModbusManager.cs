using NModbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NModbus;
using NModbus.Serial;
using System.Net.Sockets;

namespace OvensManager;

public class ModbusManager
{
    private static ModbusManager _instance;
    private IModbusSerialMaster _master;
    private SerialPort _serialPort;

    // Private constructor to prevent instantiation
    private ModbusManager() { }

    // Public static method to get the single instance
    public static ModbusManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ModbusManager();
            }
            return _instance;
        }
    }

    // Initialize the serial port and Modbus master
    public void Initialize(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        if (_serialPort == null)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _serialPort.ReadTimeout = 500; // 500 millisecond
            _serialPort.Open();
            var factory = new ModbusFactory();
            _master = factory.CreateRtuMaster(_serialPort); 
        }
    }

    // Read Input Registers from a Modbus slave
    public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numRegisters)
    {
        return _master.ReadInputRegisters(slaveAddress, startAddress, numRegisters);
    }

    // Close the serial port connection
    public void Close()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }


    // Functions to be used later //
    // Read coils from a Modbus slave
    private bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numCoils)
    {
        return _master.ReadCoils(slaveAddress, startAddress, numCoils);
    }

    // Write coils to a Modbus slave
    private void WriteCoils(byte slaveAddress, ushort startAddress, bool[] values)
    {
        _master.WriteMultipleCoils(slaveAddress, startAddress, values);
    }

    // Read holding registers from a Modbus slave
    private ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numRegisters)
    {
        return _master.ReadHoldingRegisters(slaveAddress, startAddress, numRegisters);
    }

    // Write holding registers to a Modbus slave
    private void WriteHoldingRegisters(byte slaveAddress, ushort startAddress, ushort[] values)
    {
        _master.WriteMultipleRegisters(slaveAddress, startAddress, values);
    }

    
}
