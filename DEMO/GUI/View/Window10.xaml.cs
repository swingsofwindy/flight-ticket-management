using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DTO;
using GUI.ViewModel;

namespace GUI.View
{
    public partial class Window10 : UserControl
    {   
        private ParameterDTO parameter = null;
        private ParameterDTO newParameter = null;
        private ObservableCollection<AirportDTO> airports = new ObservableCollection<AirportDTO>();
        private ObservableCollection<TicketClassDTO> ticketClassDTOs = new ObservableCollection<TicketClassDTO>();
        public Window10()
        {
            InitializeComponent();
            parameter = new BLL.SearchProcessor().GetParameterDTO();
            LoadData();
        }

        private void LoadData()
        {
            LoadParameter();
            airports = new ObservableCollection<AirportDTO>(new BLL.Airport_BLL().L_airport());
            ticketClassDTOs = new ObservableCollection<TicketClassDTO>(new BLL.Ticket_Class_BLL().L_TicketClass());
            ListAirport.ItemsSource = airports;
            ListTicketClass.ItemsSource = ticketClassDTOs;
        }

        private void LoadParameter()
        {
            if (parameter != null)
            {
                int numIAirports = parameter.IntermediateAirportCount;
                TimeSpan minFlightTime = parameter.MinFlighTime;
                TimeSpan minDownTime = parameter.MinStopTime;
                TimeSpan maxDownTime = parameter.MaxStopTime;
                TimeSpan lastBookTicket = parameter.SlowestBookingTime;
                TimeSpan lastCancel = parameter.CancelTime;

                MinFlightTime.SelectedTime = DateTime.Today.Add(minFlightTime);
                MinDownTime.SelectedTime = DateTime.Today.Add(minDownTime);
                MaxDownTime.SelectedTime = DateTime.Today.Add(maxDownTime);
                LastBookTicket.SelectedTime = DateTime.Today.Add(lastBookTicket);
                LastCancelTicket.SelectedTime = DateTime.Today.Add(lastCancel);
                maxInterAirportTextBox.Text = numIAirports.ToString();
            }
        }

