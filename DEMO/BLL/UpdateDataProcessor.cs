using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UpdateDataProcessor
    {
        public UpdateDataProcessor() { }
        public int UpdateAirportCount(int number)
        {
            return new DAL.ParameterAccess().UpdateAirportCount(number);
        }
        public int UpdateMinFligthTime(TimeSpan timeSpan)
        {
            return new DAL.ParameterAccess().UpdateMinFligthTime(timeSpan);
        }
        public int UpdateIntermediateAirportCount(int number)
        {
            return new DAL.ParameterAccess().UpdateIntermediateAirportCount(number);
        }
        public int UpdateMinStopTime(TimeSpan time)
        {
            return new DAL.ParameterAccess().UpdateMinStopTime(time);
        }
        public int UpdateMaxStopTime(TimeSpan time)
        {
            return new DAL.ParameterAccess().UpdateMaxStopTime(time);
        }
        public int UpdateSlowestBookingTime(TimeSpan time)
        {
            return new DAL.ParameterAccess().UpdateSlowestBookingTime(time);
        }
        public int UpdateCancelTime(TimeSpan time)
        {
            return new DAL.ParameterAccess().UpdateCancelTime(time);
        }
        public int UpdateTicketClassCount(int number)
        {
            return new DAL.ParameterAccess().UpdateTicketClassCount(number);
        }
        public string getState()
        {
            return new DAL.ParameterAccess().getState();
        }
    }
}
