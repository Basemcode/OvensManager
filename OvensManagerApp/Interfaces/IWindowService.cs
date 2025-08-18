using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvensManagerApp.Interfaces;

public interface IWindowService
{
    void ShowWindow<TViewModel>(TViewModel viewModel) where TViewModel : class;
    bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class;
}
