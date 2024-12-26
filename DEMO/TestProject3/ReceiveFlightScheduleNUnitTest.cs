using NUnit.Framework;
using System;

namespace FlightScheduleTests
{
    [TestFixture]
    public class ReceiveFlightScheduleTests
    {
        #region Combined Test Case
        [TestCase("", "1", "01/01/2024", "10:30", "100", ExpectedResult = false)] // Invalid airport selection
        [TestCase("Sân bay Tân Sơn Nhất", "", "01/01/2024", "10:30", "100", ExpectedResult = false)] // Invalid airport ID
        [TestCase("Sân bay Tân Sơn Nhất", "1", "31/02/2024", "10:30", "100", ExpectedResult = false)] // Invalid date
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "25:00", "100", ExpectedResult = false)] // Invalid time
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "10:30", "-100", ExpectedResult = false)] // Invalid price
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "10:30", "100", ExpectedResult = true)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "10:30", "100aq", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "10:30", "100.12", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "10:30", "", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "100:30", "100", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "10a:30", "100", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01/01/2024", "", "100", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "01a/01/2024", "10:30", "100", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "-01/01/2024", "10:30", "100", ExpectedResult = false)]
        [TestCase("Sân bay Tân Sơn Nhất", "1", "", "10:30", "100", ExpectedResult = false)]
        public bool TestCompleteFlightSchedule(string selectedAirport, string selectedID, string date, string time, string price)
        {
            // Validate Departure Airport
            if (!ValidateAirportSelection(selectedAirport)) return false;

            // Validate Departure Airport ID
            if (!ValidateAirportID(selectedID)) return false;

            // Validate Departure Day
            if (!ValidateDateFormat(date)) return false;

            // Validate Departure Time
            if (!ValidateTimeFormat(time)) return false;

            // Validate Ticket Price
            if (!ValidateTicketPrice(price)) return false;

            return true; // All validations passed
        }
        #endregion

        #region Helper Methods
        private bool ValidateAirportSelection(string airport)
        {
            return !string.IsNullOrEmpty(airport) && airport == "Sân bay Tân Sơn Nhất";
        }

        private bool ValidateAirportID(string id)
        {
            return id == "1";
        }

        private bool ValidateDateFormat(string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }

        private bool ValidateTimeFormat(string time)
        {
            TimeSpan ts;
            return TimeSpan.TryParse(time, out ts);
        }

        private bool ValidateTicketPrice(string price)
        {
            if (int.TryParse(price, out int parsedPrice))
            {
                return parsedPrice >= 0;
            }
            return false;
        }
        #endregion

        /************************************/
        #region Combined Test Case for Ticket Class and Intermediate Airport
        // Test cases for Ticket Class
        [TestCase("", "", "", "", ExpectedResult = false)] // All fields empty
        [TestCase("1", "Economy", "-1", "10", ExpectedResult = false)] // Invalid Quantity (negative number)
        [TestCase("1", "Economy", "10", "abc", ExpectedResult = false)] // Invalid Quantity (non-numeric)
        [TestCase("1", "Economy", "10", "10", ExpectedResult = true)] // Valid data
        [TestCase("1", "Economy", "10", "0", ExpectedResult = true)]
        [TestCase("1", "Economy", "10.17", "abc", ExpectedResult = false)]
        [TestCase("1", "Economy", "10", "-10", ExpectedResult = false)]
        [TestCase("1", "Economy", "10", "10.123", ExpectedResult = false)]
        [TestCase("1", "Economy", "12sqs", "abc", ExpectedResult = false)]
        public bool TestTicketClassValidation(string idTicketClass, string ticketClassName, string multiplier, string quantity)
        {
            // Validate Ticket Class ID
            if (!ValidateTicketClassID(idTicketClass)) return false;

            // Validate Ticket Class Name
            if (!ValidateTicketClassName(ticketClassName)) return false;

            // Validate Multiplier
            if (!ValidateMultiplier(multiplier)) return false;

            // Validate Quantity
            if (!ValidateQuantity(quantity)) return false;

            return true; // All validations passed
        }

        // Test cases for Intermediate Airport
        [TestCase("", "", "", "", ExpectedResult = false)] // All fields empty
        [TestCase("1", "Airport A", "-1", "Valid Note", ExpectedResult = false)] // Invalid Layover Time (negative)
        [TestCase("1", "Airport A", "abc", "Valid Note", ExpectedResult = false)] // Invalid Layover Time (non-numeric)
        [TestCase("1", "Airport A", "2", "Valid Note", ExpectedResult = true)] // Valid data
        [TestCase("1", "Airport A", "0", "Valid Note", ExpectedResult = true)]
        [TestCase("1", "Airport A", "10.123", "", ExpectedResult = false)]
        [TestCase("1", "Airport A", "-1", "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii", ExpectedResult = false)]
        public bool TestIntermediateAirportValidation(string idIntermediateAirport, string intermediateAirportName, string layoverTime, string note)
        {
            // Validate Intermediate Airport ID
            if (!ValidateIntermediateAirportID(idIntermediateAirport)) return false;

            // Validate Intermediate Airport Name
            if (!ValidateIntermediateAirportName(intermediateAirportName)) return false;

            // Validate Layover Time
            if (!ValidateLayoverTime(layoverTime)) return false;

            // Validate Note
            if (!ValidateNoteField(note)) return false;

            return true; // All validations passed
        }
        #endregion

        #region Helper Methods for Ticket Class and Intermediate Airport
        private bool ValidateTicketClassID(string id)
        {
            return !string.IsNullOrEmpty(id) && id == "1"; // Assume ID "1" is valid
        }

        private bool ValidateTicketClassName(string name)
        {
            return !string.IsNullOrEmpty(name); // Assuming valid ticket class name cannot be empty
        }

        private bool ValidateMultiplier(string multiplier)
        {
            return int.TryParse(multiplier, out int result) && result >= 0; // Valid if positive integer
        }

        private bool ValidateQuantity(string quantity)
        {
            return int.TryParse(quantity, out int result) && result >= 0; // Valid if non-negative integer
        }

        private bool ValidateIntermediateAirportID(string id)
        {
            return !string.IsNullOrEmpty(id) && id == "1"; // Assume ID "1" is valid for intermediate airports
        }

        private bool ValidateIntermediateAirportName(string name)
        {
            return !string.IsNullOrEmpty(name); // Valid name should not be empty
        }

        private bool ValidateLayoverTime(string time)
        {
            return int.TryParse(time, out int result) && result >= 0; // Valid if positive integer or zero
        }

        private bool ValidateNoteField(string note)
        {
            return !string.IsNullOrEmpty(note); // Valid note should not be empty
        }
        #endregion
    }
}
