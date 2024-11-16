using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DTO;
using GUI.ViewModel;
using System.Collections;
using System.Web.UI.WebControls;
using ControlzEx.Standard;
using BLL;
using System.Data.SqlClient;
using static GUI.View.Window2;
using TextBox = System.Windows.Controls.TextBox;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    /// 

    public partial class Window9 : UserControl
    {
        Flight_BLL fl_bll = new Flight_BLL();
        Airport_BLL airport_bll = new Airport_BLL();
        Ticket_Class_BLL ticket_class_bll = new Ticket_Class_BLL();

        private ObservableCollection<TicketClass> ticketList;
        //private TicketClass defaultTicketClass = new TicketClass { ID = "Default", Name = "Default", Quantity = -1, Multiplier = 0 };
        private ObservableCollection<IntermediateAirport> IAList; // Intermidate Airport List
        //private IntermediateAirport defaultIA = new IntermediateAirport { ID = "Default", Name = "Default", LayoverTime = TimeSpan.FromMinutes(0), Note = "..." };
        private ICollectionView collectionViewTicketClass;
        private ICollectionView collectionViewIA;

        public ParameterDTO parameterDTO;
        public List<TicketClassDTO> ticketClasses { get; set; }
        public List<AirportDTO> airports { get; set; }
        public Window9()
        {
            InitializeComponent();

            parameterDTO = new BLL.SearchProcessor().GetParameterDTO();
            airports = airport_bll.L_airport();
            ticketClasses = ticket_class_bll.L_TicketClass();

            ticketList = new ObservableCollection<TicketClass>
            {
                new TicketClass { ID = "Default", Name = "Default", Quantity = -1, Multiplier = 0 },
                new TicketClass { ID = "Default", Name = "Default", Quantity = -1, Multiplier = 0 }
            };
            IAList = new ObservableCollection<IntermediateAirport>
            {
            };
            collectionViewTicketClass = CollectionViewSource.GetDefaultView(ticketList);
            collectionViewIA = CollectionViewSource.GetDefaultView(IAList);
            dataGrid1.ItemsSource = collectionViewTicketClass;
            dataGrid2.ItemsSource = collectionViewIA;
            DestinationAirport.ItemsSource = airports;
            DestinationAirportID.ItemsSource = airports;
            SourceAirport.ItemsSource = airports;
            SourceAirportID.ItemsSource = airports;
            DataContext = this;
        }
        private void ConfirmSchedule_Click(object sender, RoutedEventArgs e)
        {
            string state = string.Empty;

            state = ValidateInput();
            if (state != string.Empty)
            {
                MessageBox.Show(state, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ScheduleData data = GetScheduleData();
            FlightDTO flightDTO = data.InitializeFlightDTO();
            List<TicketClassFlightDTO> listTicketClassFlightDTO = data.InitializeListTicketClassFlightDTO();
            List<IntermediateAirportDTO> listIntermediateAirportDTO = data.InitializeListIntermediateAirportDTO();

            state = ValidateLogicDB(flightDTO, listTicketClassFlightDTO, listIntermediateAirportDTO);
            if (state != string.Empty)
            {
                MessageBox.Show(state, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            state = new BLL.InsertProcessor().AddFlightInfor(flightDTO, listTicketClassFlightDTO, listIntermediateAirportDTO);
            if (state == string.Empty)
            {
                MessageBox.Show("Succesful");
                ResetDataWindow();
                var newTicket = new TicketClass { ID = "Default", Name = "Default", Quantity = -1, Multiplier = 0 };
                ticketList.Add(newTicket);
                collectionViewTicketClass.MoveCurrentTo(newTicket);
            }
            else
            {
                MessageBox.Show(state, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        static bool PositiveIntegerChecking(string str)
        {
            // Biểu thức chính quy để kiểm tra ký tự đặc biệt
            Regex regex = new Regex("[^0-9.]");
            return regex.IsMatch(str);
        }
        public string ValidateInput()
        {
            if (SourceAirportID.SelectedIndex == -1 || DestinationAirportID.SelectedIndex == -1)
            {
                return "Please select airport!";
            }
            else if (SourceAirportID.SelectedValue == DestinationAirportID.SelectedValue)
            {
                return "The departure and destination airports must not be the same!";
            }
            else if (FlightDay.SelectedDate == null)
            {
                return "Please enter a departure date!";
            }
            else if (!DepartureTime.SelectedTime.HasValue)
            {
                return "Please enter a departure time!";
            }
            else if (!FlightTime.SelectedTime.HasValue)
            {
                return "Please enter the flight duration!";
            }
            if (FlightDay.SelectedDate < DateTime.Today)
            {
                return "Please select a departure date from " + DateTime.Today.ToString("dd/MM/yyyy");
            }
            else if (FlightDay.SelectedDate == DateTime.Today && DepartureTime.SelectedTime < DateTime.Now)
            {
                return "Please select a departure time starting from " + DateTime.Now.ToString("dd/MM/yyyy H:m:s");
            }
            else if (TicketPrice.Text == string.Empty && PositiveIntegerChecking(TicketPrice.Text))
            {
                return "Please enter the ticket price!";
            }
            else if (isNatural(TicketPrice.Text) == false)
            {
                return "The ticket price must be a natural number!";
            }
            return string.Empty;
        }

        private string ValidateLogicDB(FlightDTO flightDTO, List<TicketClassFlightDTO> listTicketClassFlightDTO, List<IntermediateAirportDTO> listIntermediateAirportDTO)
        {
            if (FlightTime.SelectedTime.Value.TimeOfDay < parameterDTO.MinFlighTime)
            {
                return $"Flight time must be greater than  {parameterDTO.MinFlighTime}";
            }

            try
            {
                HashSet<string> set = new HashSet<string>();
                foreach (TicketClassFlightDTO obj in listTicketClassFlightDTO)
                {
                    if (!set.Add(obj.TicketClassID))
                    {
                        return "Ticket classes must be different";
                    }

                    if (obj.Quantity <= 0)
                    {
                        return "Ticket quantity must be greater than 0";
                    }

                    if (obj.Multiplier <= 0)
                    {
                        return "Ticket class multiplier must be greater than 0";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

            try
            {
                HashSet<string> set = new HashSet<string>();
                foreach (IntermediateAirportDTO obj in listIntermediateAirportDTO)
                {
                    if (!set.Add(obj.AirportID))
                    {
                        return "Intermediate airports must be different";
                    }

                    if (obj.LayoverTime < parameterDTO.MinStopTime || obj.LayoverTime > parameterDTO.MaxStopTime)
                    {
                        return $"Layover time must be greater than  {parameterDTO.MinStopTime.Hours}h{parameterDTO.MinStopTime.Minutes}m and less than {parameterDTO.MaxStopTime.Hours}h {parameterDTO.MaxStopTime.Minutes}m";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
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

        private void ResetDataWindow()
        {
            SourceAirport.SelectedIndex = -1;
            SourceAirport.Text = string.Empty;
            DestinationAirport.SelectedIndex = -1;
            DestinationAirport.Text = string.Empty;
            SourceAirportID.SelectedIndex = -1;
            SourceAirportID.Text = string.Empty;
            DestinationAirportID.SelectedIndex = -1;
            DestinationAirportID.Text = string.Empty;
            TicketPrice.Text = string.Empty;
            FlightDay.SelectedDate = null;
            FlightTime.SelectedTime = null;
            DepartureTime.SelectedTime = null;
            ticketList.Clear();
            IAList.Clear();
            collectionViewTicketClass.Filter = null;
            collectionViewIA.Filter = null;
        }

        private ScheduleData GetScheduleData()
        {
            ScheduleData data = new ScheduleData();
            data.sourceAirportID = SourceAirportID.Text.Trim();
            data.destinationAirportID = DestinationAirportID.Text.Trim();
            data.flightID = fl_bll.Get_ID();
            data.price = decimal.TryParse(TicketPrice.Text.Trim(), out decimal price) ? price : -1;
            //data.flightDay = FlightDay.SelectedDate ?? DateTime.MinValue;
            data.flightDay = combinedDayTime();
            data.flightTime = FlightTime.SelectedTime.HasValue ? FlightTime.SelectedTime.Value.TimeOfDay : TimeSpan.Zero;
            data.IAList = IAList;
            data.ticketList = ticketList;
            return data;
        }

        private DateTime combinedDayTime()
        {
            DateTime combined;
            if (FlightDay.SelectedDate.HasValue)
            {
                if (DepartureTime.SelectedTime.HasValue)
                {
                    combined = FlightDay.SelectedDate.Value.Date + DepartureTime.SelectedTime.Value.TimeOfDay;
                }
                else
                {
                    combined = FlightDay.SelectedDate.Value.Date + TimeSpan.Zero;
                }
            }
            else
            {
                combined = DateTime.MinValue;
            }
            return combined;
        }
        /*---------------------------------------------BEGIN R1------------------------------------------------*/

        private void DestinationAirport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DestinationAirport.SelectedItem is AirportDTO selectedAirport)
            {
                DestinationAirportID.SelectedValue = selectedAirport.AirportID;
            }
        }

        private void DestinationAirportID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DestinationAirportID.SelectedItem is AirportDTO selectedAirport)
            {
                DestinationAirport.SelectedValue = selectedAirport.AirportID;
            }
        }

        private void SourceAirport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SourceAirport.SelectedItem is AirportDTO selectedAirport)
            {
                SourceAirportID.SelectedValue = selectedAirport.AirportID;
            }
        }

        private void SourceAirportID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SourceAirportID.SelectedItem is AirportDTO selectedAirport)
            {
                SourceAirport.SelectedValue = selectedAirport.AirportID;
            }
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem == null && !string.IsNullOrEmpty(comboBox.Text))
            {
                comboBox.Text = "";
            }
        }

        /*---------------------------------------------END R1--------------------------------------------------*/

        /*---------------------------------------------BEGIN Data Grid 1 aka TicketClass------------------------------------------------*/

        private void AddTicket_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.Items.Count >= parameterDTO.TicketClassCount)
            {
                MessageBox.Show($"Cannot add ticket class. The maximum ticket class is {parameterDTO.TicketClassCount}");
                return;
            }

            var newTicket = new TicketClass { ID = "Default", Name = "Default", Quantity = -1, Multiplier = 0 };
            ticketList.Add(newTicket);
            collectionViewTicketClass.MoveCurrentTo(newTicket);

        }
        private void ResetTicket_Click(object sender, RoutedEventArgs e)
        {
            ticketList.Clear();
            var newTicket = new TicketClass { ID = "Default", Name = "Default", Quantity = -1, Multiplier = 0 };
            ticketList.Add(newTicket);
            collectionViewTicketClass.MoveCurrentTo(newTicket);
        }
        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.Items.Count <= 1)
            {
                MessageBox.Show($"Cannot delete ticket class. The minimum ticket class is 1");
                return;
            }
            TicketClass selectedTicket = (TicketClass)dataGrid1.SelectedItem;
            if (selectedTicket != null)
            {
                ((ObservableCollection<TicketClass>)collectionViewTicketClass.SourceCollection).Remove(selectedTicket);
                dataGrid1.ItemsSource = collectionViewTicketClass;
            }
        }

        public void UpdateMultiplierById(string id)
        {
            var ticketClass = ticketList.FirstOrDefault(t => t.ID == id);
            var ticketClassDTO = ticketClasses.FirstOrDefault(t => t.TicketClassID == id);

            if (ticketClass != null && ticketClassDTO != null)
            {
                ticketClass.Multiplier = ticketClassDTO.BaseMultiplier;
            }
        }

        private void ComboBox_BaseMultiplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ComboBox_TicketClassID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBoxID = sender as ComboBox;
            if (comboBoxID.SelectedItem is TicketClassDTO selectedTicketClass)
            {

                BindingExpression binding = comboBoxID.GetBindingExpression(ComboBox.SelectedValueProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }

                var dataGridRow = DataGridRow.GetRowContainingElement(comboBoxID);
                var comboBoxName = FindChild<ComboBox>(dataGridRow, "ComboBoxName");
                var textBoxMultiplier = FindChild<TextBox>(dataGridRow, "TextBoxMultiplier");


                if (comboBoxName != null)
                {
                    comboBoxName.SelectedValue = selectedTicketClass.TicketClassName;
                    BindingExpression bindingName = comboBoxName.GetBindingExpression(ComboBox.SelectedValueProperty);
                    if (bindingName != null)
                    {
                        bindingName.UpdateSource();
                    }
                }

                if (textBoxMultiplier != null)
                {
                    textBoxMultiplier.Text = selectedTicketClass.BaseMultiplier.ToString();
                    BindingExpression bindingmul = textBoxMultiplier.GetBindingExpression(TextBox.TextProperty);
                    if (bindingmul != null)
                    {
                        bindingmul.UpdateSource();
                    }
                }
            }
        }

        private void ComboBox_TicketClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBoxName = sender as ComboBox;
            if (comboBoxName.SelectedItem is TicketClassDTO selectedTicketClass)
            {
                BindingExpression binding = comboBoxName.GetBindingExpression(ComboBox.SelectedValueProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }

                var dataGridRow = DataGridRow.GetRowContainingElement(comboBoxName);
                var comboBoxID = FindChild<ComboBox>(dataGridRow, "ComboBoxID");
                var textBoxMultiplier = FindChild<TextBox>(dataGridRow, "TextBoxMultiplier");

                if (comboBoxID != null)
                {
                    comboBoxID.SelectedValue = selectedTicketClass.TicketClassID;
                    BindingExpression bindingid = comboBoxID.GetBindingExpression(ComboBox.SelectedValueProperty);
                    if (bindingid != null)
                    {
                        bindingid.UpdateSource();
                    }
                }

                if (textBoxMultiplier != null)
                {
                    textBoxMultiplier.Text = selectedTicketClass.BaseMultiplier.ToString();
                    BindingExpression bindingmul = textBoxMultiplier.GetBindingExpression(TextBox.TextProperty);
                    if (bindingmul != null)
                    {
                        bindingmul.UpdateSource();
                    }
                }
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            if (parent == null) return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        /*---------------------------------------------END Data Grid 1 aka TicketClass------------------------------------------------*/

        /*---------------------------------------------BEGIN Data Grid 2 aka IA-------------------------------------------------------*/

        private void AddIA_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.Items.Count >= parameterDTO.IntermediateAirportCount)
            {
                MessageBox.Show($"Cannot add intermidate airport. The maximum intermidate airport is {parameterDTO.IntermediateAirportCount}");
                return;
            }

            var newIA = new IntermediateAirport { ID = "Default", Name = "Default", LayoverTime = TimeSpan.FromMinutes(0), Note = "..." };
            IAList.Add(newIA);
            collectionViewTicketClass.MoveCurrentTo(newIA);
        }

        private void ResetIA_Click(object sender, RoutedEventArgs e)
        {
            IAList.Clear();
        }

        private void DeleteButton_Click_2(object sender, RoutedEventArgs e)
        {
            IntermediateAirport selectedIA = (IntermediateAirport)dataGrid2.SelectedItem;
            if (selectedIA != null)
            {
                ((ObservableCollection<IntermediateAirport>)collectionViewIA.SourceCollection).Remove(selectedIA);
                dataGrid2.ItemsSource = collectionViewIA;
            }
        }

        private void ComboBox_AirportID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBoxIAID = sender as ComboBox;
            if (comboBoxIAID.SelectedItem is AirportDTO selectedIA)
            {
                BindingExpression bindingID = comboBoxIAID.GetBindingExpression(ComboBox.SelectedValueProperty);
                if (bindingID != null)
                {
                    bindingID.UpdateSource();
                }

                var dataGridRow = DataGridRow.GetRowContainingElement(comboBoxIAID);
                var comboBoxIAName = FindChild<ComboBox>(dataGridRow, "ComboBoxIAName");

                if (comboBoxIAName != null)
                {
                    comboBoxIAName.SelectedValue = selectedIA.AirportName;
                    BindingExpression bindingName = comboBoxIAName.GetBindingExpression(ComboBox.SelectedValueProperty);
                    if (bindingName != null)
                    {
                        bindingName.UpdateSource();
                    }
                }
            }
        }

        private void ComboBox_AirportName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBoxIAName = sender as ComboBox;
            if (comboBoxIAName.SelectedItem is AirportDTO selectedIA)
            {

                BindingExpression bindingName = comboBoxIAName.GetBindingExpression(ComboBox.SelectedValueProperty);
                if (bindingName != null)
                {
                    bindingName.UpdateSource();
                }

                var dataGridRow = DataGridRow.GetRowContainingElement(comboBoxIAName);
                var comboBoxIAID = FindChild<ComboBox>(dataGridRow, "ComboBoxIAID");

                if (comboBoxIAID != null)
                {
                    comboBoxIAID.SelectedValue = selectedIA.AirportID;
                    BindingExpression bindingID = comboBoxIAID.GetBindingExpression(ComboBox.SelectedValueProperty);
                    if (bindingID != null)
                    {
                        bindingID.UpdateSource();
                    }
                }
            }
        }

        /*---------------------------------------------END Data Grid 2 aka IA---------------------------------------------------------*/
    }

    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && TimeSpan.TryParseExact(str, @"hh\:mm", CultureInfo.InvariantCulture, out var timeSpan))
            {
                return timeSpan;
            }
            return null;
        }
    }
}
