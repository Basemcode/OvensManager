using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvensManagerApp.Models;

public class Oven
{
    public int  Number { get; set; }
    public byte Address { get; set; }
    public int  TargetTemp { get; set; }
    public int  OpeningTemp { get; set; }
}
