namespace OvensManager.Enums
{
    public enum OperatingModes
    {
        Stopped,                                // 0 – режим Стоп;
        Working,                                // 1 – режим Работа;
        CriticalAccident,                       // 2 – режим Критическая Авария;
        ProgramIsCompleted,                     // 3 – программа технолога завершена;
        AutoTuneModeOfThePIDController,         // 4 – режим Автонастройка ПИД-регулятора;
        WaitingForAutoTuneModeToStart,          // 5 – ожидание запуска режима Автонастройка;
        AutoTuningOfThePIDControllerIsCompleted,// 6 – автонастройка ПИД-регулятора завершена;
        SetupMode                               // 7 – режим Настройка.
    }
}
