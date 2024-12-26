using DTO;
using GUI.View;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using static GUI.View.Window3;
using System.Windows.Controls;
using NUnit.Framework;

namespace TestProject3
{
    [TestFixture]
    public class SellTicketNUnitTest
    {
        private Window6 sellTicketWindow;
        private Window6 deleteItemWindow;

        [SetUp]
        public void Setup()
        {
            sellTicketWindow = new Window6();
        }

        #region Test Case
        // Textbox "ID"
        [TestCase("", "", "", "", 2002, 05, 11, false)] // ID bỏ trống
        [TestCase("John Doe", "1q2qw", "0123456789", "john@gmail.com", 2002, 05, 11, false)] // ID chứa ký tự không hợp lệ
        [TestCase("John Doe", "123", "0123456789", "john@gmail.com", 2002, 05, 11, false)] // ID quá ngắn
        [TestCase("John Doe", "12214254365473441", "0123456789", "john@gmail.com", 2002, 05, 11, false)] // ID quá dài

        // Textbox "Name"
        [TestCase("", "123456789012", "0123456789", "john@gmail.com", 2002, 05, 11, false)] // Name bỏ trống
        [TestCase("Nguyễn Ngọc @---+", "123456789012", "0123456789", "john@gmail.com", 2002, 05, 11, true)] // Name chứa ký tự đặc biệt
        [TestCase("N", "123456789012", "0123456789", "john@gmail.com", 2002, 05, 11, true)] // Name hợp lệ, 1 ký tự
        [TestCase("Nguyễn Ngọc AB... (39)", "123456789012", "0123456789", "john@gmail.com", 2002, 05, 11, true)] // Name hợp lệ, 39 ký tự
        [TestCase("Nguyễn Ngọc AB... (40)", "123456789012", "0123456789", "john@gmail.com", 2002, 05, 11, true)] // Name hợp lệ, 40 ký tự
        [TestCase(" Nguyễn Ngọc AB ", "123456789012", "0123456789", "john@gmail.com", 2002, 05, 11, true)] // Name hợp lệ, có khoảng trắng ở đầu và cuối chuỗi

        // Textbox "Phone"
        [TestCase("John Doe", "123456789012", "", "john@gmail.com", 2002, 05, 11, false)] // Phone bỏ trống
        [TestCase("John Doe", "123456789012", "01234", "john@gmail.com", 2002, 05, 11, false)] // Phone quá ngắn
        [TestCase("John Doe", "123456789012", "qư121w", "john@gmail.com", 2002, 05, 11, false)] // Phone chứa ký tự không hợp lệ
        [TestCase("John Doe", "123456789012", "0122141807534141", "john@gmail.com", 2002, 05, 11, false)] // Phone quá dài
        [TestCase("John Doe", "123456789012", "123143893", "john@gmail.com", 2002, 05, 11, false)] // Phone không hợp lệ


        // Textbox "Email"
        [TestCase("John Doe", "123456789012", "0123456789", "", 2002, 05, 11, false)] // Email bỏ trống
        [TestCase("John Doe", "123456789012", "0123456789", "nguyenngoc123456789023456789....abcd@gmail.com", 2002, 05, 11, false)] // Email quá dài (61 ký tự)
        [TestCase("John Doe", "123456789012", "0123456789", "nguyenngoc@vmail.com", 2002, 05, 11, false)] // Email không hợp lệ
        [TestCase("John Doe", "123456789012", "0123456789", "n@gmail.com", 2002, 05, 11, true)] // Email hợp lệ, ngắn gọn
        [TestCase("John Doe", "123456789012", "0123456789", "niiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii@gmail.com", 2002, 05, 11, true)] // Email hợp lệ, 59 ký tự tên
        [TestCase("John Doe", "123456789012", "0123456789", "niiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii@gmail.com", 2002, 05, 11, true)] // Email hợp lệ, 60 ký tự tên
        [TestCase("John Doe", "123456789012", "0123456789", "nguyenngoc@gmail.com", 2002, 05, 11, true)] // Email hợp lệ

