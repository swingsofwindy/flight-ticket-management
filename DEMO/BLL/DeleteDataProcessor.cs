using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DeleteDataProcessor
    {
        string state = string.Empty;
        public DeleteDataProcessor() { }
        public int DeleteIntermidiateAirport(string AirportID)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = new DAL.IntermidateAirportAccess().DeleteIntermidiateAirport(AirportID);
            }
            catch (Exception ex)
            {
                state = $"Error: {ex.Message}";
            }
            return rowsAffected;
        }

        public int DeleteTicket(string ticketID)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = new DAL.BookingTicketAccess().DeleteTicket(ticketID);
            }
            catch (Exception ex)
            {
                state = $"Error: {ex.Message}";
            }
            return rowsAffected;
        }
        public string getState()
        { 
            return state;
        }
    }
}
