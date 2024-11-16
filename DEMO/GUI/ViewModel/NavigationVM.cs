using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Ultilities; 

namespace GUI.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand Window1Command { get; set; }
        public ICommand Window2Command { get; set; }
        public ICommand Window3Command { get; set; }
        public ICommand FlightScheduleWindowCommand { get; set; }
        public ICommand ReportWindowCommand { get; set; }
        public ICommand Window6Command { get; set; }
        public ICommand Window7Command { get; set; }
        public ICommand Window8Command { get; set; }
        public ICommand Window9Command { get; set; }
        public ICommand Window10Command { get; set; }

        private void Window1(object obj) => CurrentView = new Window1VM();
        private void Window2(object obj) => CurrentView = new Window2VM();
        private void Window3(object obj) => CurrentView = new Window3VM();
        private void FlightScheduleWindow(object obj) => CurrentView = new FlightScheduleWindowVM();
        private void ReportWindow(object obj) => CurrentView = new ReportVM();
        private void Window6(object obj) => CurrentView = new Window6VM();
        private void Window7(object obj) => CurrentView = new Window7VM();
        private void Window8(object obj) => CurrentView = new Window8VM();
        private void Window9(object obj) => CurrentView = new Window9VM();
        private void Window10(object obj) => CurrentView = new Window10VM();

        public NavigationVM()
        {
            Window1Command = new RelayCommand(Window1);
            Window2Command = new RelayCommand(Window2);
            Window3Command = new RelayCommand(Window3);
            FlightScheduleWindowCommand = new RelayCommand(FlightScheduleWindow);
            ReportWindowCommand = new RelayCommand(ReportWindow);
            Window6Command = new RelayCommand(Window6);
            Window7Command = new RelayCommand(Window7);
            Window8Command = new RelayCommand(Window8);
            Window9Command = new RelayCommand(Window9);
            Window10Command = new RelayCommand(Window10);

            CurrentView = new Window1VM();
        }
    }
}
