using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvensManager;

public class OvenDataService
{
    public static async Task<OvenInfo> GetOvenInfo(byte ovenId)
    {
        OvenInfo ovenInfo = new OvenInfo();

        ushort startAddress = 0;
        ushort numRegisters = 13; // maximum 13

        ModbusManager.Instance.Initialize("COM20"); // Initialize with the correct COM port

        // read first 13 registers
        ushort[] registers0to12 = await ModbusManager.Instance.ReadInputRegisters(
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
        ushort[] registers15to17 =await ModbusManager.Instance.ReadInputRegisters(
            ovenId,
            startAddress,
            numRegisters
        );

        var registers13to17 = registers13to14.Concat(registers15to17);
        ovenInfo.Registers = [.. registers0to12, .. registers13to17];

        return ovenInfo;
    }
}
