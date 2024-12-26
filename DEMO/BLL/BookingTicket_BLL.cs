using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
namespace BLL
{
    public class BookingTicket_BLL
    {
        public DateTime GetBookingTicket_DepartureTime(string TicketID)
        {
            return new BookingTicketAccess().GetBookingTicket_DepartureTime(TicketID);
        }
        public static string UpdateStatus()
        {
            BLL.SearchProcessor prc = new BLL.SearchProcessor();
            DAL.BookingTicketAccess updateprc = new DAL.BookingTicketAccess();
            var listTicket = prc.GetBookingTicket(string.Empty, string.Empty, string.Empty, 1);
            foreach (var item in listTicket)
            {
                BLL.BookingTicket_BLL checkprc = new BookingTicket_BLL();
                DateTime dpt = checkprc.GetBookingTicket_DepartureTime(item.TicketID);
                if (DateTime.Now >= dpt)
                {
                    updateprc.UpdateStatus(item.TicketID);
                    if (updateprc.GetState() != string.Empty)
                    {
                        return updateprc.GetState();
                    }
                }
            }
            return string.Empty;
        }
    }
}