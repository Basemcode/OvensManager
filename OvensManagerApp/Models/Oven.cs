using System.Windows.Media;

namespace OvensManagerApp.Models;

public class Oven
{
    public int Number { get; set; }
    public byte Address { get; set; }
    public float Temperature { get; set; }
    public TimeSpan RunTime { get; set; }
    public Brush BackgroundColor { get; set; } 
    public Brush FontColor { get; set; }
    public int TargetTemp { get; set; }
    public int OpeningTemp { get; set; }
    public string Status { get; set; } = "Idle";
}
