using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace OvensManagerApp.Models;

public class Oven : INotifyPropertyChanged
{
    private int _number;
    private byte _address;
    private float _temperature;
    private TimeSpan _runTime;
    private Brush _backgroundColor = new SolidColorBrush(Colors.White);
    private Brush _fontColor = new SolidColorBrush(Colors.Black);
    private int _targetTemp;
    private int _openingTemp;
    private string _status = "Idle";

    public int Number
    {
        get => _number;
        set { _number = value; OnPropertyChanged(); }
    }

    public byte Address
    {
        get => _address;
        set { _address = value; OnPropertyChanged(); }
    }

    public float Temperature
    {
        get => _temperature;
        set { _temperature = value; OnPropertyChanged(); }
    }

    public TimeSpan RunTime
    {
        get => _runTime;
        set { _runTime = value; OnPropertyChanged(); }
    }

    public Brush BackgroundColor
    {
        get => _backgroundColor;
        set { _backgroundColor = value; OnPropertyChanged(); }
    }

    public Brush FontColor
    {
        get => _fontColor;
        set { _fontColor = value; OnPropertyChanged(); }
    }

    public int TargetTemp
    {
        get => _targetTemp;
        set { _targetTemp = value; OnPropertyChanged(); }
    }

    public int OpeningTemp
    {
        get => _openingTemp;
        set { _openingTemp = value; OnPropertyChanged(); }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
