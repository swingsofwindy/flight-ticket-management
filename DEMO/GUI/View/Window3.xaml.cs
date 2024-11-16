using BLL;
using DTO;
using GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static GUI.View.Window3;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : UserControl
    {
        ObservableCollection<ACCOUNT> members = new ObservableCollection<ACCOUNT>();
        private List<string> suggestions = new List<string> { "Gợi ý 1", "Gợi ý 2", "Gợi ý 3" };
        public Window3()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            LoadMembers();
        }

        private void LoadMembers()
        {
            BLL.ACCOUNT_BLL prc = new BLL.ACCOUNT_BLL();
            var result = prc.List_acc(new DTO.ACCOUNT());
            members = new ObservableCollection<ACCOUNT>(result);
            MembersDataGrid.ItemsSource = members;
        }

        public class Members
        {
            public string Seq { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string Position { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Birth { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Addmenber f = new Addmenber();
            SignUp1 f = new SignUp1();
            f.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var data = button.DataContext as ACCOUNT;
                if (data != null && MembersDataGrid.ItemsSource is ObservableCollection<ACCOUNT> collection)
                {   
                    if (data.PermissonID == 1)
                    {
                        MessageBox.Show("You do not have permission to delete this member");
                        return;
                    }
                    var result = MessageBox.Show("Are you sure you want to delete this member?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {   BLL.ACCOUNT_BLL prc = new BLL.ACCOUNT_BLL();
                        prc.deleteAccount(data.UserID);
                        collection.Remove(data);
                    }
                }
            }
        }
    }
    public class IdToNameConverterPS : IValueConverter
    {
        private Dictionary<string, string> idToNameMap = new Dictionary<string, string>() { { "1", "Admin" }, { "2", "Staff" } };
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";

            string id = value.ToString();
            if (idToNameMap.TryGetValue(id, out string name))
            {
                return name;
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }

    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd-MM-yyyy");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", culture, DateTimeStyles.None, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return value;
        }
    }
}
