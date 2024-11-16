using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AirportDTO
    {
        private string airportID;
        private string airportName;
        private int isDeleted;

        public string AirportID
        {
            get => airportID;
            set => airportID = value;
        }

        public string AirportName
        {
            get => airportName;
            set => airportName = value;
        }
        public int IsDeleted
        {
               get => isDeleted;    
               set => isDeleted = value;
        }
    }

}
