using System.Globalization;

public class CultureHelper
{
    public static void SetCultureInfo()
    {
        // Set the current culture for thread and UI culture (in case it's missing)
        CultureInfo culture = CultureInfo.InvariantCulture; // to solve the problems in ReadInputRegisters in modbus protocol
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}
