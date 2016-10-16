using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
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
   
   
    public sealed partial class ViewProfil : Page
    {

        //private ProfileModellPresenter presenter;
        // AuthentificationModel acc;
        AuthentificationModelLogOut _auth;

        public ViewProfil()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewProfile(e.Parameter as StydentProfile);

            _auth = new AuthentificationModelLogOut(App.platform, App.documentsPath);
          //  acc = e.Parameter as AuthentificationModel;
          //  presenter = new ProfileModellPresenter(this, new ProfileModel(e.Parameter as AuthentificationModel, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), ApplicationData.Current.LocalFolder.Path), new StorageServise());

            //  presenter.GetProfile();
        }

      

        public async void Logout()
        {//
           if(await _auth.LogOutAccaunt())
                CoreApplication.Exit();
           else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Повторите попытку выхода",
                           "Ошибка!");
                await dialog.ShowAsync();
            }
        }

        public void ViewProfile(StydentProfile prof)
        {

            txblock_Name.Text = prof.LastName + " " + prof.FierstName;
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
            Logout();
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
