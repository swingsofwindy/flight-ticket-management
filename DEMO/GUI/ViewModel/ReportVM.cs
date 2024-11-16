using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using GUI.Model;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using DTO;

namespace GUI.ViewModel
{
    class ReportVM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Report
        {
            get { return _pageModel.Window5; }
            set { _pageModel.Window5 = value; OnPropertyChanged(); }
        }

        public ReportVM()
        {
            _pageModel = new PageModel();
        }

        
    }
    public class ReportByFlightData
    {
        public string flightID { get; set; } 
        public int ticketsSold { get; set; } 
        public decimal revenue { get; set; }
        public decimal ratio { get; set; }

        public static ReportByFlightData Convert(ReportByFlightDTO reportByFlightDTO)
        {
            return new ReportByFlightData
            {
                flightID = reportByFlightDTO.flightID,
                ratio = reportByFlightDTO.ratio,
                revenue = reportByFlightDTO.revenue,
                ticketsSold = reportByFlightDTO.ticketsSold
            };
        }
        public static ObservableCollection<ReportByFlightData> ConvertListDTOToObservableCollectionData(List<ReportByFlightDTO> list)
        {
            var observableCollection = new ObservableCollection<ReportByFlightData>();
            foreach (ReportByFlightDTO item in list)
            {
                observableCollection.Add(ReportByFlightData.Convert(item));
            }
            return observableCollection;
        }
    }

    public class ReportByMonthData
    {
        public string month_year { get; set; }
        public int flightQuantity { get; set; }
        public decimal revenue { get; set; }
        public decimal ratio { get; set; }
        public static ReportByMonthData Convert(ReportByMonthDTO reportByMonthDTO)
        {
            return new ReportByMonthData
            {
                month_year = reportByMonthDTO.time.ToString("MM-yyyy"),
                ratio = reportByMonthDTO.ratio,
                revenue = reportByMonthDTO.revenue,
                flightQuantity = reportByMonthDTO.flightQuantity
            };
        }
        public static ObservableCollection<ReportByMonthData> ConvertListDTOToObservableCollectionData(List<ReportByMonthDTO> list)
        {
            var observableCollection = new ObservableCollection<ReportByMonthData>();
            foreach (ReportByMonthDTO item in list)
            {
                observableCollection.Add(ReportByMonthData.Convert(item));
            }
            return observableCollection;
        }
    }
}
