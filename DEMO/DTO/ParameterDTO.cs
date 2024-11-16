using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ParameterDTO
    {
        private int airportCount;
        private TimeSpan minFlighTime;
        private int intermediateAirportCount;
        private int ticketClassCount;
        private TimeSpan minStopTime;
        private TimeSpan maxStopTime;
        private int seatCount;
        private TimeSpan slowestBookingTime;
        private TimeSpan cancelTime;
        private int isDeleted;

        public int AirportCount
        {
            get => airportCount;
            set => airportCount = value;
        }

        public TimeSpan MinFlighTime
        {
            get => minFlighTime;
            set => minFlighTime = value;
        }

        public int IntermediateAirportCount
        {
            get => intermediateAirportCount;
            set => intermediateAirportCount = value;
        }

        public int TicketClassCount
        {
            get => ticketClassCount;
            set => ticketClassCount = value;
        }

        public TimeSpan MinStopTime
        {
            get => minStopTime;
            set => minStopTime = value;
        }

        public TimeSpan MaxStopTime
        {
            get => maxStopTime;
            set => maxStopTime = value;
        }

        public TimeSpan SlowestBookingTime
        {
            get => slowestBookingTime;
            set => slowestBookingTime = value;
        }

        public TimeSpan CancelTime
        {
            get => cancelTime;
            set => cancelTime = value;
        }
    }
}
