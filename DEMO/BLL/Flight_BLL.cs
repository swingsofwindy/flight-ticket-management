using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Flight_BLL
    {
        public string Get_ID()
        {
            return new DAL.FlightAccess().AutoID();
        }
        public int GetNumFlight(string SourID, string DesID, DateTime StartD, DateTime EndD)
        {
            return new DAL.FlightAccess().GetNumFlight(SourID, DesID, StartD, EndD);
        }
    }
}
