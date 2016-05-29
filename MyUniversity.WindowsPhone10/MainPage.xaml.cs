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

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyUniversity.WindowsPhone10
{
   public interface IViewMainPage
    {
        void ViewEmail();
    }
    public sealed partial class MainPage : Page,IViewMainPage
    {
        AuthentificationModel acc;
        string Header { get; set; }

        public MainPagePresenter _presenter;

  

        public MainPage()
        {
            this.InitializeComponent();

        
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            acc = e.Parameter as AuthentificationModel;

            _presenter = new MainPagePresenter(this, e.Parameter as AuthentificationModel);

            ViewEmail();

            txblHeader.Text = "Информация";

            frameMainPageContent.Navigate(typeof(InfoPage));

        }

        public void ViewEmail()
        {
            txblckNameProfile.Text = acc._Acc.Email;
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

















     

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Профиль";
          
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(ViewProfil), acc);
        }

    

        private void MessageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Уведомления";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(MessagesCollectionPage), acc);
        }

        private void btnBRS_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Успеваемость";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(LessonsPage), acc);
        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            txblHeader.Text = "Расписание";
            this.splViewMainMenu.IsPaneOpen = false;
            frameMainPageContent.Navigate(typeof(ShedulePage), acc);
        }
    }
}
