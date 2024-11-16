using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TicketClassDTO
    {
        private string ticketClassID;
        private string ticketClassName;
        private decimal baseMultiplier;
        private int isDeleted;
        public string TicketClassID
        {
            get => ticketClassID;
            set => ticketClassID = value;
        }

        public string TicketClassName
        {
            get => ticketClassName;
            set => ticketClassName = value;
        }

        public decimal BaseMultiplier
        {
            get => baseMultiplier;
            set => baseMultiplier = value;
        }
        
        public int IsDeleted
        {
            get => isDeleted;
            set => isDeleted = value;
        }
    }
}
