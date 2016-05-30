using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyUniversity.Core.AuthenticationModel;
using System.Text.RegularExpressions;

namespace MyUniversity.Android
{
  public  class AuthentificationPresenter
    {
         readonly IAuthentificationPage _view;
         readonly IAuthentificationModel _model;

        public AuthentificationPresenter(IAuthentificationPage view, IAuthentificationModel model)
        {
            this._view = view;
            this._model = model;




            _model.IncorrectLoginANDPassword += _model_IncorrectLoginANDPassword;


            _model.NoNetwork += _model_NoNetwork1;

            _model.LoginSucsessfull += _model_LoginSucsessfull1;
        }

        private void _model_LoginSucsessfull1(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.Navigation(_model);
        }

        private void _model_NoNetwork1(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.NotNetwork();
        }

        private void _model_IncorrectLoginANDPassword(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewLogInError();
        }

        public void LogInEvent(string login, string pass)
        {


           

            if (!ValidationEmail(login))
            {
                _view.ViewEmailError();
                return;
            }
            if (String.IsNullOrWhiteSpace(pass))
            {
                _view.ViewPasswordError();
                return;
            }

            _model.LogInAccaunt(login, pass);
        }

     

       



        /// <summary>
        /// Проверка правильности ввода E-mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool ValidationEmail(string email)
        {

            if (Regex.IsMatch(email,
              @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
              RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)) && !string.IsNullOrEmpty(email))
                return true;
            else
                return false;
        }



     



    }
}