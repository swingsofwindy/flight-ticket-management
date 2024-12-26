
using System.Windows.Controls;

namespace GUI.Test
{
    [TestFixture]
    public class Test_AddMember
    {
        private SignUp1 _addMemberWindow;

        [SetUp]
        public void SetUp()
        {
            _addMemberWindow = new SignUp1();

            // Khởi tạo ItemsSource
            _addMemberWindow.Day = new ComboBox { ItemsSource = Enumerable.Range(1, 31).ToList() };
            _addMemberWindow.Month = new ComboBox { ItemsSource = Enumerable.Range(1, 12).ToList() };
            _addMemberWindow.Year = new ComboBox { ItemsSource = Enumerable.Range(1900, 2000).ToList() };

        }

        #region Test Case
        // Test first name
        [TestCase("A@", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("Tháiiiiiiiiiiiiiiiiiiiiiiiiii Annnnnnnnnn", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, false)]

        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("An", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("Tháiiiiiiiiiiiiiiiiiiiiiiiii Annnnnnnnn", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("Tháiiiiiiiiiiiiiiiiiiiiiiiiii Annnnnnnnn", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("     An       ", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]

        // Test last name
        [TestCase("A","A@", "h", "0323456789", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "", "h", "0323456789", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "Nguyễnnnn Vănnnnnnnnnn Hoàngggg Tháiiiiii ", "h", "0323456789", "false", "N", "N", 28, 1, 2003, false)]

        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "Aa", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "Nguyễnnn Vănnnnnnnnn Hoàngggg Tháiiiiii", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "Nguyễnnnn Vănnnnnnnnn Hoàngggg Tháiiiiii", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "      Nguyễn Văn       ", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]

        //Test email
        [TestCase("A", "A", "Anguyễnvan", "0323456789", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "A", "", "0323456789", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "A", "NguyennnnnnnnVannnnnnnAiiiiiiiiiiiiiiiiiiiiiiiiiiiiii12345678", "0323456789", "false", "N", "N", 28, 1, 2003, false)]

        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "A", "NguyennnnnnnnVannnnnnnAiiiiiiiiiiiiiiiiiiiiiiiiiiiii12345678", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "A", "NguyennnnnnnVannnnnnnAiiiiiiiiiiiiiiiiiiiiiiiiiiiii12345678", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "A", "       NguyenVanA112", "0323456789", "false", "N", "N", 28, 1, 2003, true)]

        //Test phone
        [TestCase("A", "A", "h", "qa11231111", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "A", "h", "", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "A", "h", "012345678", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "A", "h", "01234567890", "false", "N", "N", 28, 1, 2003, false)]
        [TestCase("A", "A", "h", "1777123345", "false", "N", "N", 28, 1, 2003, false)]

        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "       0777123345     ", "false", "N", "N", 28, 1, 2003, true)]

        //Test position
        [TestCase("A", "A", "h", "0323456789", "true", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]

        //Test password and confirm password
        [TestCase("A", "A", "h", "0323456789", "false", "", "", 28, 1, 2003, false)]
        [TestCase("A", "A", "h", "0323456789", "false", "NgggggggggggVannnnnnnnnnnnAiiiiiiiiiiiiiiiiiiii12341234123456", "NgggggggggggVannnnnnnnnnnnAiiiiiiiiiiiiiiiiiiii12341234123456", 28, 1, 2003, false)]
        
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "Ng", "Ng", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "NggggggggggVannnnnnnnnnnnAiiiiiiiiiiiiiiiiiii12341234123456", "NggggggggggVannnnnnnnnnnnAiiiiiiiiiiiiiiiiiii12341234123456", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "NgggggggggggVannnnnnnnnnnnAiiiiiiiiiiiiiiiiiii12341234123456", "NgggggggggggVannnnnnnnnnnnAiiiiiiiiiiiiiiiiiii12341234123456", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "       Ngvana1233       ", "       Ngvana1233       ", 28, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "       Ngvana1233       ", 28, 1, 2003, false)]

        //Test birth
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 28, 2, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 29, 2, 2004, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 29, 2, 2003, false)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 30, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 30, 4, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 31, 1, 2003, true)]
        [TestCase("A", "A", "h", "0323456789", "false", "N", "N", 31, 4, 2003, false)]


        [TestCase("A", "A", "Admin", "0323456789", "false", "password1", "password1", 28, 1, 2003, true)]
        [TestCase("A", "A", "staff", "0323456789", "false", "password1", "password1", 28, 1, 2003, true)]
        
        [TestCase("A", "A", "admin", "0323456789", "false", "h", "h", 28, 1, 2003, true)]
        [TestCase("A", "A", "admin", "0323456789", "false", "ha", "ha", 28, 1, 2003, true)]
        [TestCase("A", "A", "admin", "0323456789", "false", "pássword0", "pássword0", 28, 1, 2003, true)]
        [TestCase("A", "A", "admin", "0323456789", "false", "Password1", "Password1", 28, 1, 2003, true)]


        [Apartment(ApartmentState.STA)]
        #endregion
        public void TestAddMember(string fName, string lName, string email, string phone, bool admin, string password, string rePassword, int day, int month, int year, bool expectedResult)
        {
            // Gán giá trị cho email và password
            _addMemberWindow.fName.Text = fName;
            _addMemberWindow.lName.Text = lName;
            _addMemberWindow.Email.Text = email;
            _addMemberWindow.Phone.Text = phone;
            _addMemberWindow.Admin.IsChecked = admin;
           
            _addMemberWindow.Day.SelectedItem=day;
            _addMemberWindow.Month.SelectedItem=month;
            _addMemberWindow.Year.SelectedItem =year;
            _addMemberWindow.Password.Password = password;
            _addMemberWindow.RePassword.Password = rePassword;

            // Thực hiện hành động đăng nhập
            _addMemberWindow.Button_Click(null, null);

            Console.WriteLine($"Expected: {expectedResult}, Actual: {_addMemberWindow.IsSuccess}");

            // Xác nhận kết quả
            //Assert.AreEqual(expectedResult, _loginWindow.IsLoggedIn);
            Assert.That(expectedResult, Is.EqualTo(_addMemberWindow.IsSuccess));
        }
    }
}
