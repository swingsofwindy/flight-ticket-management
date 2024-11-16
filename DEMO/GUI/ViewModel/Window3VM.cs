using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Model;

namespace GUI.ViewModel
{
    class Window3VM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window3
        {
            get { return _pageModel.Window3; }
            set { _pageModel.Window3 = value; OnPropertyChanged(); }
        }

        public Window3VM()
        {
            _pageModel = new PageModel();
        }
    }
}
