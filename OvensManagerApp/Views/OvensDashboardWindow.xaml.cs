using Microsoft.Extensions.Configuration;
using OvensManagerApp.Models;
using OvensManagerApp.MyControls;
using OvensManagerApp.Services;
using System.Windows;
using WpfScreenHelper;

namespace OvensManagerApp.Views
{
    /// <summary>
    /// Interaction logic for OvensDashboardWindow.xaml
    /// </summary>
    public partial class OvensDashboardWindow : Window
    {
        private List<Oven> _ovens;
        public OvensDashboardWindow()
        {
            MoveToSecondScreen();
            InitializeComponent();
            _ovens = LoadOvens();
            // Assuming OvenCard has a public property `OvenNumber`
            var ovenCards = new Dictionary<int, OvenCard>();

            foreach (var child in myGrid.Children)
            {
                if (child is OvenCard ovenCard)
                {
                    ovenCards[ovenCard.OvenNumber] = ovenCard;
                }
            }
        }
        private void MoveToSecondScreen()
        {
            // get all the available screens
            var screens = WpfScreenHelper.Screen.AllScreens.ToList();
            Screen screen;

            // check if we have more than one screen
            if (screens.Count < 2)
            {
                screen = screens[0];
            }
            else
            {
                screen = screens[1];
            }

            // Set the position to the selected screen with maximaized size
            Left = screen.WpfBounds.Left;
            Top = screen.WpfBounds.Top;
            Height = screen.WpfBounds.Height;
            Width = screen.WpfBounds.Width;
        }

        public void MaximizeWindow()
        {
            WindowState = WindowState.Normal;
            Thread.Sleep(100);
            WindowState = WindowState.Maximized;
        }

        public void Start()
        {
            foreach (var oven in _ovens)
            {
                var temperature = OvenDataService.Instance.GetOvenTemperature(oven.Address);

            }
        }
        public List<Oven> LoadOvens()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return configuration.GetSection("Ovens").Get<List<Oven>>() ?? new List<Oven>();
        }

    }
}
