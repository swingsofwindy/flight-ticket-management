using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Window8.xaml
    /// </summary>
    /// 


    /* Mô tả:
     Tìm kiếm vé theo CustomerID, FlightID, TicketID, Status. Có thể bổ sung nhiều thuộc tính hơn nếu có thời gian
     Mỗi thuộc tính tìm kiếm đều có thể NULL
     
     Lưu ý: Các Processor cần chứa thuộc tính state là trạng thái xử lí của Process, nếu state là chuỗi rỗng thì xem như thành công
     
     Khác: Khi vé được chèn vào db, sẽ có Status mặc định là 1 - Sold
                 Khi chuyến bay cất cánh, Status của vé chuyển sang 0 - Flown
                 Khi hủy vé, isDeleted = 1;

     */
    public partial class Window8 : UserControl
    {
        private ObservableCollection<BookingTicketDTO> listTicket;
        public Window8()
        {
            InitializeComponent();
            listTicket = new ObservableCollection<BookingTicketDTO>(){};

            dataGrid.ItemsSource = listTicket;

            Status.ItemsSource = new List<ST>
            {
                new ST(){ID = "-1", Name = "All"},
                new ST(){ID = "1", Name = "Sold"},
                new ST(){ID = "2", Name = "Flown"}
            };
            Status.SelectedValue = "1";
            BLL.BookingTicket_BLL.UpdateStatus();
        }

        private void Click_Search(object sender, RoutedEventArgs e)
        {
            string state = string.Empty;
            try
            {
                BLL.BookingTicket_BLL.UpdateStatus();
                string customerID = CustomerID.Text;
                string ticketID = TicketID.Text;
                string flightID = FlightID.Text;
                int status = Convert.ToInt32(Status.SelectedValue.ToString());
                BLL.SearchProcessor prc = new BLL.SearchProcessor();
                var result = prc.GetBookingTicket(ticketID, customerID, flightID, status);
                if (prc.GetState() == string.Empty)
                {
                    listTicket = new ObservableCollection<BookingTicketDTO>(result);
                    dataGrid.ItemsSource = listTicket;
                    MessageBox.Show($"Found {listTicket.Count} tickets");
                }
            }
            catch(Exception ex)
            {
                state = $"Error: {ex.Message}";
                MessageBox.Show(state);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var item = button.DataContext as BookingTicketDTO;
                if (item.TicketStatus == 2)
                {
                    MessageBox.Show($"Flown tickets cannot be canceled");
                    return;
                }
                BLL.BookingTicket_BLL checkprc = new BookingTicket_BLL();
                DateTime dpt = checkprc.GetBookingTicket_DepartureTime(item.TicketID);
                var para = new BLL.SearchProcessor().GetParameterDTO();
                if (DateTime.Now.Add(para.CancelTime) >= dpt)
                {
                    MessageBox.Show($"Tickets can only be cancelled at least {para.CancelTime.Hours}h{para.CancelTime.Minutes}m before departure");
                    return;
                }
                if (item != null)
                {   
                    BLL.DeleteDataProcessor prc = new BLL.DeleteDataProcessor();

                    prc.DeleteTicket(item.TicketID);
                    if (prc.getState() != string.Empty)
                    {
                        MessageBox.Show(prc.getState());
                        return;
                    }
                    listTicket.Remove(item);
                }
            }
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var item = button.DataContext;
            }
        }
    }

    public class IdToNameConverterTK : IValueConverter
    {
        private Dictionary<string, string> idToNameMap = new Ticket_Class_BLL().L_TicketClass().ToDictionary(ticketclass => ticketclass.TicketClassID, ticketclass => ticketclass.TicketClassName);

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

    public class IdToNameConverterST : IValueConverter
    {
        private Dictionary<string, string> idToNameMap = new Dictionary<string, string>
        {
            {"1", "Sold"},
            {"2", "Flown"}
        };

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

    public class ST
    {
        public ST() { }
        public ST(string name) { }
        public string Name { get; set; }
        public string ID { get; set; }
    }
}
