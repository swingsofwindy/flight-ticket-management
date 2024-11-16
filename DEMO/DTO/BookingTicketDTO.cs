using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BookingTicketDTO
    {
        private string ticketID;
        private string flightID;
        private string id;
        private string ticketClassID;
        private int ticketStatus;
        private DateTime bookingDate;
        private int isDeleted;

        public string TicketID
        {
            get => ticketID;
            set => ticketID = value;
        }

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

        public int TicketStatus
        {
            get => ticketStatus;
            set => ticketStatus = value;
        }

        public DateTime BookingDate
        {
            get => bookingDate;
            set => bookingDate = value;
        }
        public int IsDeleted
        {
            get => isDeleted;
            set => isDeleted = value;
        }
    }
}
