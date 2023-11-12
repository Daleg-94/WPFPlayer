using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFPlayer.Infrastructure.Commands;

namespace WPFPlayer.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private MainWindow _window { get; }
        public MainWindowViewModel(MainWindow window)
        {
            _window = window;
        }

    }
}
