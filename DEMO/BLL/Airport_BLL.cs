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
    public class Airport_BLL
    {
        string state = string.Empty;
        public List<AirportDTO> L_airport()
        {
            /*AirportAccess airportAccess = new AirportAccess();
            return airportAccess.L_airport();*/
            return new DAL.AirportAccess().L_airport();
        }
        public bool isExist(string name)
        {
            return new DAL.AirportAccess().isExist(name);
        }
        public string insertAirport(string airportName)
        {
            return new DAL.AirportAccess().AddAirport(airportName);
        }
        public int deleteAirport(string airportID)
        {
            if (new DAL.AirportAccess().Get_cnt_Airport_Flight(airportID))
            {
                state = "This airport is in use";
                return 0;
            }
            else if (new DAL.AirportAccess().Get_cnt_Airport_IntermidiateAirport(airportID))
            {
                state = "This airport is in use";
                return 0;
            }
            return new DAL.AirportAccess().DeleteAirport(airportID);
        }
        
    }
}
