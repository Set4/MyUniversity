using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
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



namespace MyUniversity.WindowsPhone10
{
    public interface IAuthentificationPage
    {
     

        void ViewEmailError();
        void ViewPasswordError();
        void ViewLogInError();

        void NotNetworc();

        void ClearViewError();

        void Perehod(IAuthentificationModel _auth);
    }

    public sealed partial class AuthentificationPage : Page, IAuthentificationPage
    {
        private AuthentificationPresenter presenter;

     

        public AuthentificationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            presenter = new AuthentificationPresenter(this, e.Parameter as AuthentificationModel);

          
        }

        public void Perehod(IAuthentificationModel _auth)
        {
            Frame.Navigate(typeof(MainPage), _auth);
        }

        private void btnLogIn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            presenter.LogInEvent(txbEmail.Text.Trim(), txbPassword.Password.Trim());
        }




        public async void NotNetworc()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
               "Net soedinenia, poprobyite Authentification pozje");

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
