using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SellingTicketDTO
    {
        private string flightID;
        private string id;
        private string ticketClassID;
        private string airportID;
        private DateTime sellingDate;
        private string customerName;
        private int phone;
        private string email;
        private int isDeleted;

        public string FlightID
        {
            get => flightID;
            set => flightID = value;
        }

        public string ID
        {
            get => id;
            set => id = value;
        }

        public string TicketClassID
        {
            get => ticketClassID;
            set => ticketClassID = value;
        }

        public string AirportID
        {
            get => airportID;
            set => airportID = value;
        }

        public DateTime SellingDate
        {
            get => sellingDate;
            set => sellingDate = value;
        }

        public string CustomerName
        {
            get => customerName;
            set => customerName = value;
        }

        public int Phone
        {
            get => phone;
            set => phone = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public int IsDeleted
        {
            get => isDeleted;
            set => isDeleted = value;
        }
    }
}
