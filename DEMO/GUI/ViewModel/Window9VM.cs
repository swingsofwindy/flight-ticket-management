using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Model;

namespace GUI.ViewModel
{
    class Window9VM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window9
        {
            get { return _pageModel.Window9; }
            set { _pageModel.Window9 = value; OnPropertyChanged(); }
        }

        public Window9VM()
        {
            _pageModel = new PageModel();
        }
    }
}
