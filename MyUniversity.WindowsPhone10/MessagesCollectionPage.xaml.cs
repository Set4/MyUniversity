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

   public interface IMessagesCollectionPage
    {
        void ViewNotifications(List<Notification> items);
        void ViewErrorNoNetwork();
        void ViewErrorAccountIncorrect();
    }

    public sealed partial class MessagesCollectionPage : Page, IMessagesCollectionPage
    {
        private MessagesModelPresenter presenter;

     

        AuthentificationModel acc;

        public MessagesCollectionPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            acc = e.Parameter as AuthentificationModel;

            presenter = new MessagesModelPresenter(this, new NotificationModel(e.Parameter as AuthentificationModel, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), ApplicationData.Current.LocalFolder.Path));

            presenter.GetNotifications();
        }



        public void ViewNotifications(List<Notification> items)
        {
            foreach (Notification item in items)
                listViewMessages.Items.Add(item);
        }

        public async void ViewErrorAccountIncorrect()
        {
            await Task.Factory.StartNew(async () =>
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
              "acc incorr");

                await dialog.ShowAsync();
                Frame.Navigate(typeof(AuthentificationPage), acc);
            });
        }
        public async void ViewErrorNoNetwork()
        {
            await Task.Factory.StartNew(async () =>
            {
                var dialog = new Windows.UI.Popups.MessageDialog(" Повторите попытку позже.", "Ошибка NEySEti");
                await dialog.ShowAsync();
            });
        }


        private void listViewMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewMessages.SelectedItem != null)
            {

                presenter.Readet(listViewMessages.SelectedItem as Notification);
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
            if (element.IsRead==false)
                return UnMessageTemplate;
            else
                return MessageTemplate;
        }
    }

}
