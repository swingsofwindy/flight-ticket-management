using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Model;

namespace GUI.ViewModel
{
    class Window8VM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window8
        {
            get { return _pageModel.Window8; }
            set { _pageModel.Window8 = value; OnPropertyChanged(); }
        }

        public Window8VM()
        {
            _pageModel = new PageModel();
        }
    }
}
