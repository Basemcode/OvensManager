namespace OvensCommonLib.Interfaces;

public interface IOvenSnapshot
{
    int Number { get; }
    float Temperature { get; }
    string CycleStep { get; }
}
