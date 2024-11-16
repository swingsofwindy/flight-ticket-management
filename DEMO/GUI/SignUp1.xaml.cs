using System;
using System.Collections.Generic;
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
        public SignUp1()
        {
            InitializeComponent();
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
            // Biểu thức chính quy để kiểm tra ký tự đặc biệt
            Regex regex = new Regex("[^0-9]");
            return regex.IsMatch(str);
        }

        public string AddMember (ACCOUNT user, out string message)
        {
            message= string.Empty;
            if (HasSpecialCharacters(User.UserName))
            {
                message = "User Name has special character";
                return message;
            }
            if (PositiveIntegerChecking(User.Phone))
            {
                message = "User Phone has special character";
                return message;
            }
            if (Admin_bt.AllowDrop)
            {
                User.PermissonID = 1;
            }
            else
            {
                User.PermissonID = 2;
            }
            string kq = "";
            //accBLL.SignUp(User, ref kq);
            new BLL.InsertProcessor().SignUp(User, ref kq);

            if (kq == "")
            {
                return "Sign Up Success";
            }
            else
            {
                message = kq;
                return message;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User.UserName = txtFName.Text.Trim() + " " + txtLName.Text.Trim();
            User.Email = txtMailAdd.Text.Trim() + "@gmail.com";
            User.Birth = new DateTime((int)Y_comboBox.SelectedValue, (int)M_comboBox.SelectedValue, (int)D_comboBox.SelectedValue);
            User.PasswordUser = txtRePassword.Password.Trim();
            User.Phone = txtPhone.Text.Trim();

            string message;
            string result=AddMember(User, out message);
            MessageBox.Show(result);
            if(result == "Sign Up Success")
            {
                Window.GetWindow(this).Close();
            }    
        }
    }
}
