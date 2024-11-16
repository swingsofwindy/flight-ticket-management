using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DTO;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace DAL
{
    public class FlightAccess : DatabaseAccess
    {
        string state = string.Empty; // Chuỗi rỗng xem như thành công
        public string AutoID()
        {
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();

            SqlCommand cmd = new SqlCommand("select count(*) from FLIGHT", con);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            i++;
            return i.ToString("FL000");
        }
        // Cũng là Add_Flight nhưng có trả về trạng thái xử lý để dễ debug bằng cách huyền thoại
        public string Add_Flights(FlightDTO flight)
        {
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();

            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO FLIGHT (FlightID, SourceAirportID, DestinationAirportID, FlightDay, FlightTime, Price, isDeleted) VALUES (@ID, @SouID, @DesID, @FlDay, @FlTime, @Price, 0)";
                        cmd.Connection = con;
                        cmd.Transaction = transaction;

                        SqlParameter parID = new SqlParameter("@ID", SqlDbType.VarChar)
                        {
                            Value = AutoID()
                        };
                        SqlParameter parSouID = new SqlParameter("@SouID", SqlDbType.VarChar)
                        {
                            Value = flight.SourceAirportID
                        };
                        SqlParameter parDesID = new SqlParameter("@DesID", SqlDbType.VarChar)
                        {
                            Value = flight.DestinationAirportID
                        };
                        SqlParameter parFlDay = new SqlParameter("@FlDay", SqlDbType.SmallDateTime)
                        {
                            Value = flight.FlightDay
                        };
                        SqlParameter parFlTime = new SqlParameter("@FlTime", SqlDbType.Time)
                        {
                            Value = flight.FlightTime
                        };
                        SqlParameter parPrice = new SqlParameter("@Price", SqlDbType.Money)
                        {
                            Value = flight.Price
                        };

                        cmd.Parameters.Add(parID);
                        cmd.Parameters.Add(parSouID);
                        cmd.Parameters.Add(parDesID);
                        cmd.Parameters.Add(parFlDay);
                        cmd.Parameters.Add(parFlTime);
                        cmd.Parameters.Add(parPrice);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();
                            return "No rows were inserted.";
                        }
                    }
                    transaction.Commit();
                    return string.Empty; // Chuỗi rỗng xem như thành công
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Insert failed: {ex.Message}";
                }
            }
        }

        public List<FlightDTO> getFlight(string sourceAirportID, string destinationAirportID, DateTime startDate, DateTime endDate)
        {
            List<FlightDTO> data = new List<FlightDTO>();
            SqlConnection con = SqlConnectionData.Connect();
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"SELECT FlightID, SourceAirportID, DestinationAirportID, FlightDay, FlightTime, Price
                                FROM FLIGHT FL
                                WHERE (@sourceAirportID IS NULL OR FL.SourceAirportID = @sourceAirportID)
                                AND (@destinationAirportID IS NULL OR FL.DestinationAirportID = @destinationAirportID)
                                AND (FL.FlightDay BETWEEN @startDate AND @endDate)
                                AND (FL.isDeleted = 0)";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Thiết lập các tham số
                    //command.Parameters.AddWithValue("@sourceAirportID", sourceAirportID == "" ? (object)DBNull.Value : sourceAirportID);
                    //command.Parameters.AddWithValue("@destinationAirportID", destinationAirportID == "" ? (object)DBNull.Value : destinationAirportID);

                    command.Parameters.AddWithValue("@sourceAirportID", (sourceAirportID == "")? (object)DBNull.Value : sourceAirportID);
                    command.Parameters.AddWithValue("@destinationAirportID", (destinationAirportID == "")? (object)DBNull.Value : destinationAirportID);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    // Đọc kết quả truy vấn
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FlightDTO flight = new FlightDTO()
                            {
                                FlightID = reader["FlightID"].ToString(),
                                DestinationAirportID = reader["DestinationAirportID"].ToString(),
                                SourceAirportID = reader["SourceAirportID"].ToString(),
                                FlightDay = Convert.ToDateTime(reader["FlightDay"]),
                                FlightTime = (TimeSpan)reader["FlightTime"],
                                Price = Convert.ToDecimal(reader["Price"])

                            };
                            data.Add(flight);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                state = $"Error: {ex.Message}";
            }
            // Đóng kết nối
            con.Close();
            return data;
        }

        public List<FlightDTO> getFlight(string sourceAirportID, string destinationAirportID, DateTime startDate, DateTime endDate, string ticketClass, int numTicket)
        {
            List<FlightDTO> data = new List<FlightDTO>();
            SqlConnection con = SqlConnectionData.Connect();
            string state = string.Empty;
            try
            {
                con.Open();
                string query = @" 
                                SELECT f.FlightID, f.SourceAirportID, f.DestinationAirportID, f.FlightDay, f.FlightTime, f.Price
                                FROM FLIGHT f
                                INNER JOIN TICKETCLASS_FLIGHT tf ON f.FlightID = tf.FlightID and f.isDeleted = tf.isDeleted
                                LEFT JOIN (
                                    SELECT FlightID, TicketClassID, COUNT(*) AS BookedTickets, isDeleted
                                    FROM BOOKING_TICKET
                                    GROUP BY FlightID, TicketClassID, isDeleted
                                ) bt ON f.FlightID = bt.FlightID AND tf.TicketClassID = bt.TicketClassID and bt.isDeleted = f.isDeleted 
                                WHERE (f.isDeleted = 0)";

                if (sourceAirportID != "" || destinationAirportID != "" || ticketClass != null)
                {
                    query += @" AND (@sourceAirportID IS NULL OR f.SourceAirportID = @sourceAirportID)
                                AND (@destinationAirportID IS NULL OR f.DestinationAirportID = @destinationAirportID)
                                AND (f.FlightDay BETWEEN @startDate AND @endDate)
                                AND (@ticketClass IS NULL OR tf.TicketClassID = @ticketClass)
                                AND (tf.Quantity - ISNULL(bt.BookedTickets, 0)) >= @numTicket";
                }
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Thiết lập các tham số
                    command.Parameters.AddWithValue("@sourceAirportID", sourceAirportID == "" ? (object)DBNull.Value : sourceAirportID);
                    command.Parameters.AddWithValue("@destinationAirportID", destinationAirportID == "" ? (object)DBNull.Value : destinationAirportID);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@ticketClass", ticketClass == "" ? (object)DBNull.Value : ticketClass);
                    command.Parameters.AddWithValue("@numTicket", numTicket);

                    // Đọc kết quả truy vấn
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FlightDTO flight = new FlightDTO()
                            {
                                FlightID = reader["FlightID"].ToString(),
                                SourceAirportID = reader["SourceAirportID"].ToString(),
                                DestinationAirportID = reader["DestinationAirportID"].ToString(),
                                FlightDay = Convert.ToDateTime(reader["FlightDay"]),
                                FlightTime = (TimeSpan)reader["FlightTime"],
                                Price = Convert.ToDecimal(reader["Price"])
                            };
                            data.Add(flight);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                state = $"Error: {ex.Message}";
            }
            // Đóng kết nối
            con.Close();
            return data;
        }
        public int GetNumFlight(string SourID, string DesID, DateTime StartD, DateTime EndD)
        {
            SqlConnection con = SqlConnectionData.Connect();
            string state = string.Empty;
            int count = 0;
            try
            {
                con.Open();
                string query = @"select count(*)from FLIGHT
                                WHERE (@SourID IS NULL OR @SourID = SourceAirportID)
                                AND (@DesID is NULL OR @DesID = DestinationAirportID)
                                AND FlightDay BETWEEN @StartD and @EndD
                                AND isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@SourID", SourID == "" ? (object)DBNull.Value : SourID);
                    command.Parameters.AddWithValue("@DesID", DesID == "" ? (object)DBNull.Value : DesID);
                    command.Parameters.AddWithValue("@startDate", StartD);
                    command.Parameters.AddWithValue("@endDate", EndD);

                    count = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                state = $"Error: {ex.Message}";
            }
            con.Close();
            return count;
        }
        public string GetState()
        {
            return this.state;
        }
    }
}
