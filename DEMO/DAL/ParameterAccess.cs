using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ParameterAccess : DatabaseAccess
    {
        string state = string.Empty;
        public ParameterDTO GetParameters()
        {
            ParameterDTO parameter = new ParameterDTO();

            SqlConnection con = SqlConnectionData.Connect();
            try
            {
                con.Open();
                string query = @"SELECT AirportCount, MinFlightTime, IntermediateAirportCount, MinStopTime, MaxStopTime, TicketClassCount, SlowestBookingTime, CancelTime
                             FROM PARAMETER
                             where isDeleted = 0";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Đọc kết quả truy vấn
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            parameter = new ParameterDTO()
                            {
                                AirportCount = Convert.ToInt32(reader["AirportCount"]),
                                MinFlighTime = (TimeSpan)reader["MinFlightTime"],
                                IntermediateAirportCount = Convert.ToInt32(reader["IntermediateAirportCount"]),
                                MinStopTime = (TimeSpan)reader["MinStopTime"],
                                MaxStopTime = (TimeSpan)reader["MaxStopTime"],
                                TicketClassCount = Convert.ToInt32(reader["TicketClassCount"]),
                                SlowestBookingTime = (TimeSpan)reader["SlowestBookingTime"],
                                CancelTime = (TimeSpan)reader["CancelTime"]
                            };
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
            return parameter;
        }
        public int DeleteParamater()
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                            set isDeleted = 1
                            where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
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
        public int UpdateAirportCount(int number)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set AirportCount = @number
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@number", number);
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
        public int UpdateMinFligthTime(TimeSpan timeSpan)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set MinFlightTime = @time
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@time", timeSpan);
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
        public int UpdateIntermediateAirportCount(int number)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set IntermediateAirportCount = @number
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@number", number);
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
        public int UpdateMinStopTime(TimeSpan time)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set MinStopTime = @time
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@time", time);
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
        public int UpdateMaxStopTime(TimeSpan time)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set MaxStopTime = @time
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@time", time);
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
        public int UpdateSlowestBookingTime(TimeSpan time)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set SlowestBookingTime = @time
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@time", time);
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
        public int UpdateCancelTime(TimeSpan time)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set CancelTime = @time
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@time", time);
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
        public int UpdateTicketClassCount(int number)
        {
            SqlConnection con = SqlConnectionData.Connect();
            int rowsAffected = 0;
            this.state = string.Empty;
            try
            {
                con.Open();
                string query = @"update PARAMETER
                    set TicketClassCount = @number
                    where isDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@number", number);
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
        public string getState()
        {
            return state;
        }
    }
}
