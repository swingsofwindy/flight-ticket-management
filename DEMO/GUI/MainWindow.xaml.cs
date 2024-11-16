using BLL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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

        private void Btn_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
