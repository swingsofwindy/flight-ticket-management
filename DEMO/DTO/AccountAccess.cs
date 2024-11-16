using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO;
using System.Security.Principal;

namespace DAL
{
    public class AccountAccess : DatabaseAccess
    {
        public string CheckLogic(ACCOUNT acc)
        {
            string info = CheckLogicDTO(acc);
            return info;
        }
    }
}
