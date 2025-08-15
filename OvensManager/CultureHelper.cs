using System.Globalization;

public class CultureHelper
{
    public static void SetCultureInfo()
    {
        // Set the current culture for thread and UI culture (in case it's missing)
        CultureInfo culture = CultureInfo.InvariantCulture; // Or set to any specific culture if required
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}
