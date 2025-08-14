using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color =System.Windows.Media.Color;

namespace OvensManagerApp.Controls
{
    public partial class OvenControl : UserControl, INotifyPropertyChanged
    {
        private int ovenNumber;
        private float ovenTemperature;
        private string ovenStatus;
        private TimeOnly runTime;
        private Brush backgroundColor;
        private double ovenFontSize = 1;
        private Brush fontColor;
        public OvenControl()
        {
            DataContext = this;
            InitializeComponent();
            
        }

        public int OvenNumber
        {
            get => ovenNumber;
            set
            {
                ovenNumber = value;
                OnPropertyChanged();
            }
        }

        public float OvenTemperature
        {
            get => ovenTemperature;
            set
            {
                ovenTemperature = value;
                OnPropertyChanged();
            }
        }

        public string OvenStatus
        {
            get => ovenStatus;
            set
            {
                ovenStatus = value;
                OnPropertyChanged();
            }
        }

        public TimeOnly RunTime
        {
            get => runTime;
            set
            {
                runTime = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public double OvenFontSize
        {
            get => ovenFontSize;
            set
            {
                ovenFontSize = value;
                OnPropertyChanged();
            }
        }

        public Brush FontColor
        {
            get => fontColor;
            set
            {
                fontColor = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
