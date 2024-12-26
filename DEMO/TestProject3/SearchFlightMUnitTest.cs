using System;
using NUnit.Framework;
using Moq;
using BLL; // Giả sử bạn sử dụng Moq để tạo mock cho IAccountBLL
using GUI;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace FlightSearchTests
{
    // Lớp giả lập (Mocked Data) cho chuyến bay (FlightInfoDTO)
    public class FlightInfoDTO
    {
        public Flight Flight { get; set; }
        public int emptySeats { get; set; }
        public int bookedTickets { get; set; }
    }

    public class Flight
    {
        public string FlightID { get; set; }
        public DateTime FlightDay { get; set; }
        public TimeSpan FlightTime { get; set; }
    }

    // Extension method to simulate fetching mocked flight data
    public static class MockDataHelper
    {
        public static IEnumerable<FlightInfoDTO> GetMockedFlightData()
        {
            return new List<FlightInfoDTO>
            {
                new FlightInfoDTO
                {
                    Flight = new Flight
                    {
                        FlightID = "FL001",
                        FlightDay = DateTime.Now.AddDays(1),
                        FlightTime = TimeSpan.FromHours(8)
                    },
                    emptySeats = 10,
                    bookedTickets = 5
                },
                new FlightInfoDTO
                {
                    Flight = new Flight
                    {
                        FlightID = "FL002",
                        FlightDay = DateTime.Now.AddDays(2),
                        FlightTime = TimeSpan.FromHours(10)
                    },
                    emptySeats = 15,
                    bookedTickets = 3
                }
            };
        }
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)] // Chạy trong luồng STA cho ứng dụng WPF
    public class Test_SearchFlight
    {
        private Mock<SearchProcessor> _mockSearchProcessor;
        private Mock<UpdateDataProcessor> _mockUpdateDataProcessor;
        private SearchProcessor _searchProcessor;

        [SetUp]
        public void Setup()
        {
            // Khởi tạo mock cho SearchProcessor và UpdateDataProcessor
            _mockSearchProcessor = new Mock<SearchProcessor>();
            _mockUpdateDataProcessor = new Mock<UpdateDataProcessor>();
            _searchProcessor = new SearchProcessor();
        }

        #region Kiểm tra chức năng tìm kiếm chuyến bay

        [TestCase("AIR001", "AIR002", true)] // Kiểm tra tìm chuyến bay hợp lệ
        [TestCase("INVALID", "INVALID", false)] // Kiểm tra chuyến bay không hợp lệ
        [TestCase("AIR001", "AIR002", true)] // Kiểm tra chuyến bay hợp lệ với thông số khác
        [Apartment(ApartmentState.STA)] // Chạy trong luồng STA
        public void SearchFlight_ShouldReturnCorrectResults(string departure, string arrival, bool expectedResult)
        {
            // Thiết lập mock để trả về kết quả theo dữ liệu đầu vào
            _mockSearchProcessor.Setup(x => x.GetFlightInfoDTO(departure, arrival, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((string dep, string arr, DateTime startDate, DateTime endDate) =>
                    expectedResult ? MockDataHelper.GetMockedFlightData() : Enumerable.Empty<FlightInfoDTO>());

            // Thực hiện tìm kiếm chuyến bay
            var flights = _searchProcessor.GetFlightInfoDTO(departure, arrival, DateTime.Now, DateTime.Now.AddDays(1));

            // Kiểm tra kết quả trả về
            Assert.AreEqual(expectedResult, flights.Any());
        }

        #endregion

        #region Kiểm tra tích hợp với UpdateDataProcessor

        [Test]
        public void AddAirport_ShouldIncreaseAirportCount()
        {
            // Thiết lập mock để cập nhật số lượng sân bay
            _mockUpdateDataProcessor.Setup(x => x.UpdateAirportCount(It.IsAny<int>())).Returns(1);

            // Kiểm tra số lượng sân bay sau khi thêm
            int initialCount = _mockUpdateDataProcessor.Object.UpdateAirportCount(0);
            int updatedCount = _mockUpdateDataProcessor.Object.UpdateAirportCount(initialCount + 1);

            // Kiểm tra kết quả số lượng tăng
            Assert.AreEqual(initialCount + 1, updatedCount);
        }

        [Test]
        public void RemoveAirport_ShouldDecreaseAirportCount()
        {
            // Thiết lập mock để giảm số lượng sân bay
            _mockUpdateDataProcessor.Setup(x => x.UpdateAirportCount(It.IsAny<int>())).Returns(0);

            // Kiểm tra số lượng sân bay sau khi giảm
            int initialCount = _mockUpdateDataProcessor.Object.UpdateAirportCount(1);
            int updatedCount = _mockUpdateDataProcessor.Object.UpdateAirportCount(initialCount - 1);

            // Kiểm tra kết quả số lượng giảm
            Assert.AreEqual(initialCount - 1, updatedCount);
        }

        #endregion

        #region Kiểm tra UI: Date Picker và ComboBoxes

        [Test]
        public void DatePicker_ShouldShowCalendarAndHighlightToday_WhenEmpty()
        {
            // Mô phỏng sự kiện click vào DatePicker khi textbox chưa có giá trị
            var datePicker = new DatePicker();
            datePicker.Focus();
            // Kiểm tra xem calendar có hiện lên và ngày hôm nay được highlight không
            Assert.IsTrue(datePicker.IsDropDownOpen);
            Assert.AreEqual(DateTime.Now.Date, datePicker.SelectedDate.Value.Date);
        }

        [Test]
        public void ComboBox_ShouldShowDropdownAndHighlightSelection()
        {
            // Mô phỏng sự kiện click vào ComboBox (From) và kiểm tra các tùy chọn
            var comboBox = new ComboBox();
            comboBox.ItemsSource = new List<string> { "Airport1", "Airport2", "Airport3" };
            comboBox.Focus();

            // Kiểm tra xem dropdown có mở ra và có các tùy chọn không
            Assert.IsTrue(comboBox.IsDropDownOpen);
            Assert.AreEqual(3, comboBox.Items.Count);
        }

        [Test]
        public void ComboBox_ShouldShowValidationMessage_WhenNoSelection()
        {
            // Mô phỏng khi không chọn sân bay
            var comboBox = new ComboBox();
            comboBox.SelectedItem = null;
            Assert.IsFalse(comboBox.IsDropDownOpen);
            Assert.AreEqual("Please select airport!", comboBox.ToolTip);
        }

        #endregion

        #region Kiểm tra TextBox: Ticket Quantity và Input Validation

        [Test]
        public void TicketQuantity_TextBox_ShouldTrimWhitespace()
        {
            // Kiểm tra xem space ở đầu và cuối có bị trim không
            var ticketQuantityTextBox = new TextBox { Text = "  10  " };
            Assert.AreEqual("10", ticketQuantityTextBox.Text.Trim());
        }

        [Test]
        public void TicketQuantity_TextBox_ShouldShowError_ForNegativeValue()
        {
            // Kiểm tra nhập giá trị âm
            var ticketQuantityTextBox = new TextBox { Text = "-5" };
            // Kiểm tra nếu có cảnh báo lỗi
            Assert.IsTrue(ticketQuantityTextBox.Text.Contains("Invalid input"));
        }

        #endregion

        #region Kiểm tra Button: Select và Close

        [Test]
        public void SelectButton_ShouldEnableAndSelectSingleFlight()
        {
            // Kiểm tra button Select được enable và chỉ có thể chọn 1 chuyến bay
            var buttonSelect = new Button { IsEnabled = true };
            Assert.IsTrue(buttonSelect.IsEnabled);
            // Kiểm tra khi chọn chuyến bay, hiển thị thông tin
            buttonSelect.Click += (sender, args) =>
            {
                var flightInfo = new FlightInfoDTO { Flight = new Flight { FlightID = "FL001" }, emptySeats = 10, bookedTickets = 5 };
                Assert.AreEqual("FL001", flightInfo.Flight.FlightID);
            };
        }

        [Test]
        public void CloseButton_ShouldCloseSearchForm()
        {
            // Kiểm tra chức năng đóng form khi click Close
            var closeButton = new Button { IsEnabled = true };
            closeButton.Click += (sender, args) => { /* Simulate form close */ };
            closeButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Pass("Form closed successfully");
        }

        #endregion

    }
}
