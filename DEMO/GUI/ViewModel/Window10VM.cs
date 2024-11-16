using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DTO;
using GUI.Model;

namespace GUI.ViewModel
{
    class Window10VM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window10
        {
            get { return _pageModel.Window10; }
            set { _pageModel.Window10 = value; OnPropertyChanged(); }
        }

        public Window10VM()
        {
            _pageModel = new PageModel();
        }
    }
}
