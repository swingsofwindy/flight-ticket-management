using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.SqlServer.Server;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace BLL
{
    public class SearchProcessor
    {
        string state = string.Empty;
        public SearchProcessor() { }
        public List<FlightInforDTO> GetFlightInfoDTO(string sourceAirportID, string destinationAirportID
                                                                       , DateTime startDate, DateTime endDate)
        {
            FlightAccess flightAccess = new FlightAccess();
            SellingTicketAcess sellingTicketAcess = new SellingTicketAcess();
            Ticket_classAccess ticket_ClassAccess = new Ticket_classAccess();
            this.state = string.Empty;
            
            List<FlightInforDTO> data = new List<FlightInforDTO>();
            List<FlightDTO> flights = new List<FlightDTO>();
            try
            {
                flights = flightAccess.getFlight(sourceAirportID, destinationAirportID, startDate, endDate);
                if (flightAccess.GetState() != string.Empty)
                {
                    return data;
                }

                foreach (FlightDTO flight in flights)
                {
                    FlightInforDTO flightInformationSearchDTO = new FlightInforDTO();
                    flightInformationSearchDTO.Flight = flight;
                    flightInformationSearchDTO.bookedTickets = sellingTicketAcess.getTicketSales_byFlightID(flight.FlightID);
                    flightInformationSearchDTO.emptySeats = ticket_ClassAccess.getTotalSeat_byFlightID(flight.FlightID) - flightInformationSearchDTO.bookedTickets;
                    data.Add(flightInformationSearchDTO);
                }
            }
            catch (Exception ex)
            {
                this.state = $"Error: {ex.Message}";
                return data;
            }
            return data;
        }

        public List<FlightInforDTO> GetFlightInfoDTO(string sourceAirportID, string destinationAirportID, DateTime startDate, DateTime endDate, string ticketClass, int numTicket)
        {
            FlightAccess flightAccess = new FlightAccess();
            SellingTicketAcess sellingTicketAcess = new SellingTicketAcess();
            Ticket_classAccess ticket_ClassAccess = new Ticket_classAccess();
            this.state = string.Empty;

            List<FlightInforDTO> data = new List<FlightInforDTO>();
            List<FlightDTO> flights = new List<FlightDTO>();

            try
            {
                flights = flightAccess.getFlight(sourceAirportID, destinationAirportID, startDate, endDate, ticketClass, numTicket);
                if (flightAccess.GetState() != string.Empty)
                {
                    return data;
                }

                var parameter = new BLL.SearchProcessor().GetParameterDTO();
                List<FlightDTO> resultflights = new List<FlightDTO>();
                foreach (FlightDTO flight in flights)
                {
                    if (flight.FlightDay > DateTime.Now.Add(parameter.SlowestBookingTime))
                    {
                        resultflights.Add(flight);
                    }
                }

                foreach (FlightDTO flight in resultflights)
                {
                    FlightInforDTO flightInformationSearchDTO = new FlightInforDTO();
                    flightInformationSearchDTO.Flight = flight;
                    flightInformationSearchDTO.bookedTickets = sellingTicketAcess.getTicketSales_byFlightID_TicketClassID(flight.FlightID, ticketClass);
                    flightInformationSearchDTO.emptySeats = ticket_ClassAccess.getTotalSeat_byFlightID_TicketClassID(flight.FlightID, ticketClass) - flightInformationSearchDTO.bookedTickets;
                    data.Add(flightInformationSearchDTO);
                }
            }
            catch (Exception ex)
            {
                this.state = $"Error: {ex.Message}";
                return data;
            }
            return data;
        }


        /* 
           Hàm sort được Generalize với mức bất kì ObservableCollection<T>
           Tuy nhiên có thể không hoạt động đúng cách nếu T chứa thuộc tính kiểu non-primitive
                + items: ObservableCollection
                + propertyName: tên thuộc tính trong T, nếu không có trong T sẽ gây ra exception, sẽ xử lí sau nếu phát sinh
                + sortOrder: "ASC" hoặc "DESC", nếu không sẽ gây ra exception, sẽ xử lí sau nếu phát sinh
        */
        public static ObservableCollection<T> SortItems<T>(ObservableCollection<T> items, string propertyName, string sortOrder)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(typeof(T), propertyName);
            return SortCollection(items, propertyInfo, sortOrder);
        }

        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            return propertyInfo;
        }

        public static ObservableCollection<T> SortCollection<T>(ObservableCollection<T> items, PropertyInfo propertyInfo, string sortOrder)
        {
            IEnumerable<T> sortedItems;
            if (sortOrder == "ASC")
            {
                sortedItems = items.OrderBy(item => propertyInfo.GetValue(item, null));
            }
            else
            {
                sortedItems = items.OrderByDescending(item => propertyInfo.GetValue(item, null));
            }
            return new ObservableCollection<T>(sortedItems);
        }

        public ParameterDTO GetParameterDTO()
        {
            ParameterDTO parameterDTO = new ParameterDTO();
            try
            {
                parameterDTO = new DAL.ParameterAccess().GetParameters();
            }
            catch (Exception ex)
            {
                this.state = $"Error: {ex.Message}";

            }
            return parameterDTO;
        }
        public List<BookingTicketDTO> GetBookingTicket(string TicketID, string CustomerID, string FLigthID, int Status)
        {
            List<BookingTicketDTO> data = new List<BookingTicketDTO>();
            try
            {   
                DAL.BookingTicketAccess prc = new DAL.BookingTicketAccess();
                data = prc.GetBookingTicket(TicketID, CustomerID, FLigthID, Status);
                this.state = prc.GetState();
            }
            catch (Exception ex)
            {
                this.state = $"Error: {ex.Message}";
            }
            return data;
        }
        public int GetNumIntermidateAirport(string FlightID)
        {
            int count = 0;
            try
            {
                count = new DAL.IntermidateAirportAccess().GetNumIntermidateAirport(FlightID);
            }
            catch (Exception ex) 
            {
                this.state = $"Error: {ex.Message}";
            }
            return count;
        }
        public List<ACCOUNT> GetAccounts(ACCOUNT account)
        {
            List <ACCOUNT> data = new List<ACCOUNT>();  
            try
            {
                data = new DAL.AccountAccess().GetMember(account);
            }
            catch (Exception ex)
            {
                this.state = $"Error: {ex.Message}";
            }
            return data;
        }
        public int GetNumIntermidiateAirport()
        {
            int count = 0;
            try
            {
                count = new DAL.IntermidateAirportAccess().GetNumIntermidiateAirport();
            }
            catch (Exception ex)
            {
                this.state = $"Error: {ex.Message}";
            }
            return count;
        }

        public string GetState()
        {
            return this.state;
        }
    }
}
