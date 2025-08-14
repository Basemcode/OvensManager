using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfScreenHelper;

namespace OvensManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MoveToSecondScreen();
            InitializeComponent();
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
    }
}
