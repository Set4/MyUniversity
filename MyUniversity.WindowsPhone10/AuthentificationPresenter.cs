using System;


using Windows.Storage;
using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.Сommon_Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MyUniversity.WindowsPhone10
{
    public class AuthentificationPresenter
    {
        public readonly IAuthentificationPage _view;
        public readonly IAuthentificationModel _model;

        public AuthentificationPresenter(AuthentificationPage view, AuthentificationModel model)
        {
            this._view = view;
            this._model = model;


         


            _model.IncorrectLoginANDPassword += _model_BadLoginANDPassData;

            _model.NoNetwork += _model_NoNetwork;

            _model.LoginSucsessfull += _model_LoginSucsessfull;
        }



        public void LogInEvent(string login, string pass)
        {
           

            _view.ClearViewError();

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

        private void _model_LoginSucsessfull(object sender, MessageEvent e)
        {
            _view.Perehod(_model);
        }

        private void _model_NoNetwork(object sender, MessageEvent e)
        {
            _view.NotNetworc();
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



        private void _model_BadLoginANDPassData(object sender, MessageEvent e)
        {
            _view.ClearViewError();
            _view.ViewLogInError();

        }



    

      
    }



}
