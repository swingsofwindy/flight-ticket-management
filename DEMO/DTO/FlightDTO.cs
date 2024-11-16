using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FlightDTO
    {
        private string flightID;
        private string sourceAirportID;
        private string destinationAirportID;
        private DateTime flightDay;
        private TimeSpan flightTime;
        private decimal price;
        private int isDeleted;

        public string FlightID
        {
            get => flightID;
            set => flightID = value;
        }

        public string SourceAirportID
        {
            get => sourceAirportID;
            set => sourceAirportID = value;
        }

        public string DestinationAirportID
        {
            get => destinationAirportID;
            set => destinationAirportID = value;
        }

        public DateTime FlightDay
        {
            get => flightDay;
            set => flightDay = value;
        }

        public TimeSpan FlightTime
        {
            get => flightTime;
            set => flightTime = value;
        }

        public decimal Price
        {
            get => price;
            set => price = value;
        }

        public int IsDeleted
        {
            get => isDeleted;
            set => isDeleted = value;
        }
    }
}
