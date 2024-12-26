using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL;
using DTO;
namespace GUI
{
    /// <summary>
    /// Interaction logic for SignUp1.xaml
    /// </summary>

    public partial class SignUp1 : Window
    {
        public TextBox fName {  get; set; }
        public TextBox lName { get; set; }
        public TextBox Email { get; set; }
        public TextBox Phone { get; set; }
        public RadioButton Admin { get; set; }
        public ComboBox Day { get; set; }
        public ComboBox Month { get; set; }
        public ComboBox Year { get; set; }
        public PasswordBox Password { get; set; }
        public PasswordBox RePassword { get; set; }

        public bool IsSuccess { get; private set; }
        private IAddMemberBLL addMemberBLL= new InsertProcessor();

        public SignUp1()
        {
            InitializeComponent();
            fName= new TextBox();
            lName= new TextBox();
            Email= new TextBox();
            Phone= new TextBox();
            Admin= new RadioButton();

            Password = new PasswordBox();
            RePassword= new PasswordBox();
            //Day
            List<int> days = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                days.Add(i);
            }
            D_comboBox.ItemsSource = days;
            //Month
            List<int> months = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(i);
            }

            M_comboBox.ItemsSource = months;
            //Year
            int currentYear = DateTime.Now.Year;

            // Khởi tạo danh sách các năm
            List<int> years = new List<int>();
            for (int i = currentYear - 80; i <= currentYear; i++)
            {
                years.Add(i);
            }
            Y_comboBox.ItemsSource = years;
        }
        private void textFName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtFName.Focus();
        }

        private void txtFName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFName.Text) && txtFName.Text.Length > 0)
            {
                textFName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textFName.Visibility = Visibility.Visible;
            }
        }

        private void textLName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtLName.Focus();
        }

        private void txtLName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLName.Text) && txtLName.Text.Length > 0)
            {
                textLName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textLName.Visibility = Visibility.Visible;
            }
        }

        private void textMailAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtMailAdd.Focus();
        }

        private void txtMailAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMailAdd.Text) && txtMailAdd.Text.Length > 0)
            {
                textMailAdd.Visibility = Visibility.Collapsed;
            }
            else
            {
                textMailAdd.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void textRePassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtRePassword.Focus();
        }

        private void txtRePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRePassword.Password) && txtRePassword.Password.Length > 0)
            {
                textRePassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textRePassword.Visibility = Visibility.Visible;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Login f = new Login();
            //f.Show();
            Window.GetWindow(this).Close();
        }

        private void textPhone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPhone.Focus();
        }

        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPhone.Text) && txtPhone.Text.Length > 0)
            {
                textPhone.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPhone.Visibility = Visibility.Visible;
            }
        }

        ACCOUNT_BLL accBLL = new ACCOUNT_BLL();
        ACCOUNT User = new ACCOUNT();
        static bool HasSpecialCharacters(string str)
        {
            // Regex pattern để kiểm tra ký tự đặc biệt (ngoại trừ khoảng trắng)
            string pattern = @"[^\w\sÀ-ỹ]"; // \w bao gồm chữ cái và số, \s là khoảng trắng, À-ỹ cho các ký tự tiếng Việt

            // Tạo đối tượng Regex với pattern
            Regex regex = new Regex(pattern);

            // Kiểm tra nếu chuỗi có chứa ký tự đặc biệt
            return regex.IsMatch(str);
        }

        static bool PositiveIntegerChecking(string str)
        {
            // Regex kiểm tra số điện thoại
            string pattern = @"^0\d{9}$"; // Bắt đầu bằng 0, sau đó là 9 chữ số (10 chữ số tổng cộng)
            return string.IsNullOrEmpty(str) || !Regex.IsMatch(str, pattern);
        }
        static bool ContainsVietnameseToneMarks(string input)
        {
            // Loại bỏ dấu tiếng Việt bằng Normalize
            string normalized = input.Normalize(NormalizationForm.FormD);
            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                {
                    return true;
                }
            }
            return false;
        }
        bool TrySetBirthDate(int year, int month, int day, out DateTime birthDate)
        {
            birthDate = default;

            // Kiểm tra năm, tháng, ngày hợp lệ
            if (year >= DateTime.MinValue.Year && year <= DateTime.MaxValue.Year &&
                month >= 1 && month <= 12 &&
                day >= 1 && day <= DateTime.DaysInMonth(year, month))
            {
                birthDate = new DateTime(year, month, day);
                return true;
            }

            return false;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(fName.Text) || string.IsNullOrEmpty(lName.Text))
            {
                IsSuccess = false;
                MessageBox.Show("First name or last name is null.","Error");
                return;
            }

            if (fName.Text.Length>40 || lName.Text.Length > 40)
            {
                IsSuccess = false;
                MessageBox.Show("First name or last name is over 40 characters.","Error");
                return;
            }

            if (ContainsVietnameseToneMarks(Email.Text) || ContainsVietnameseToneMarks(Phone.Text))
            {
                IsSuccess = false;
                MessageBox.Show("Email or phone number have tone marks.", "Error");
                return;
            }

            if (string.IsNullOrEmpty(Password.Password) || string.IsNullOrEmpty(RePassword.Password)||string.IsNullOrEmpty(Email.Text))
            {
                IsSuccess = false;
                MessageBox.Show("Email, password or confirm password is null.", "Error");
                return;
            }

            if (Email.Text.Length>60|| Password.Password.Length > 60 || RePassword.Password.Length > 60)
            {
                IsSuccess = false;
                MessageBox.Show("Email, password or confirm password is over 60 characters.", "Error");
                return;
            }

            if (Password.Password!=RePassword.Password)
            {
                IsSuccess=false;
                MessageBox.Show("Confirm password fail.", "Error");
                return;
            }

            if (!TrySetBirthDate((int)Year.SelectedValue, (int)Month.SelectedValue, (int)Day.SelectedValue, out var birthDate))
            {
                IsSuccess = false;
                MessageBox.Show("Birth invalid");
                return;
            }
            User.UserName = fName.Text.Trim() + " " + lName.Text.Trim();
            User.Email = Email.Text.Trim() + "@gmail.com";
            User.Birth = new DateTime((int)Year.SelectedValue, (int)Month.SelectedValue, (int)Day.SelectedValue);
            User.PasswordUser = RePassword.Password.Trim();
            User.Phone = Phone.Text.Trim();
            IsSuccess = true;
            
            if (HasSpecialCharacters(User.UserName))
            {
                IsSuccess = false;
                MessageBox.Show("User Name has special character");
                return;
            }
            if (PositiveIntegerChecking(User.Phone)) 
            {
                IsSuccess = false;
                MessageBox.Show("User Phone has special character, null or not 0 first");
                return;
            }

            if (User.Phone.Length != 10)
            {
                IsSuccess = false;
                MessageBox.Show("Phone number contains 10 characters.", "Error");
                return;
            }

            if (Admin.IsChecked==true)
            {
                User.PermissonID = 1;
            }
            else
            {
                User.PermissonID = 2;
            }
            string kq = "";
            //accBLL.SignUp(User, ref kq);
            addMemberBLL.Add_Member(User, ref kq);

            if (kq == "")
            {
                IsSuccess = true;
                MessageBox.Show("Member Add Success");
                /*Login f = new Login();
                f.Show();*/
                Window.GetWindow(this).Close();
            }
            else
            {
                IsSuccess = false;
                MessageBox.Show(kq);
            }
        }
    }
}
