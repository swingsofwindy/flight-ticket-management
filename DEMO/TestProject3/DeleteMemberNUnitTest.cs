using System;
using NUnit.Framework;
using Moq;
using BLL; // Nếu bạn sử dụng Moq để tạo mock cho IAccountBLL
using GUI;
using System.Windows;
using GUI.View;
using DTO;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GUI.Test
{
    [TestFixture]
    public class Test_DeleteMember
    {
        private Window3 _deleteMemberWindow;
        //private ObservableCollection<ACCOUNT> _members;


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

            //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            //{
            //    Source = new Uri("pack://application:,,,/MahApps.Metro.IconPacks;component/Material.xaml", UriKind.Absolute)
            //});
            //// Tương tự, nạp thêm các tài nguyên khác
            //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            //{
            //    Source = new Uri("pack://application:,,,/MahApps.Metro.IconPacks;component/Material.xaml", UriKind.Absolute)
            //});

            _deleteMemberWindow = new Window3();
        }

        #region Test Case
        [TestCase("1", false)]
        [TestCase("2", true)]
        //[Apartment(ApartmentState.STA)]
        #endregion
        [Test, Apartment(ApartmentState.STA)]

        public void TestDeleteMember(string userId, bool expectedResult)
        {
            //Lấy thông tin tài khoản cần kiểm tra
            var accountToDelete = _deleteMemberWindow._members.FirstOrDefault(acc => acc.UserID == userId);

            //Mô phỏng việc gán DataContext cho nút xóa
            var deleteButton = new Button
            {
                DataContext = accountToDelete
            };

            // Gọi sự kiện Delete_Click
            _deleteMemberWindow.Delete_Click(deleteButton, null);

            Console.WriteLine($"Expected: {expectedResult}, Actual: {_deleteMemberWindow.IsSuccess}");

            // Xác nhận kết quả
            Assert.That(expectedResult, Is.EqualTo(_deleteMemberWindow.IsSuccess));

        }
    }
}

