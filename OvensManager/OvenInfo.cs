using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OvensManager.Enums;

namespace OvensManager;

public class OvenInfo 
{
    public ushort[]? Registers { get; set; } = null;

    public decimal CurrentTemperature
    {
        get
        {
            if (Registers is not null)
            {
                var temp = Convert.ToDecimal(Registers[2]);
                temp = temp / (int)(Math.Pow(10, Registers[0]));
                return temp;
            }
            throw new Exception("Registers are null");
        }
    }

    public int CurrentProgram
    {
        get
        {
            if (Registers is not null)
                return Registers[15];
            else
                throw new Exception("Registers are null");
            ;
        }
    }

    public int CurrentStep
    {
        get
        {
            if (Registers is not null)
                return Registers[16];
            else
                throw new Exception("Registers are null");
            ;
        }
    }

    public OperatingModes OperatingMode
    {
        get
        {
            if (Registers is not null)
                return (OperatingModes)Registers[17];
            else
                throw new Exception("Registers are null");
            ;
        }
    }
}
