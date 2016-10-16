using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MyUniversity.Core;
using MyUniversity.Core.AuthenticationModel;

using Windows.Storage;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.Сommon_Code;
using Windows.UI.Popups;
using MyUniversity.Core.Model;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyUniversity.WindowsPhone10
{
   public interface IViewMainPage
    {
        void ViewHeaderInformation(string text);
        void ViewMessage(string text);
    }
    public sealed partial class MainPage : Page,IViewMainPage
    {
        public MainPagePresenter _presenter;
        StydentProfile _prof;



        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> _authdata = e.Parameter as Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>;
           _presenter = new MainPagePresenter(this, new MainModel(App.platform, App.documentsPath, new StorageServise(), _authdata));

            _prof = await _presenter.GetProfile();
           ViewHeaderInformation(_prof.LastName + " " + _prof.FierstName);
            frameMainPageContent.Navigate(typeof(InfoPage));
        }

        public void ViewHeaderInformation(string text)
        {
            txblckNameProfile.Text = text;
        }

       public async void ViewMessage(string text)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(text, "Внимание!");
            await dialog.ShowAsync();
        }









        private void btnMainMenu_Tapped(object sender, TappedRoutedEventArgs e)
        {        
            if (!this.splViewMainMenu.IsPaneOpen)
            {
               
                this.splViewMainMenu.IsPaneOpen = true;
            }
        }

        private  void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.splViewMainMenu.IsPaneOpen = false;
        }



        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Профиль";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(ViewProfil), await _presenter.GetProfile());
        }

    

        private async void MessageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Уведомления";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(MessagesCollectionPage), await _presenter.GetNotification());
        }

        private async void btnBRS_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Успеваемость";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(LessonsPage), await _presenter.GetLessons());
        }

        private async void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Расписание";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(ShedulePage), await _presenter.GetSchedulse());
        }

        private void Button_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Информация";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(InfoPage));
        }
    }
}
