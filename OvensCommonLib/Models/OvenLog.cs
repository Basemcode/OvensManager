using System.ComponentModel.DataAnnotations;

namespace OvensCommonLib.Models;

public class OvenLog
{
    public int Id { get; set; }
    public int OvenNumber { get; set; }
    [MaxLength(25)]
    public string CycleStep { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public DateTime Timestamp { get; set; }
    public LogType LogType { get; set; }
}