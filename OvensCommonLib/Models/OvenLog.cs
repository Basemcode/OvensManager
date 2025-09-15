public class OvenLog
{
    public int Id { get; set; }
    public int OvenNumber { get; set; }
    public string CycleStep { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public DateTime Timestamp { get; set; }
    public LogType LogType { get; set; }
}