        private void Button_Add_Airport(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Delete_Airport(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Edit_Airport(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Add_TC(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Delete_TC(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Edit_TC(object sender, RoutedEventArgs e)
        {

        }

        private void maxInterAirportTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }

        private void Button_Edit_main(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Collapsed;
            EditPanel.Visibility = Visibility.Visible;
            MinFlightTime.IsEnabled = true;
            MinDownTime.IsEnabled = true;
            MaxDownTime.IsEnabled = true;
            LastBookTicket.IsEnabled = true;
            LastCancelTicket.IsEnabled = true;
            maxInterAirportTextBox.IsEnabled = true;
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Visible;
            EditPanel.Visibility = Visibility.Collapsed;
            MinFlightTime.IsEnabled = false;
            MinDownTime.IsEnabled = false;
            MaxDownTime.IsEnabled = false;
            LastBookTicket.IsEnabled = false;
            LastCancelTicket.IsEnabled = false;
            maxInterAirportTextBox.IsEnabled = false;

            try
            {
                TimeSpan MinFlightTimeSpan = (MinFlightTime.SelectedTime != null) ? MinFlightTime.SelectedTime.Value.TimeOfDay : TimeSpan.Zero;
                TimeSpan MinDownTimeSpan = (MinDownTime.SelectedTime != null) ? MinDownTime.SelectedTime.Value.TimeOfDay : TimeSpan.Zero;
                TimeSpan MaxDownTimeSpan = (MaxDownTime.SelectedTime != null) ? MaxDownTime.SelectedTime.Value.TimeOfDay : TimeSpan.Zero;
                TimeSpan LastBookTicketSpan = (LastBookTicket.SelectedTime != null) ? LastBookTicket.SelectedTime.Value.TimeOfDay : TimeSpan.Zero;
                TimeSpan LastCancelTicketSpan = (LastCancelTicket.SelectedTime != null) ? LastCancelTicket.SelectedTime.Value.TimeOfDay : TimeSpan.Zero;
                int numIAirports = Convert.ToInt32(maxInterAirportTextBox.Text);

                newParameter = new ParameterDTO()
                {
                    IntermediateAirportCount = numIAirports,
                    CancelTime = LastCancelTicketSpan,
                    SlowestBookingTime = LastBookTicketSpan,
                    MinStopTime = MinDownTimeSpan,
                    MaxStopTime = MaxDownTimeSpan,
                    MinFlighTime = MinFlightTimeSpan,
                };

                

                BLL.UpdateDataProcessor prc = new BLL.UpdateDataProcessor();

                if (newParameter.IntermediateAirportCount < 0)
                {
                    MessageBox.Show("Number of Intermediate Airport can not be negative");
                    return;
                }

                if (newParameter.IntermediateAirportCount != parameter.IntermediateAirportCount)
                {
                        prc.UpdateIntermediateAirportCount(numIAirports);
                        //MessageBox.Show($"{numIAirports}");
                }

                if (newParameter.CancelTime != parameter.CancelTime)
                {   
                    prc.UpdateCancelTime(newParameter.CancelTime);
                    //MessageBox.Show("B");
                }

                if (newParameter.SlowestBookingTime != parameter.SlowestBookingTime)
                {   
                    prc.UpdateSlowestBookingTime(newParameter.SlowestBookingTime);
                    //MessageBox.Show("C");
                }

                if (newParameter.MinStopTime != parameter.MinStopTime)
                {
                    prc.UpdateMinStopTime(newParameter.MinStopTime);
                    //MessageBox.Show("D");
                }

                if (newParameter.MaxStopTime != parameter.MaxStopTime)
                {   
                    prc.UpdateMaxStopTime(newParameter.MaxStopTime);
                    //MessageBox.Show("E");
                }

                if (newParameter.MinFlighTime != parameter.MinFlighTime)
                {   prc.UpdateMinFligthTime(newParameter.MinFlighTime);
                    //MessageBox.Show("F");
                }

                parameter = newParameter;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}");
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Visible;
            EditPanel.Visibility = Visibility.Collapsed;
            MinFlightTime.IsEnabled = false;
            MinDownTime.IsEnabled = false;
            MaxDownTime.IsEnabled = false;
            LastBookTicket.IsEnabled = false;
            LastCancelTicket.IsEnabled = false;
            maxInterAirportTextBox.IsEnabled = false;

            LoadParameter();
        }
        static bool HasSpecialCharacters(string str)
        {
            // Regex pattern để kiểm tra ký tự đặc biệt (ngoại trừ khoảng trắng)
            string pattern = @"[^\w\sÀ-ỹ]"; // \w bao gồm chữ cái và số, \s là khoảng trắng, À-ỹ cho các ký tự tiếng Việt

            // Tạo đối tượng Regex với pattern
            Regex regex = new Regex(pattern);

            return regex.IsMatch(str);
        }

        static string FormatString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            string[] words = str.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            return string.Join(" ", words);
        }

            static bool PositiveIntegerChecking(string str)
            {
            // Biểu thức chính quy để kiểm tra ký tự đặc biệt
            Regex regex = new Regex("[^0-9.]");
            return regex.IsMatch(str);
            }



        private void Add_Airport_Click(object sender, RoutedEventArgs e)
        {
            if (HasSpecialCharacters(NewAirport.Text))
            {
                MessageBox.Show("Airport has special characters", "Error");
                return;
            }
            if (new BLL.Airport_BLL().isExist(FormatString(NewAirport.Text.ToString())))
            {
                MessageBox.Show("This airport has existed", "Error");
                return;
            }
            if (!string.IsNullOrWhiteSpace(NewAirport.Text))
            {
                BLL.Airport_BLL prc = new BLL.Airport_BLL();
                prc.insertAirport(FormatString(NewAirport.Text.ToString()));
                airports = new ObservableCollection<AirportDTO>(new BLL.Airport_BLL().L_airport());
                ListAirport.ItemsSource = airports;
                BLL.UpdateDataProcessor prc2 = new BLL.UpdateDataProcessor();
                prc2.UpdateAirportCount(airports.Count);
                NewAirport.Text = string.Empty;
            }
        }
        public string check()
        {
            string st = string.Empty;
            foreach(char c in NewMultiplier.Text.ToString())
            {
                if(char.IsLetter(c))
                {
                    return "Please re-enter the Multiplier";
                }
            }
            return st;
        }
        private void Add_Class_Click(object sender, RoutedEventArgs e)
        {
            if (HasSpecialCharacters(NewClassName.Text))
            {
                MessageBox.Show("Ticket class' name has special character");
                return;
            }
            if (PositiveIntegerChecking(NewMultiplier.Text))
            {
                MessageBox.Show("Ticket class'multiplier must be > 0");
                return;
            }
            if (!string.IsNullOrWhiteSpace(FormatString(NewClassName.Text)) && !string.IsNullOrWhiteSpace(NewMultiplier.Text))
            {
                string st = check();
                if(st != string.Empty)
                {
                    MessageBox.Show(st, "Error");
                    NewMultiplier.Text = string.Empty;
                    return;
                }
                BLL.Ticket_Class_BLL prc = new BLL.Ticket_Class_BLL();
                prc.InsertTicketClass(new TicketClassDTO() { TicketClassName = NewClassName.Text, BaseMultiplier = Convert.ToDecimal(NewMultiplier.Text.ToString())});
                ticketClassDTOs = new ObservableCollection<TicketClassDTO>(new BLL.Ticket_Class_BLL().L_TicketClass());
                ListTicketClass.ItemsSource = ticketClassDTOs;
                BLL.UpdateDataProcessor prc2 = new BLL.UpdateDataProcessor();
                prc2.UpdateTicketClassCount(ticketClassDTOs.Count);
                NewClassName.Text = string.Empty;
                NewMultiplier.Text = string.Empty;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(NewClassName.Text))
                {
                    MessageBox.Show("Please enter the ticket class", "Error");
                    return;
                }
                if(string.IsNullOrWhiteSpace(NewMultiplier.Text))
                {
                    MessageBox.Show("Please enter the multiplier of the ticket class", "Error");
                    return;
                }
            }
        }

        private void Delete_Airport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is AirportDTO airport)
            {
                
                BLL.Airport_BLL prc = new BLL.Airport_BLL();
                int result = prc.deleteAirport(airport.AirportID);
                if (result == 0)
                {
                    MessageBox.Show("Cannot Delete this Airport", "Error");
                }
                else
                {
                    airports.Remove(airport);
                    MessageBox.Show("Delete Successfully", "Success");
                }

            }
        }

        private void Delete_Class_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is TicketClassDTO ticketClass)
            {
                
                BLL.Ticket_Class_BLL prc = new BLL.Ticket_Class_BLL();
                int result = prc.DeleteTicketClass(ticketClass.TicketClassID);
                if (result == 0)
                {
                    MessageBox.Show("Cannot Delete this Ticket Class", "Error");
                }
                else
                {
                    ticketClassDTOs.Remove(ticketClass);
                    MessageBox.Show("Delete Successfully", "Success");
                }
            }
        }
    }
}
