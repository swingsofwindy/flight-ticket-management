using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Windows.Input;
using GUI.Model;
using GUI.Ultilities;
using GUI.View;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Markup;

namespace GUI.ViewModel
{
    class FlightScheduleWindowVM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window4
        {
            get { return _pageModel.Window4; }
            set { _pageModel.Window4 = value; OnPropertyChanged(); }
        }

        public FlightScheduleWindowVM()
        {
            _pageModel = new PageModel();
        }
    }

    public class TicketClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _id;
        private string _name;
        private int _quantity;
        private decimal _multiplier;
        private string _buttonContent = "Edit";

        public string ID { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public int Quantity { get => _quantity; set { _quantity = value; OnPropertyChanged(); } }

        public decimal Multiplier { get => _multiplier; set { _multiplier = value; OnPropertyChanged(); } }
        public string ButtonContent
        {
            get => _buttonContent;
            set { _buttonContent = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Quantity: {Quantity}, Multiplier: {Multiplier}";
        }
    }

    public class IntermediateAirport : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _id;
        private string _name;
        private TimeSpan _layoverTime;
        private string _note;
        private string _buttonContent = "Edit";

        public string ID { get => _id; set { if (_id != value) { _id = value; OnPropertyChanged(); } } }

        public string Name { get => _name; set { if (_name != value) { _name = value; OnPropertyChanged(); } } }

        public TimeSpan LayoverTime { get => _layoverTime; set { if (_layoverTime != value) { _layoverTime = value; OnPropertyChanged(); } } }

        public string Note { get => _note; set { if (_note != value) { _note = value; OnPropertyChanged(); } } }

        public string ButtonContent
        {
            get => _buttonContent;
            set { _buttonContent = value; OnPropertyChanged(); }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Layover Time: {LayoverTime}, Note: {Note}";
        }
    }
    public class ScheduleData
    {
        public string flightID;
        public string sourceAirportID;
        public string destinationAirportID;
        public decimal price;
        public DateTime flightDay;
        public TimeSpan flightTime;
        public ObservableCollection<TicketClass> ticketList;
        public ObservableCollection<IntermediateAirport> IAList;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Flight ID: {flightID} ({GetDataType(nameof(flightID))})");
            sb.AppendLine($"Source Airport ID: {sourceAirportID} ({GetDataType(nameof(sourceAirportID))})");
            sb.AppendLine($"Destination Airport ID: {destinationAirportID} ({GetDataType(nameof(destinationAirportID))})");
            sb.AppendLine($"Price: {price} ({GetDataType(nameof(price))})");
            sb.AppendLine($"Flight Day: {flightDay} ({GetDataType(nameof(flightDay))})");
            sb.AppendLine($"Flight Time: {flightTime} ({GetDataType(nameof(flightTime))})");
            sb.AppendLine("Ticket List:");
            foreach (var ticket in ticketList)
            {
                sb.AppendLine($"  {ticket.ToString()}");
            }
            sb.AppendLine("Intermediate Airports:");
            foreach (var ia in IAList)
            {
                sb.AppendLine($"  {ia.ToString()}");
            }
            return sb.ToString();
        }

        private string GetDataType(string propertyName)
        {
            var fieldInfo = this.GetType().GetField(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType.Name;
            }
            var propertyInfo = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                return propertyInfo.PropertyType.Name;
            }
            return "Unknown";
        }

        public FlightDTO InitializeFlightDTO()
        {
            return new FlightDTO
            {
                DestinationAirportID = this.destinationAirportID,
                SourceAirportID = this.sourceAirportID,
                FlightID = this.flightID,
                Price = this.price,
                FlightDay = this.flightDay,
                FlightTime = this.flightTime
            };
        }

        public AirportDTO InitializeAirportDTO()
        {
            return new AirportDTO();
        }

        public List<TicketClassDTO> InitializeListTicketClassDTO()
        {
            List<TicketClassDTO> list = new List<TicketClassDTO>();
            foreach (var ticketclass in this.ticketList)
            {
                list.Add(new TicketClassDTO
                {
                    TicketClassID = ticketclass.ID,
                    TicketClassName = ticketclass.Name,
                });
            }
            return list;
        }

        public List<TicketClassFlightDTO> InitializeListTicketClassFlightDTO()
        {
            List<TicketClassFlightDTO> list = new List<TicketClassFlightDTO>();
            foreach (var ticketclass in this.ticketList)
            {
                list.Add(new TicketClassFlightDTO
                {
                    TicketClassID = ticketclass.ID,
                    FlightID = this.flightID,
                    Quantity = ticketclass.Quantity,
                    Multiplier = ticketclass.Multiplier,
                });
            }
            return list;
        }

        public List<IntermediateAirportDTO> InitializeListIntermediateAirportDTO()
        {
            List<IntermediateAirportDTO> list = new List<IntermediateAirportDTO>();
            foreach (var airport in this.IAList)
            {
                list.Add(new IntermediateAirportDTO
                {
                    FlightID = this.flightID,
                    AirportID = airport.ID,
                    LayoverTime = airport.LayoverTime,
                    Note = airport.Note
                });
            }
            return list;
        }
    }
}
