using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Calendar = System.Windows.Controls.Calendar;
using CalendarMode = System.Windows.Controls.CalendarMode;
using CalendarModeChangedEventArgs = System.Windows.Controls.CalendarModeChangedEventArgs;
using DatePicker = System.Windows.Controls.DatePicker;
using DTO;
using GUI.ViewModel;
using System.Collections.ObjectModel;
using BLL;
using System.Resources;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Window5.xaml
    /// </summary>
    public partial class Window5 : UserControl
    {

        // -DTO : Đuôi cho object trong DTO để trao đổi/xử lí
        // -Data : Đuôi cho object để binding trong UI
        // byFlight: với mỗi chuyến bay, cần ID, số vé, doanh thu, tỉ lệ doanh thu của chuyến này trên tất cả chuyến được query ?
        // byMonth: với mỗi tháng, cần số lượng chuyến bay trong tháng, tỉ lệ doanh thu của tháng đó trên cả tất cả tháng được query ?

        private ObservableCollection<ReportByFlightData> reportsByFlightData = new ObservableCollection<ReportByFlightData>();
        private ObservableCollection<ReportByMonthData> reportsByMonthData = new ObservableCollection<ReportByMonthData>();
        public Window5()
        {
            InitializeComponent();

            //----------BEGIN-Test UI-----------------------------------------------------------------------------//
            List<ReportByFlightDTO> listReportByFlightDTO = new List<ReportByFlightDTO>()
            {
            };

            List<ReportByMonthDTO> listReportByMonthDTO = new List<ReportByMonthDTO>()
            {
            };

            reportsByFlightData = ReportByFlightData.ConvertListDTOToObservableCollectionData(listReportByFlightDTO);
            GridRP_Month.ItemsSource = reportsByFlightData;
            reportsByMonthData = ReportByMonthData.ConvertListDTOToObservableCollectionData(listReportByMonthDTO);
            GridRP_Year.ItemsSource = reportsByMonthData;
            TotalRevenue_Month.Text = ((Int64)0).ToString();
            TotalRevenue_Year.Text = ((Int64)0).ToString();
            //----------END-Test UI-----------------------------------------------------------------------------//

        }

        public string ValidateInputTabMonth()
        {
            if (Month_TabMonth.Text.ToString() == string.Empty)
            {
                return "Please enter a Month!";
            }
            else if(Year_TabMonth.Text.ToString() == string.Empty)
            {
                return "Please enter a Year!";
            } 
            if (Convert.ToInt32(Month_TabMonth.Text) <= 0 || Convert.ToInt32(Month_TabMonth.Text) > 12)
            {
                return "Please enter Month from 1 to 12";
            }
            if (Convert.ToInt32(Year_TabMonth.Text) < 0)
            {
                return "Year cant be negative";
            }
            if (Convert.ToInt32(Year_TabMonth.Text) >= DateTime.Now.Year + 1)
            {
                return "Year exceeds the current year (" + DateTime.Now.Year.ToString() + 1 + ")";
            }
            return string.Empty;
        }


        private void Search_TabMonth_Click(object sender, RoutedEventArgs e)
        {
            string state = string.Empty;
            state = ValidateInputTabMonth();

            if (state != string.Empty)
            {
                MessageBox.Show(state);
                return;

            }

            
            foreach (char c in Month_TabMonth.Text.ToString())
            {
                if(char.IsLetter(c))
                {
                    MessageBox.Show("Month error");
                    Month_TabMonth.Text = string.Empty;
                    return;
                }
            }
            foreach (char c in Year_TabMonth.Text.ToString())
            {
                if (char.IsLetter(c))
                {
                    MessageBox.Show("Year error");
                    Year_TabMonth.Text = string.Empty;
                    return;
                }
            }

            int month = Convert.ToInt32(Month_TabMonth.Text.ToString());
            int year = Convert.ToInt32(Year_TabMonth.Text.ToString());
            List<ReportByFlightDTO> listReportByFlightDTO = new List<ReportByFlightDTO>();
            int total = 0;

            BLL.ReportBLL prc = new ReportBLL();
            var result = prc.GetReportByFlightBLL(month, year);
            listReportByFlightDTO = result.reportByFlightDTOs;
            total = result.total;
            state = prc.GetState();

            

            if (state != string.Empty)
            {

                MessageBox.Show(state);
            }

            reportsByFlightData = ReportByFlightData.ConvertListDTOToObservableCollectionData(listReportByFlightDTO);
            GridRP_Month.ItemsSource = reportsByFlightData;
            TotalRevenue_Month.Text = total.ToString();
        }

        public string ValidateInputTabYear()
        {
            if (Year_TabYear.Text.ToString() == string.Empty)
            {
                return "Please enter a Year!";
            }
            if (Convert.ToInt32(Year_TabYear.Text) < 0)
            {
                return "Year cant be negative";
            }
            if (Convert.ToInt32(Year_TabYear.Text) >= DateTime.Now.Year + 1)
            {
                return "Year exceeds the current year (" + DateTime.Now.Year.ToString() + 1 + ")";
            }
            return string.Empty;
        }

        private void Search_TabYear_Click(object sender, RoutedEventArgs e)
        {
            string state = string.Empty;
            state = ValidateInputTabYear();

            if (state != string.Empty)
            {
                MessageBox.Show(state);
                return;
            }
            foreach (char c in Year_TabYear.Text.ToString())
            {
                if (char.IsLetter(c))
                {
                    MessageBox.Show("Year error");
                    Year_TabYear.Text = string.Empty;
                    return;
                }
            }
            int year = Convert.ToInt32(Year_TabYear.Text.ToString());

            List<ReportByMonthDTO> listReportByMonthDTO = new List<ReportByMonthDTO>();
            int total = 0;

            BLL.ReportBLL prc = new ReportBLL();
            var result = prc.GetReportByMonthDAL(year);
            listReportByMonthDTO = result.reportByMonthDTOs;
            total = result.total;
            state = prc.GetState();

            if (state != string.Empty)
            {
                MessageBox.Show(state);
            }

            reportsByMonthData = ReportByMonthData.ConvertListDTOToObservableCollectionData(listReportByMonthDTO);
            GridRP_Year.ItemsSource = reportsByMonthData;
            TotalRevenue_Year.Text = total.ToString();
        }

    }

    //------------BEGIN-Date Picker Custom----------------------------------------------------------------//
    // Một số chỉnh sửa để chỉ chọn tháng từ DatePicker trong WPF Design
    public static class GlobalMouseHandler
    {
        public static void Initialize()
        {
            EventManager.RegisterClassHandler(typeof(UIElement), UIElement.PreviewMouseDownEvent,
                new MouseButtonEventHandler(OnGlobalMouseDown), true);
        }

        private static void OnGlobalMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DatePicker))
            {
                DatePickerCalendar.CloseAllOpenDatePickers();
            }
        }
    }

    public class DatePickerCalendar
    {
        private static List<DatePicker> OpenDatePickers = new List<DatePicker>();

        public static void RegisterOpenDatePicker(DatePicker datePicker)
        {
            if (!OpenDatePickers.Contains(datePicker))
            {
                OpenDatePickers.Add(datePicker);
            }
        }

        public static void UnregisterOpenDatePicker(DatePicker datePicker)
        {
            if (OpenDatePickers.Contains(datePicker))
            {
                OpenDatePickers.Remove(datePicker);
            }
        }

        public static void CloseAllOpenDatePickers()
        {
            foreach (var datePicker in OpenDatePickers)
            {
                var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
                if (popup != null && popup.IsOpen)
                {
                    popup.IsOpen = false;
                }
            }
            OpenDatePickers.Clear();
        }

        public static readonly DependencyProperty IsMonthYearProperty =
            DependencyProperty.RegisterAttached("IsMonthYear", typeof(bool), typeof(DatePickerCalendar),
                                                new PropertyMetadata(OnIsMonthYearChanged));

        public static bool GetIsMonthYear(DependencyObject dobj)
        {
            return (bool)dobj.GetValue(IsMonthYearProperty);
        }

        public static void SetIsMonthYear(DependencyObject dobj, bool value)
        {
            dobj.SetValue(IsMonthYearProperty, value);
        }

        private static void OnIsMonthYearChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            Application.Current.Dispatcher
                .BeginInvoke(DispatcherPriority.Loaded,
                             new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetCalendarEventHandlers),
                             datePicker, e);
        }

        private static void SetCalendarEventHandlers(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            if ((bool)e.NewValue)
            {
                datePicker.CalendarOpened += DatePickerOnCalendarOpened;
                datePicker.CalendarClosed += DatePickerOnCalendarClosed;
            }
            else
            {
                datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
                datePicker.CalendarClosed -= DatePickerOnCalendarClosed;
            }
        }

        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            RegisterOpenDatePicker(sender as DatePicker);
            var calendar = GetDatePickerCalendar(sender);
            calendar.DisplayMode = CalendarMode.Year;

            calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
        }

        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            RegisterOpenDatePicker(sender as DatePicker);
            var datePicker = (DatePicker)sender;
            var calendar = GetDatePickerCalendar(sender);
            datePicker.SelectedDate = calendar.SelectedDate;

            calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
        }

        private static void CalendarOnDisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            if (calendar.DisplayMode != CalendarMode.Month)
                return;

            calendar.SelectedDate = GetSelectedCalendarDate(calendar.DisplayDate);

            var datePicker = GetCalendarsDatePicker(calendar);
            datePicker.IsDropDownOpen = false;
        }

        private static Calendar GetDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
            return ((Calendar)popup.Child);
        }

        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            var parent = (FrameworkElement)child.Parent;
            if (parent.Name == "PART_Root")
                return (DatePicker)parent.TemplatedParent;
            return GetCalendarsDatePicker(parent);
        }

        private static DateTime? GetSelectedCalendarDate(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue)
                return null;
            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }
    }

    public class DatePickerDateFormat
    {
        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.RegisterAttached("DateFormat", typeof(string), typeof(DatePickerDateFormat),
                                                new PropertyMetadata(OnDateFormatChanged));

        public static string GetDateFormat(DependencyObject dobj)
        {
            return (string)dobj.GetValue(DateFormatProperty);
        }

        public static void SetDateFormat(DependencyObject dobj, string value)
        {
            dobj.SetValue(DateFormatProperty, value);
        }

        private static void OnDateFormatChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded, new Action<DatePicker>(ApplyDateFormat), datePicker);
        }
        private static void ApplyDateFormat(DatePicker datePicker)
        {
            var binding = new Binding("SelectedDate")
            {
                RelativeSource = new RelativeSource { AncestorType = typeof(DatePicker) },
                Converter = new DatePickerDateTimeConverter(),
                ConverterParameter = new Tuple<DatePicker, string>(datePicker, GetDateFormat(datePicker)),
                StringFormat = GetDateFormat(datePicker)
            };

            var textBox = GetTemplateTextBox(datePicker);
            textBox.SetBinding(TextBox.TextProperty, binding);

            textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;

            var dropDownButton = GetTemplateButton(datePicker);

            datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
            datePicker.CalendarOpened += DatePickerOnCalendarOpened;

            dropDownButton.PreviewMouseUp -= DropDownButtonPreviewMouseUp;
            dropDownButton.PreviewMouseUp += DropDownButtonPreviewMouseUp;
        }

        private static ButtonBase GetTemplateButton(DatePicker datePicker)
        {
            return (ButtonBase)datePicker.Template.FindName("PART_Button", datePicker);
        }

        private static void DropDownButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var datePicker = fe.TryFindParent<DatePicker>();
            if (datePicker == null || datePicker.SelectedDate == null) return;

            var dropDownButton = GetTemplateButton(datePicker);

            if (e.OriginalSource == dropDownButton && datePicker.IsDropDownOpen == false)
            {
                datePicker.SetCurrentValue(DatePicker.IsDropDownOpenProperty, true);

                datePicker.SetCurrentValue(DatePicker.DisplayDateProperty, datePicker.SelectedDate.Value);

                dropDownButton.ReleaseMouseCapture();

                e.Handled = true;
            }
        }



        private static TextBox GetTemplateTextBox(Control control)
        {
            control.ApplyTemplate();
            return (TextBox)control?.Template?.FindName("PART_TextBox", control);
        }

        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;

            e.Handled = true;

            var textBox = (TextBox)sender;
            var datePicker = (DatePicker)textBox.TemplatedParent;
            var dateStr = textBox.Text;
            var formatStr = GetDateFormat(datePicker);
            datePicker.SelectedDate = DatePickerDateTimeConverter.StringToDateTime(datePicker, formatStr, dateStr);
        }

        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs e)
        {
            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
        }

        private class DatePickerDateTimeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var formatStr = ((Tuple<DatePicker, string>)parameter).Item2;
                var selectedDate = (DateTime?)value;
                return DateTimeToString(formatStr, selectedDate);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var tupleParam = ((Tuple<DatePicker, string>)parameter);
                var dateStr = (string)value;
                return StringToDateTime(tupleParam.Item1, tupleParam.Item2, dateStr);
            }

            public static string DateTimeToString(string formatStr, DateTime? selectedDate)
            {
                return selectedDate.HasValue ? selectedDate.Value.ToString(formatStr) : null;
            }

            public static DateTime? StringToDateTime(DatePicker datePicker, string formatStr, string dateStr)
            {
                DateTime date;
                var canParse = DateTime.TryParseExact(dateStr, formatStr, CultureInfo.CurrentCulture,
                                                      DateTimeStyles.None, out date);

                if (!canParse)
                    canParse = DateTime.TryParse(dateStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                return canParse ? date : datePicker.SelectedDate;
            }


        }

    }

    public static class FEExten
    {
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject parentObject = GetParentObject(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return TryFindParent<T>(parentObject);
            }
        }
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            return VisualTreeHelper.GetParent(child);
        }
    }

    //------------END-Date Picker Custom----------------------------------------------------------------//
}
