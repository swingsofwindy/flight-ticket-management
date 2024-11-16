using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class IntermediateAirportDTO
    {
        // Thông tin về sân bay trung gian ứng với chuyến bay
        private string flightID;
        private string airportID;
        private TimeSpan layoverTime;
        private string note;
        private int isDeleted;

        public string FlightID
        {
            get => flightID;
            set => flightID = value;
        }

        public string AirportID
        {
            get => airportID;
            set => airportID = value;
        }

        public TimeSpan LayoverTime
        {
            get => layoverTime;
            set => layoverTime = value;
        }

        public string Note
        {
            get => note;
            set => note = value;
        }
        public int IsDeleted
        {
            get => isDeleted;
            set => isDeleted = value;
        }

        public IntermediateAirportDTO(string flightID, string airportID, TimeSpan layoverTime, string note)
        {
            this.flightID = flightID;
            this.airportID = airportID;
            this.layoverTime = layoverTime;
            this.note = note;
        }
        public IntermediateAirportDTO()
        {
        }
    }
}