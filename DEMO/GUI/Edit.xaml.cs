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

namespace GUI
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        public Edit()
        {
            InitializeComponent();
        }

        private void textMaChuyenBay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtMaChuyenBay.Focus();
        }

        private void txtMaChuyenBay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaChuyenBay.Text) && txtMaChuyenBay.Text.Length > 0)
            {
                textMaChuyenBay.Visibility = Visibility.Collapsed;
            }
            else
            {
                textMaChuyenBay.Visibility = Visibility.Visible;
            }
        }

        private void textGiaVe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtGiaVe.Focus();
        }

        private void txtGiaVe_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGiaVe.Text) && txtGiaVe.Text.Length > 0)
            {
                textGiaVe.Visibility = Visibility.Collapsed;
            }
            else
            {
                textGiaVe.Visibility = Visibility.Visible;
            }
        }

        private void textGheHang1_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtGheHang1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textGheHang2_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtGheHang2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textGheTrong_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtGheTrong_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        //private void textSanBayDi_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void txtSanBayDi_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}
    }
}