        // Date picker "Birth"
        [TestCase("John Doe", "123456789012", "0123456789", "john@gmail.com", 2022, 12, 20, true)] // Birth hợp lệ (20/12/2022)
        #endregion

        [Test, Apartment(ApartmentState.STA)]
        public void TestConfirm_Click(string name, string id, string phone, string email, int year, int month, int day, bool expectedResult)
        {
            // Arrange: Giả lập dữ liệu khách hàng hợp lệ
            var eventArgs = new RoutedEventArgs();
            sellTicketWindow.selectedFlight = new FlightDTO
            {
                FlightID = "FL002",
                SourceAirportID = "001",
                DestinationAirportID = "002",
                FlightDay = new DateTime(2024, 6, 2),
                FlightTime = TimeSpan.Parse("09:00:00"),
                Price = 150.00m,
                IsDeleted = 0
            };

            sellTicketWindow.selectedTicketClass = new TicketClassDTO
            {
                TicketClassID = "002",
                TicketClassName = "Business",
                BaseMultiplier = 1.5m,
                IsDeleted = 0
            };

            // Tạo danh sách khách hàng
            var customerList = new ObservableCollection<CustomerDTO>
            {
                new CustomerDTO
                {
                    CustomerName = name,
                    ID = id,
                    Phone = phone,
                    Email = email,
                    Birth = new DateTime(year, month, day)
                }
            };

            // Giả lập khách hàng
            sellTicketWindow.ViewCustomerData = customerList;
            sellTicketWindow.customerView = CollectionViewSource.GetDefaultView(sellTicketWindow.ViewCustomerData);
            sellTicketWindow.myListView.ItemsSource = sellTicketWindow.customerView;

            // Act: Gọi Confirm_Click
            sellTicketWindow.Confirm_Click(this, eventArgs);

            // Log kết quả để debug
            Console.WriteLine($"Name: {name}, ID: {id}, Phone: {phone}, Email: {email}, Birth: {new DateTime(year, month, day)}");
            Console.WriteLine($"Expected: {expectedResult}, Actual: {sellTicketWindow.IsSuccess}");

            // Assert: Kiểm tra kết quả
            Assert.That(expectedResult, Is.EqualTo(sellTicketWindow.IsSuccess));
        }


        /*--------------------------test delete item------------------------------------*/
        [SetUp]
        public void SetUp()
        {
            if (Application.Current == null)
            {
                new Application(); // Tạo Application nếu chưa có.
            }
            // Tải ResourceDictionary chính
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/GUI;component/Styles/Page.xaml", UriKind.Absolute)
            };
            // Đảm bảo tất cả ResourceDictionary được tải
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            deleteItemWindow = new Window6();
        }

        #region Test Case
        [TestCase("123456789012111", false)]  // Non-existing customer
        //[TestCase("123456789013", true)]      // Existing customer
        #endregion
        [Test, Apartment(ApartmentState.STA)]
        public void TestDeleteMember(string userId, bool expectedResult)
        {
            // Find the customer to delete
            var accountToDelete = deleteItemWindow.ViewCustomerData.FirstOrDefault(acc => acc.ID == userId);

            // Log for debugging to see if the account was found
            Console.WriteLine($"Found customer: {accountToDelete?.CustomerName ?? "None"}");

            // Act: Delete the customer if found
            if (accountToDelete != null)
            {
                deleteItemWindow.DeleteItem(accountToDelete);
            }

            // Assert: Check if deletion was successful based on the expected result
            Console.WriteLine($"Expected: {expectedResult}, Actual: {deleteItemWindow.IsSuccess1}");
            Assert.That(deleteItemWindow.IsSuccess1, Is.EqualTo(expectedResult));
        }
    }
}
