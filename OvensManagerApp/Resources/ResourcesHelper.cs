using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OvensManagerApp.Resources
{
    public class ResourcesHelper
    {
        // Brushes for ovenCard background
        public static Brush redBrush = (Brush)Application.Current.FindResource("RedGradient");
        public static Brush greenBrush = (Brush)Application.Current.FindResource("GreenGradient");
        public static Brush yellowBrush = (Brush)Application.Current.FindResource("YellowGradient");
        public static Brush blackBrush = (Brush)Application.Current.FindResource("BlackGradient");
        public static Brush blueBrush = (Brush)Application.Current.FindResource("BlueGradient");
    }
}
