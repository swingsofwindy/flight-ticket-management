using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TicketClassFlightAccess
    {
        string state = string.Empty;
        public TicketClassFlightAccess() { }
        public string insertListTicketClass(List<TicketClassFlightDTO> listTicketClassFlight)
        {
            SqlConnection con = SqlConnectionData.Connect();

            con.Open();
            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    StringBuilder queryBuilder = new StringBuilder("INSERT INTO TICKETCLASS_FLIGHT (TicketClassID, FlightID, Quantity, Multiplier, isDeleted) VALUES ");
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    for (int i = 0; i < listTicketClassFlight.Count; i++)
                    {
                        TicketClassFlightDTO ticketClassFlight = listTicketClassFlight[i];

                        queryBuilder.Append($"(@TicketClassID_{i}, @FlightID_{i}, @Quantity_{i}, @Multiplier_{i}, 0),");

                        SqlParameter ticketClassIDParam = new SqlParameter($"@TicketClassID_{i}", System.Data.SqlDbType.VarChar);
                        ticketClassIDParam.Value = ticketClassFlight.TicketClassID;
                        parameters.Add(ticketClassIDParam);

                        SqlParameter flightIDParam = new SqlParameter($"@FlightID_{i}", System.Data.SqlDbType.VarChar);
                        flightIDParam.Value = ticketClassFlight.FlightID;
                        parameters.Add(flightIDParam);

                        SqlParameter quantityParam = new SqlParameter($"@Quantity_{i}", System.Data.SqlDbType.Int);
                        quantityParam.Value = ticketClassFlight.Quantity;
                        parameters.Add(quantityParam);

                        SqlParameter multiplierParam = new SqlParameter($"@Multiplier_{i}", System.Data.SqlDbType.Float);
                        multiplierParam.Value = ticketClassFlight.Multiplier;
                        parameters.Add(multiplierParam);
                    }

                    if (queryBuilder.Length > 0 && queryBuilder[queryBuilder.Length - 1] == ',')
                    {
                        queryBuilder.Remove(queryBuilder.Length - 1, 1);
                    }

                    using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), con, transaction))
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                        
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

        public string GetState()
        {
            return this.state;
        }
    }
}
