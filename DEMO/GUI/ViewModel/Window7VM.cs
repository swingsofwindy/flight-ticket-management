using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Model;

namespace GUI.ViewModel
{
    class Window7VM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window7
        {
            get { return _pageModel.Window7; }
            set { _pageModel.Window7 = value; OnPropertyChanged(); }
        }

        public Window7VM()
        {
            _pageModel = new PageModel();
        }
    }
}
