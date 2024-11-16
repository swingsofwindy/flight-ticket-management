using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FlightInforDTO
    {   
        // Thông tin cần thiết cho chức năng tìm thông tin chuyến bay
        public FlightDTO Flight {  get; set; } // Thông tin cơ bản của chuyến bay
        public int bookedTickets { get; set; } // Số vẽ đã bán/đặt
        public int emptySeats { get; set; }  // Tổng số ghế còn lại của chuyến bay
        public List<IntermediateAirportDTO> IntermediateAirports { get; set;} // Thông tin về các sân bay trung gian
        public List<TicketClassFlightDTO> TicketClasses { get; set; } // Thông tin về các hạng vé của chuyến bay

        // Thêm các thuộc tính nếu cần thiết ...
    }
}
