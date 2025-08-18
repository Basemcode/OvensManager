using Microsoft.Extensions.Configuration;
using OvensManagerApp.Models;
using OvensManagerApp.MyControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using WpfScreenHelper;
using OvensManagerApp.Services;
using CommunityToolkit.Mvvm.Input;
using OvensManagerApp.Interfaces;

namespace OvensManagerApp.ViewModels;

public class OvensDashboardWindowViewModel: ViewModelBase, INotifyPropertyChanged,IClosable
{
    private ObservableCollection<Oven> _ovens;

    public ObservableCollection<Oven> Ovens
    {
        get { return _ovens; }
        set
        {
            _ovens = value;
            OnPropertyChanged();
        }
    }

    // For closing the window
    public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
    public RelayCommand<IClosable> WindowClosedCommand { get; set; }


    private bool _started;
    public double _dashboardFontSize = 50;
    public double DashboardFontSize
    {
        get { return _dashboardFontSize; }
        set
        {
            _dashboardFontSize = value;
            OnPropertyChanged();
        }
    }


    public OvensDashboardWindowViewModel()
    {
        this.CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
        _ovens = new ObservableCollection<Oven>(LoadOvens());
    }


    public async void Start()
    {
        _started = true;
        while (_started)
        {
            foreach (var oven in _ovens)
            {
                /*var temperature = OvenDataService.Instance.GetOvenTemperature(oven.Address);
                ovenCards[oven.Number].OvenTemperature = temperature;
                ovenCards[oven.Number].RunTime = TimeSpan.FromHours(2);*/
            }
            //delay 5 seconds
            await Task.Delay(1000);

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



    private void CloseWindow(IClosable window)
    {
        if (window != null)
        {
            window.Close();
        }
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}
