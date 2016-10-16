using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.NotificationModel;
using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


    public sealed partial class MessagesCollectionPage : Page
    {
        List<Notification> items;

        public MessagesCollectionPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            items = e.Parameter as List<Notification>;
            
            ViewNotifications(items);
        }



        public void ViewNotifications(List<Notification> items)
        {
            foreach (Notification item in items)
            {
                item.Date = "(" + item.Date + ")";
                listViewMessages.Items.Add(item);
            }
        }

        private void listViewMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(listViewMessages.SelectedItem!=null && listViewMessages.SelectedItem is Notification)
            { 
                Frame.Navigate(typeof(ViewMessage), listViewMessages.SelectedItem);
            }
               
        }
    }



    public class MessageSelector : DataTemplateSelector
    {
        public DataTemplate UnMessageTemplate { get; set; }
        public DataTemplate MessageTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var element = item as Notification;
            if (element.State==false)
                return UnMessageTemplate;
            else
                return MessageTemplate;
        }
    }

}
