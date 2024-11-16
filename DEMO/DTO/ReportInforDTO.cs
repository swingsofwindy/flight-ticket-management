using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ReportByFlightDTO
    {
        public string flightID {  get; set; } // Mã chuyến bay
        public int ticketsSold {  get; set; } // Số vé đã bán trong chuyến bay đó
        public decimal revenue {  get; set; } // Doanh thu từ việc bán vé
        public decimal ratio {  get; set; } // Tỉ lệ doanh thu ? của chuyến này trên tổng doanh thu ??
    }

    public class ReportByMonthDTO
    {
        public DateTime time { get; set; } // Tháng + Năm
        public int flightQuantity { get; set; } // Số chuyến bay khởi hành trong tháng
        public decimal revenue { get; set; }
        public decimal ratio { get; set; }
    }
}
