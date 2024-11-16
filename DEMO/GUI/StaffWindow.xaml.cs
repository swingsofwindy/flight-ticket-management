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

namespace GUI
{
    /// <summary>
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        public StaffWindow()
        {
            InitializeComponent();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {
            BLL.SessionManager.EndSession(ClientSession.Instance.mail);
            GUI.ClientSession.Instance.EndSession();
            Login f = new Login();
            f.Show();
            Window.GetWindow(this).Close();
        }
    }
}
