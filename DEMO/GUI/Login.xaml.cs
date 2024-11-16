using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }
        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void openAdminForm(string mail, List<string> permissions)
        {
            ClientSession.Instance.StartSession(mail, permissions);
            SessionManager.StartSession(mail, mail, permissions);

            // Mở giao diện admin
            MainWindow f = new MainWindow();
            f.Show();
            Window.GetWindow(this).Close();
        }

        private void openUserForm(string mail, List<string> permissions)
        {
            ClientSession.Instance.StartSession(mail, permissions);
            SessionManager.StartSession(mail, mail, permissions);

            // Mở giao diện người dùng thông thường
            StaffWindow f = new StaffWindow();
            f.Show();
            Window.GetWindow(this).Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            string email = txtEmail.Text;
            string password = txtPassword.Password;

            ACCOUNT_BLL accountBLL = new ACCOUNT_BLL();
            int permissionID;

            if (accountBLL.AuthenticateAccount(email, password, out permissionID))
            {
                // Tài khoản và mật khẩu đúng
                switch (permissionID)
                {
                    case 1:
                        // Xử lý khi permissionID = 1 (Ví dụ: mở giao diện admin)
                        openAdminForm(email, new[] {permissionID.ToString()}.ToList());
                        break;
                    case 2:
                        // Xử lý khi permissionID = 2 (Ví dụ: mở giao diện người dùng thông thường)
                        openUserForm(email, new[] { permissionID.ToString() }.ToList());
                        break;
                    default:
                        MessageBox.Show("Invalid permission", "Error");
                        break;
                }
            }
            else
            {
                // Tài khoản hoặc mật khẩu không đúng
                MessageBox.Show("Invalid account", "Error");
            }


        }

    }
}
