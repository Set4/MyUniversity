using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
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



namespace MyUniversity.WindowsPhone10
{



    public interface IAuthentificationPage
    {
        void ViewEmailError();
        void ViewPasswordError();
        void ViewLogInError();

        void ViewError(string message);

        void ClearViewError();
        void EndAuth(Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> auth);
    }

    public sealed partial class AuthentificationPage : Page, IAuthentificationPage
    {
        // private AuthentificationPresenter presenter;

        private AuthPresenter presenter;

        public AuthentificationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // presenter = new AuthentificationPresenter(this, e.Parameter as AuthentificationModel);

            presenter = new AuthPresenter(this, new AuthentificationModel(App.platform, App.documentsPath));
        }

        public void EndAuth(Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> auth)
        {
            Frame.Navigate(typeof(MainPage), auth);
        
        }

        private async void btnLogIn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await presenter.LogInEvent(txbEmail.Text.Trim(), txbPassword.Password.Trim());
        }




        public async void ViewError(string message)
        {
                var dialog = new Windows.UI.Popups.MessageDialog(message,
                             "Net soedinenia");
                await dialog.ShowAsync();
        }

        public void ViewEmailError()
        {
            txblockErrorEmail.Text = "\u2219 Некорректный E-Mail";
        }

        public void ViewPasswordError()
        {
            txblockErrorPassword.Text = "\u2219 Некорректный Password";
        }

        public void ViewLogInError()
        {
            txblockErrorLogin.Text = "\u2219 Неверный Логин и/или Пароль.";
        }

        public void ClearViewError()
        {
            txblockErrorEmail.Text = txblockErrorPassword.Text = txblockErrorLogin.Text = String.Empty;
        }
    }
}
