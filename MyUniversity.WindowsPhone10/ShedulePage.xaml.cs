
using MyUniversity.Core.AuthenticationModel;

using MyUniversity.Core.ScheduleModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public interface IShedulePage
    {
        void ViewErrorNoNetwork();
        void ViewErrorAccountIncorrect();
        void SetSchedule(List<WeekData> items);
        void ViewSchedule();
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

    public sealed partial class ShedulePage : Page, IShedulePage
    {
        List<WeekData> _items;


        bool IsWeekView = true;
        private ShedulePresenter presenter;
    

        AuthentificationModel acc;
        public ShedulePage()
        {
            this.InitializeComponent();
            _items = new List<WeekData>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            acc = e.Parameter as AuthentificationModel;
            presenter = new ShedulePresenter(this, new ScheduleModel(e.Parameter as AuthentificationModel, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), ApplicationData.Current.LocalFolder.Path));

            presenter.GetWeeks();
        }


        public async void ViewErrorNoNetwork()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(" Повторите попытку позже.", "Ошибка NEySEti");

            await dialog.ShowAsync();
        }

        public async void ViewErrorAccountIncorrect()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
              "acc incorr");

            await dialog.ShowAsync();
            Frame.Navigate(typeof(AuthentificationPage), acc);
        }


        public void SetSchedule(List<WeekData> items)
        {
            _items = items;
        }

        public void ViewSchedule()
        {
           

          
            pivotShedule.Items.Clear();
            if (IsWeekView)
                foreach (WeekData w in _items)
                    pivotShedule.Items.Add(new PivotItemsView() { Header = w.Key + " НЕДЕЛЯ", Content = w.Days }); //pivotShedule.Items.Add(new PivotItem() { Header = , Content  });// 
            else
                foreach (WeekData w in _items)
                  foreach (DayData d in w.Days)
                       pivotShedule.Items.Add(new PivotItemsView() { Header = d.Date + " " + d.DayWeek , Content = d.Schedules}); //
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
