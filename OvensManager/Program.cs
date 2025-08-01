using NModbus;
using NModbus.Extensions.Enron;
using NModbus.Serial;
using NModbus.Utility;
using System.IO.Ports;
namespace OvensManager;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}