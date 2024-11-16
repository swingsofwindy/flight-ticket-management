using BLL;
using ControlzEx.Standard;
using DTO;
using GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
using Button = System.Windows.Controls.Button;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Window6.xaml
    /// </summary>
    public partial class Window6 : UserControl
    {
        private ObservableCollection<CustomerDTO> ViewCustomerData { get; set; } // Danh sách khách hàng được nhập
        public ObservableCollection<FlightInforDTO> Flights { get; set; } // Danh sách chuyến bay thảo mãn

        private FlightDTO selectedFlight { get; set; } // Chuyến bay được chọn
        private TicketClassDTO selectedTicketClass { get; set; } // Hạng vé ứng với chuyến bay được chọn

        private Int32 maxNumTicket = 0; // Số vé tôi đa cho phép nhập = số ghế trống của chuyến bay được chọn
        private Int32 numTicket = 0; // Số vé cần/đang nhập
        private Int64 ticketPrice = 0; // Giá mỗi vé

        private ICollectionView customerView;

        public List<AirportDTO> airports { get; set; }
        private Dictionary<string, string> airportDictionary = new Dictionary<string, string>();
        public List<TicketClassDTO> ticketClasses { get; set; }
        private Dictionary<string, string> ticketClassDictionary = new Dictionary<string, string>();
        public ICommand DeleteCommand { get; private set; }
        public Window6()
        {
            InitializeComponent();
            //this.Loaded += Popup_Loaded;
            //Application.Current.Deactivated += Popup_Deactivated;

            // Test data
            ViewCustomerData = new ObservableCollection<CustomerDTO>();
            numTicket = ViewCustomerData.Count;

            customerView = CollectionViewSource.GetDefaultView(ViewCustomerData);
            MyListView.ItemsSource = customerView;

            Airport_BLL airport_bll = new Airport_BLL();
            Ticket_Class_BLL ticket_class_bll = new Ticket_Class_BLL();
            airports = airport_bll.L_airport();
            ticketClasses = ticket_class_bll.L_TicketClass();
            airportDictionary = airports.ToDictionary(airport => airport.AirportID, airport => airport.AirportName);
            ticketClassDictionary = ticketClasses.ToDictionary(ticketClass => ticketClass.TicketClassID, ticketClass => ticketClass.TicketClassName);
            SourceAirport_popup.ItemsSource = airports;
            DestinationAirport_popup.ItemsSource = airports;
            TicketClass_popup.ItemsSource = ticketClasses;

            // Test data
           /* var flights = new List<FlightInforDTO>
            {
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1001",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1002",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1003",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1004",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1005",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1006",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1007",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1008",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL1009",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL10010",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL10011",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL10012",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL10013",SourceAirportID = "S101",DestinationAirportID = "D201",FlightDay = DateTime.Today.AddDays(1),FlightTime = TimeSpan.FromHours(3),Price = 110.00m},bookedTickets = 52,emptySeats = 148},
            new FlightInforDTO{Flight = new FlightDTO{FlightID = "FL10014",SourceAirportID = "S102",DestinationAirportID = "D202",FlightDay = DateTime.Today.AddDays(2),FlightTime = TimeSpan.FromHours(4),Price = 120.00m},bookedTickets = 54,emptySeats = 146},
            };*/

            dataGridFlights.ItemsSource = Flights;

            DeleteCommand = new RelayCommand<object>(DeleteItem);
            DataContext = this;

        }
        private void DeleteItem(object parameter)
        {
            var itemToRemove = parameter as CustomerDTO;
            if (itemToRemove != null)
            {
                ViewCustomerData.Remove(itemToRemove);
                numTicket = ViewCustomerData.Count;
                TicketQuantity.Text = numTicket.ToString();
                TotalPrice.Text = (numTicket * ticketPrice).ToString() + "  VND";
            }
        }

        private void SelectButton_Click_1(object sender, RoutedEventArgs e)
        {   
            /*
             Sau khi nhấn nút chọn, các thuộc tính cần thiết sẽ được điền
             */
            Button selectButton = sender as System.Windows.Controls.Button;
            if (selectButton != null)
            {
                FlightInforDTO selectedFlightInfo = selectButton.DataContext as FlightInforDTO;
                if (selectedFlightInfo != null)
                {
                    selectedFlight = selectedFlightInfo.Flight;
                    selectedTicketClass = new TicketClassDTO() { TicketClassID = TicketClass_popup.SelectedValue.ToString(), TicketClassName = ticketClassDictionary[TicketClass_popup.SelectedValue.ToString()] };
                    
                    SearchFlight_Popup.IsOpen = false;
                    FlightID.Text = selectedFlightInfo.Flight.FlightID;
                    DepartureAirport.Text = airportDictionary[selectedFlightInfo.Flight.SourceAirportID];
                    DestinationAirport.Text = airportDictionary[selectedFlightInfo.Flight.DestinationAirportID];
                    DepartureTime.Text = selectedFlightInfo.Flight.FlightDay.ToString("dd-MM-yyyy HH:mm");
                    Duration.Text = selectedFlightInfo.Flight.FlightTime.ToString(@"hh\:mm");
                    //TicketClass.Text = ticketClassDictionary[TicketClass_popup.SelectedValue.ToString()];
                    if (TicketClass_popup.SelectedValue != null)
                    {
                        TicketClass.Text = ticketClassDictionary[TicketClass_popup.SelectedValue.ToString()];
                    }
                    else
                    {
                        TicketClass.Text = "All";
                    }
                    TicketPrice.Text = (Convert.ToInt64(selectedFlightInfo.Flight.Price)).ToString() + "  VND";
                    maxNumTicket = selectedFlightInfo.emptySeats;
                    numTicket = Convert.ToInt32(NumTicket.Text.ToString());
                    TicketQuantity.Text = numTicket.ToString();
                    TotalPrice.Text = Convert.ToInt64((numTicket * selectedFlightInfo.Flight.Price)).ToString() + "  VND";

                    var cus = new ObservableCollection<CustomerDTO>();
                    for (int i = 0; i < numTicket; i++)
                    {
                        cus.Add(new CustomerDTO { ID = "", CustomerName = "", Phone = "", Email = "", Birth = new DateTime(2000, 1, 1) });
                    }
                    ViewCustomerData = cus;
                    customerView = CollectionViewSource.GetDefaultView(ViewCustomerData);
                    MyListView.ItemsSource = customerView;
                }
            }
        }

        private void SearchFlight_Click(object sender, RoutedEventArgs e)
        {   

            string state = string.Empty;
            state = ValidateInput_Search();
            if (state != string.Empty)
            {
                var originalTopmost = Application.Current.MainWindow.Topmost;
                Application.Current.MainWindow.Topmost = true;
                MessageBox.Show(Application.Current.MainWindow, state);
                Application.Current.MainWindow.Topmost = originalTopmost;
                return;
            }
            /*MessageBox.Show(SourceAirport_popup.SelectedValue.ToString() + " "
                            + DestinationAirport_popup.SelectedValue.ToString() + " "
                            + DepartureDay_popup.SelectedDate.Value.Date.ToString() + " "
                            + TicketClass_popup.SelectedValue.ToString() + " "
                            + DepartureDay_popup.SelectedDate.Value.Date.AddDays(1).AddTicks(-1).ToString() + " "
                            + "\n Chỉ dành cho debug");*/
            string SourceAirport = (SourceAirport_popup.SelectedValue == null ? "" : SourceAirport_popup.SelectedValue.ToString());
            string DestinationAirport = (DestinationAirport_popup.SelectedValue == null ? "" : DestinationAirport_popup.SelectedValue.ToString());
            string TicketClass = (TicketClass_popup.SelectedValue == null ? "" : TicketClass_popup.SelectedValue.ToString());
            DateTime DepartureDay1 = (DepartureDay_popup.SelectedDate.HasValue ? DepartureDay_popup.SelectedDate.Value.Date : new DateTime(2024,1,1));
            DateTime DepartureDay2 = (DepartureDay_popup.SelectedDate.HasValue ? DepartureDay_popup.SelectedDate.Value.Date.AddDays(1).AddTicks(-1) : new DateTime(3000,1,1));

            List<FlightInforDTO> flights = new BLL.SearchProcessor().GetFlightInfoDTO(SourceAirport, DestinationAirport, DepartureDay1, DepartureDay2, TicketClass,
                                                               int.TryParse(NumTicket.Text, out int numTicket) ? numTicket : 0);
            dataGridFlights.ItemsSource = new ObservableCollection<FlightInforDTO>(flights);

            /*MessageBox.Show(flights.list.Count + flights.list[0].Flight.FlightID + flights.state + "\n Chỉ dùng cho debug", "Debug");*/
        }
        static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Sử dụng biểu thức chính quy để kiểm tra định dạng email
                string pattern = @"^[a-zA-Z]+@gmail\.com$";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                return regex.IsMatch(email);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private string CheckInfo()
        {
            List <CustomerDTO> customers = customerView.OfType<CustomerDTO>().ToList();
            foreach(CustomerDTO customer in customers)
            {
                if(customer.CustomerName == string.Empty)
                {
                    return "Please enter Customer Name";
                }
                foreach (char c in customer.ID)
                {
                    if (char.IsLetter(c))
                    {
                        return "Invalid ID";
                    }
                }
                if (customer.ID.Length != 12)
                {
                    return "CCCD has 12 number";
                }  
                if (customer.Birth == null)
                {
                    return "Please enter Customer birthday";
                }
                foreach (char c in customer.Phone)
                {
                    if (char.IsLetter(c))
                    {
                        return "Invalid phone number";
                    }
                }
                if(customer.Phone.Length != 10)
                {
                    return "Invalid phone number";
                }
                if (!IsValidEmail(customer.Email))
                {
                    return "Please enter a valid email";
                }
            }
            return string.Empty;
        }
        private string ValidateInput_Search()
        {
            if (isNatural(NumTicket.Text.ToString()) == false)
            {
                return "The number of tickets must be a positive number!";
            }

            if (SourceAirport_popup.SelectedIndex == -1)
            {
                return "Please select departure airport!";
            }

            if (DestinationAirport_popup.SelectedIndex == -1)
            {
                return "Please select destination airport";
            }

            if (TicketClass_popup.SelectedIndex == -1)
            {
                return "Please select ticket class";
            }
            
            if (DepartureDay_popup.SelectedDate.HasValue == false)
            {
                return "Please select a departure date";
            }

            return string.Empty;
        }
        private bool isNatural(string input)
        {
            try
            {
                Int64 t = Convert.ToInt64(input);
                if (t > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            /*
             Mô tả: Thêm dữ liệu vào DB, nếu thành công xóa toàn bộ dữ liệu trên UI để nhập tiếp
             */
            string state = CheckInfo();
            if (state == string.Empty)
            {
                state = new BLL.InsertProcessor().Add_ListBookingTicket(customerView.OfType<CustomerDTO>().ToList(), selectedFlight, selectedTicketClass, DateTime.Now, 1);
            }
            else
            {
                var originalTopmost = Application.Current.MainWindow.Topmost;
                Application.Current.MainWindow.Topmost = true;
                MessageBox.Show(Application.Current.MainWindow, state, "Error");
                Application.Current.MainWindow.Topmost = originalTopmost;
                return;
            }
            if (state == string.Empty)
            {
                ResetData();
            }
        }

        private void ResetData()
        {
            FlightID.Text = string.Empty;
            DepartureAirport.Text = string.Empty;
            DestinationAirport.Text = string.Empty;
            DestinationAirport_popup.Text = string.Empty;
            DepartureTime.Text = string.Empty;
            TicketClass.Text = string.Empty;
            TicketPrice.Text = string.Empty;

            ViewCustomerData = new ObservableCollection<CustomerDTO>();
            numTicket = ViewCustomerData.Count;
            customerView = CollectionViewSource.GetDefaultView(ViewCustomerData);
            MyListView.ItemsSource = customerView;

            maxNumTicket = 0;
            numTicket = 0;
            ticketPrice = 0;

            TicketQuantity.Text = string.Empty;
            TotalPrice.Text = string.Empty;
            NumTicket.Text = string.Empty;
            SourceAirport_popup.Text = string.Empty;
            DestinationAirport_popup.Text = string.Empty;
            TicketClass_popup.Text = string.Empty;

            Flights = new ObservableCollection<FlightInforDTO>();
            dataGridFlights.ItemsSource = Flights;
        }

        private void DepartureDay_popup_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void textMaChuyenBay_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtMaChuyenBay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textGiave_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtGiaVe_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
        private void FindFlight_Click(object sender, RoutedEventArgs e)
        {
            SearchFlight_Popup.IsOpen = true;
        }

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            SearchFlight_Popup.IsOpen = true;
        }

        private void Popup_Deactivated(object sender, EventArgs e)
        {
            SearchFlight_Popup.IsOpen = false;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            SourceAirport_popup.SelectedIndex = -1;
            DestinationAirport_popup.SelectedIndex = -1;
            DepartureDay_popup.SelectedDate = null;
            TicketClass_popup.SelectedIndex = -1;
            SearchFlight_Popup.IsOpen = false;
        }
    }
}

