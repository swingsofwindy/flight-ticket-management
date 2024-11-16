using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SellingTicketAcess
    {
        public SellingTicketAcess() { }
        public int getTicketSales_byFlightID(string flightID)
        {
            int number = 0;
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            string query = @"SELECT COUNT(FlightID) AS SoldTickets
                            FROM BOOKING_TICKET
                            WHERE (@flightID IS NULL OR FlightID = @flightID)
                            AND isDeleted = 0";

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Thiết lập các tham số
                command.Parameters.AddWithValue("@flightID", flightID ?? (object)DBNull.Value);

                // Đọc kết quả truy vấn
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        number = (int)reader[0];
                    }
                }
            }
            // Đóng kết nối
            con.Close();

            return number;
        }

        public int getTicketSales_byFlightID_TicketClassID(string flightID, string ticketClassID)
        {
            int number = 0;
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            string query = @"SELECT COUNT(FlightID) AS SoldTickets
                            FROM BOOKING_TICKET
                            WHERE (@flightID IS NULL OR FlightID = @flightID)
                            AND(@ticketClassID IS NULL OR TicketClassID = @ticketClassID)
                            AND isDeleted = 0";

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Thiết lập các tham số
                command.Parameters.AddWithValue("@flightID", flightID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ticketClassID", ticketClassID ?? (object)DBNull.Value);

                // Đọc kết quả truy vấn
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        number = (int)reader["SoldTickets"];
                    }
                }
            }
            // Đóng kết nối
            con.Close();

            return number;
        }
    }
}
