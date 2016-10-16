
using MyUniversity.Core.AuthenticationModel;

using MyUniversity.Core.ScheduleModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyUniversity.WindowsPhone10
{
    public class DayData
    {

        public DateTime Date { get; private set; }


        /////// <summary>
        /////// дата число.месяц
        /////// </summary>
       public string DateCustom
        {
            get
            {
                return Date.Day.ToString() + " " + new System.Globalization.CultureInfo("ru-RU").DateTimeFormat.GetMonthName(Date.Month).ToString().ToUpper().Substring(0, 3);
            }
        }

        public string DayWeekCustom
        {
            get
            {
                return new System.Globalization.CultureInfo("ru-RU").DateTimeFormat.GetDayName(Date.DayOfWeek).ToString().ToUpper();
            }
        }

        public int EvenWeek { get; private set; }

        public int DayWeek
        {
            get
            {
                return (int)Date.DayOfWeek;
            }
        }

        public TimeSpan Time { get; private set; }

        public string StartTime
        {
            get
            {
                if (Time.Hours == 0 && Time.Minutes == 0 && Time.Milliseconds == 0)
                    return "Нет занятий";
                else
                    return Time.Hours.ToString("hh") + ":" + Time.Minutes.ToString("mm");
            }
        }

        public DayData(DateTime date, int evenWeek, TimeSpan time)
        {
            this.Date = date;
            this.EvenWeek = evenWeek;
            this.Time = time;
        }

    }

    public class PivotItemsView
    {
        public string Header { get; set; }
        object content;
        public object Content
        {
            get
            {
                return content;
            }
            set
            {
                if (value is List<ScheduleItem> && (value as List<ScheduleItem>).Count == 0)
                    content = new List<string>() { "Нет занятий"};
                else
                    content = value;
            }
        }
    }

    public sealed partial class ShedulePage : Page
    {
 
        Tuple<List<WeekData>, List<ScheduleItem>> schedules;

        bool IsWeekView = true;
     
        public ShedulePage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           schedules = e.Parameter as Tuple<List<WeekData>, List<ScheduleItem>>;

            ViewSchedule();
         
        }





        public void ViewSchedule()
        {
            List<ScheduleItem> itemsschedule;
            TimeSpan time;

            pivotShedule.Items.Clear();
            if (IsWeekView)
                foreach (WeekData item in schedules.Item1)
                {
                    List<DayData> days = new List<DayData>();
                    foreach (DateTime date in item.DaysInWeek)
                    {
                        List<ScheduleItem> itemsch = schedules.Item2.Where<ScheduleItem>((i) =>
                          (i.DayWeek == (int)date.DayOfWeek && i.EvenWeek == item.IsEvenWeek) || i.Date == date.ToString()).ToList<ScheduleItem>();

                        if (itemsch.Count>0)
                        {
                            var maxTimeStart = itemsch.Max(obj => obj.TimeStart).FirstOrDefault().ToString();
                            time = TimeSpan.ParseExact(maxTimeStart, "g", CultureInfo.CurrentCulture);
                            days.Add(new DayData(date, item.IsEvenWeek, time));
                        }

                        days.Add(new DayData(date, item.IsEvenWeek, new TimeSpan(0,0,0)));
                    }
                    pivotShedule.Items.Add(new PivotItemsView() { Header = item.Key + " НЕДЕЛЯ", Content = days });//pivotShedule.Items.Add(new PivotItem() { Header = , Content  });// 
                }
            else
                foreach (WeekData item in schedules.Item1)
                {

                    List<DayData> days = new List<DayData>();
                    foreach (DateTime date in item.DaysInWeek)
                    {
                        time = new TimeSpan(0, 0, 0);//zaglushka
                        days.Add(new DayData(date, item.IsEvenWeek, time));
                    }
                    foreach (DayData itemday in days)
                    {
                        itemsschedule = schedules.Item2.Where<ScheduleItem>((i) =>
                            (i.DayWeek == itemday.DayWeek && i.EvenWeek == itemday.EvenWeek) || i.Date == itemday.Date.ToString()).ToList<ScheduleItem>();

                        //  if (itemsschedule.Count < 1)

                        pivotShedule.Items.Add(new PivotItemsView()
                        {
                            Header = itemday.DateCustom + " " + itemday.DayWeekCustom,
                            Content = itemsschedule
                        });
                    }
                }


        }



        private void pivotShedule_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (IsWeekView)
                IsWeekView = false;
            else
                IsWeekView = true;

            ViewSchedule();
        }
    


        private void listboxSheduleItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedday=0;
            List<ScheduleItem> itemsschedule;
            TimeSpan time;
            DayData itemday;

            ListBox listbox = sender as ListBox;

            if(listbox.SelectedItem!=null &&listbox.SelectedItem is DayData)
            {
                itemday = listbox.SelectedItem as DayData;
                pivotShedule.Items.Clear();
                foreach (WeekData item in schedules.Item1)
                {

                    List<DayData> days = new List<DayData>();
                    foreach (DateTime date in item.DaysInWeek)
                    {
                        time = new TimeSpan(0, 0, 0);//zaglushka
                        days.Add(new DayData(date, item.IsEvenWeek, time));
                    }
                    for (int position = 0; position < days.Count; position++)
                    {

                        if (days[position].Date == itemday.Date)
                            selectedday = position;
                        // foreach (DayData itemday in days)

                        itemsschedule = schedules.Item2.Where<ScheduleItem>((i) =>
                            (i.DayWeek == days[position].DayWeek && i.EvenWeek == days[position].EvenWeek) || i.Date == days[position].Date.ToString()).ToList<ScheduleItem>();

                        //  if (itemsschedule.Count < 1)

                        pivotShedule.Items.Add(new PivotItemsView()
                        {
                            Header = days[position].DateCustom + " " + days[position].DayWeekCustom,
                            Content = itemsschedule
                        });

                    }
                 
                }

                   pivotShedule.SelectedIndex = selectedday;
            }
        }

       
    }


    public class ItemsSelector : DataTemplateSelector
    {
        
        public DataTemplate DayDataItemsTemplate { get; set; }
        public DataTemplate SheduleItemsTenplate { get; set; }
        public DataTemplate SheduleEmptyItemsTenplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is DayData)
                return DayDataItemsTemplate;
            else if (item is ScheduleItem)
                return SheduleItemsTenplate;
            else
                return SheduleEmptyItemsTenplate;

        }
    }
}
