using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    public interface IViewProfile
    {
        void ViewProfile(StydentProfile prof);

        void ViewErrorLogOut();

        void ViewErrorNoNetwork();
        void ViewErrorAccountIncorrect();

        void Logout();
    }
   
    public sealed partial class ViewProfil : Page, IViewProfile
    {

        private ProfileModellPresenter presenter;
        AuthentificationModel acc;
     
        public ViewProfil()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            acc = e.Parameter as AuthentificationModel;
            presenter = new ProfileModellPresenter(this, new ProfileModel(e.Parameter as AuthentificationModel, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), ApplicationData.Current.LocalFolder.Path), new StorageServise());

            presenter.GetProfile();
        }

      public async void ViewErrorLogOut()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(" Повторите попытку позже.", "Ошибка Выхода");

            await dialog.ShowAsync();
        }

        public async void ViewErrorNoNetwork()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(" Повторите попытку позже.","Ошибка NEySEti"       );

            await dialog.ShowAsync();
        }

        public async void ViewErrorAccountIncorrect()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
              "acc incorr");

            await dialog.ShowAsync();
            Frame.Navigate(typeof(AuthentificationPage), acc);
        }

        public  void Logout()
        {
         
            Frame.Navigate(typeof(AuthentificationPage), acc);
        }

        public void ViewProfile(StydentProfile prof)
        {

          
            txblock_EMail.Text = prof.Email;

            txblockGroup.Text = prof.GroupNumber;
            txblockDepartment.Text = prof.Department;
            txblockChair.Text = prof.Chair;
            txblockSpecialty.Text = prof.Specialty;
            txblockTrainingProfile.Text = prof.TrainingProfile;
            txblockModeofStudy.Text = prof.ModeofStudy;

          

        }

        private void btnLogOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            presenter.Logout();
        }

       

      


        private void btnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            fltEdit.Hide();
        }

        private void btnsave_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
        }

        private void fltEdit_Closed(object sender, object e)
        {
        }

        private void txblock_Name_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            fltEdit.ShowAt(btnflt);
        }
    }




   

   
}
