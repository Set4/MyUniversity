using MyUniversity.Core.AuthenticationModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.WindowsPhone10
{
    class AuthPresenter
    {
        readonly IAuthentificationPage _view;
        readonly IAuthentificationModel _model;

        public AuthPresenter(IAuthentificationPage view, IAuthentificationModel model)
        {
            this._view = view;
            this._model = model;

            _model.AccessToSiteProblem += _model_AccessToSiteProblem;
            _model.IncorrectAuthData += _model_IncorrectAuthData;
            _model.NoNetwork += _model_NoNetwork1;

        }

        private void _model_NoNetwork1(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewError(e.Message);
        }

        private void _model_IncorrectAuthData(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ClearViewError();
            _view.ViewError(e.Message);
        }

        private void _model_AccessToSiteProblem(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewError(e.Message);
        }



        public async Task LogInEvent(string login, string pass)
        {
            _view.ClearViewError();

            if (!await ValidationEmail(login))
            {
                _view.ViewEmailError();
                return;
            }
            if (String.IsNullOrWhiteSpace(pass))
            {
                _view.ViewPasswordError();
                return;
            }
            Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> authdata = await _model.LogInAccaunt(login, pass);
            if (authdata != null)
                _view.EndAuth(authdata);
            else
                _view.ViewLogInError();
        }




        /// <summary>
        /// Проверка правильности ввода E-mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<bool> ValidationEmail(string email)
        {
            bool result = false;
            try
            {
                result = await Task.Run(() =>
                {

                    if (Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)) && !string.IsNullOrEmpty(email))
                        return true;
                    else
                        return false;

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Validation e-mail Error: {0}", ex.Message);
            }

            return result;
        }





      

    }
}
