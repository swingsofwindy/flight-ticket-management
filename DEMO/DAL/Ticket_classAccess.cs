using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DTO;

namespace DAL
{
    public class Ticket_classAccess : DatabaseAccess
    {
        string state = string.Empty;
        public string AutoID()
        {
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from TICKET_CLASS", con);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            i++;
            return i.ToString("000");
        }
        public List<TicketClassDTO> L_TicketClass()
        {
            List<TicketClassDTO> ticketclass = new List<TicketClassDTO>();
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            string query = @"SELECT TicketClassID, TicketClassName, BaseMultiplier
                             FROM TICKET_CLASS
                             WHERE isDeleted = 0";

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Thiết lập các tham số

                // Đọc kết quả truy vấn
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ticketclass.Add(new TicketClassDTO()
                        {
                            TicketClassID = (string)reader["TicketClassID"],
                            TicketClassName = (string)reader["TicketClassName"],
                            BaseMultiplier = (decimal)(double)reader["BaseMultiplier"]
                        });
                    }
                }
            }
            // Đóng kết nối
            con.Close();

            return ticketclass;
        }
        public int getTotalSeat_byFlightID(string flightID)
        {
            int number = 0;
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            string query = @"SELECT ISNULL(SUM(Quantity), 0) AS TotalSeat
                            FROM TICKETCLASS_FLIGHT
                            WHERE (@flightID IS NULL OR FlightID = @flightID)
                            AND isDeleted = 0";

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Thiết lập các tham số
                command.Parameters.AddWithValue("@flightID", flightID ?? (object)DBNull.Value);

                // Đọc kết quả truy vấn
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        number = (int)reader["TotalSeat"];
                    }
                }
            }
            // Đóng kết nối
            con.Close();

            return number;
        }

        public int getTotalSeat_byFlightID_TicketClassID(string flightID, string ticketClassID)
        {
            int number = 0;
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            string query = @"SELECT ISNULL(SUM(Quantity), 0) AS TotalSeat
                            FROM TICKETCLASS_FLIGHT
                            WHERE (@flightID IS NULL OR FlightID = @flightID)
                            AND (@ticketClassID IS NULL OR TicketClassID = @ticketClassID)
                            AND isDeleted = 0";

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Thiết lập các tham số
                command.Parameters.AddWithValue("@flightID", flightID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ticketClassID", ticketClassID ?? (object)DBNull.Value);

                // Đọc kết quả truy vấn
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        number = (int)reader["TotalSeat"];
                    }
                }
            }
            // Đóng kết nối
            con.Close();

            return number;
        }

        public string Add_TicketClass(TicketClassDTO ticketClassDTO)
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
                        cmd.CommandText = "INSERT INTO TICKET_CLASS (TicketClassID, TicketClassName, BaseMultiplier, isDeleted) VALUES (@ICD, @TCNAME, @BASE, 0)";
                        cmd.Connection = con;
                        cmd.Transaction = transaction;

                        SqlParameter parID = new SqlParameter("@ICD", SqlDbType.VarChar)
                        {
                            Value = AutoID()
                        };
                        SqlParameter parName = new SqlParameter("@TCNAME", SqlDbType.NVarChar)
                        {
                            Value = ticketClassDTO.TicketClassName
                        };
                        SqlParameter parBase = new SqlParameter("@BASE", SqlDbType.Float)
                        {
                            Value = ticketClassDTO.BaseMultiplier
                        };
                        cmd.Parameters.Add(parID);
                        cmd.Parameters.Add(parName);
                        cmd.Parameters.Add(parBase);

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
        public bool isUsed(string id)
        {
            int number = 0;
            SqlConnection con = SqlConnectionData.Connect();
            con.Open();
            string query = @"select COUNT(*) as SL
                            from TICKET_CLASS tc, TICKETCLASS_FLIGHT tcf
                            WHERE tc.TicketClassID = tcf.TicketClassID
                            AND tc.TicketClassID = @TicketClassID 
                            AND tc.isDeleted = 0 AND tcf.isDeleted = 0";

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Thiết lập các tham số
                command.Parameters.AddWithValue("@TicketClassID", id);

                // Đọc kết quả truy vấn
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        number = (int)reader["SL"];
                    }
                }
            }
            // Đóng kết nối
            con.Close();
            return number > 0;
        }
        public int DeleteTicketClass(string ID)
        {

            if (isUsed(ID))
            {
                return 0;
            }
            

            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update TICKET_CLASS
                            set isDeleted = 1
                            where isDeleted = 0
                            AND TicketClassID = @ID";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                state = $"Error: {ex.Message}";
            }
            con.Close();
            return rowsAffected;
        }
        public string GetState()
        {
            return state;
        }
    }
}
