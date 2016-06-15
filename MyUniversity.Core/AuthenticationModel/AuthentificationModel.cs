using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.AuthenticationModel
{
    /// <summary>
    /// класс представления аккаунта в программе
    /// </summary>
   public class Account
    {
        private string email = null;
        public string Email
        {
            get { return email; }
            private set { value = email; }
        }

        private string password = null;
        public string Password
        {
            get { return password; }
            private set { value = password; }
        }

        private string requestVerificationToken = null;
        public string RequestVerificationToken
        {
            get { return requestVerificationToken; }
            private set { value = requestVerificationToken; }
        }

        private string aspxauthtToken = null;
        public string ASPXAUTHToken
        {
            get { return aspxauthtToken; }
            private set { value = aspxauthtToken; }
        }



        public Account(List<StorageAccount> items)
        {
            StorageAccount _stp = null;

            _stp = items.Where(i => i.Key == "UserName").FirstOrDefault();
            if (_stp != null)
                email = _stp.Value;
            else
                email = String.Empty;

            _stp = items.Where(i => i.Key == "Password").FirstOrDefault();
            if (_stp != null)
                password = _stp.Value;
            else
                password = String.Empty;

            _stp = items.Where(i => i.Key == "__RequestVerificationToken").FirstOrDefault();
            if (_stp != null)
                requestVerificationToken = _stp.Value;
            else
                requestVerificationToken = String.Empty;

            _stp = items.Where(i => i.Key == ".ASPXAUTH").FirstOrDefault();
            if (_stp != null)
                aspxauthtToken = _stp.Value;
            else
                aspxauthtToken = String.Empty;
        }

        public void Update(string email, string pass, string token1, string token2)
        {
            this.email = email;
            password = pass;
            requestVerificationToken = token1;
            aspxauthtToken = token2;
        }

        public bool AccCelii()
        {
            if (Email != String.Empty && Password != String.Empty && RequestVerificationToken != String.Empty && ASPXAUTHToken != String.Empty)
                return true;
            else
                return false;
        }

    }


    /// <summary>
    /// класс для сохранения данных аккаунта в бд
    /// </summary>
    public class StorageAccount : stringKey
    {
        [PrimaryKey, MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(200)]
        public string Value { get; set; }
    }


    
    public interface IAuthentificationModel
    {
        
        event EventHandler<MessageEvent> IncorrectLoginANDPassword;
         event EventHandler<MessageEvent> LoginSucsessfull;
         event EventHandler<MessageEvent> NoNetwork;

        Task<Account> LoadAccount();
        void LogInAccaunt(string login, string password);

        Task<bool> LogOutAccaunt();
    }




  public  class AuthentificationModel: IAuthentificationModel
    {

        private Account accaunt = null;

       public  Account _Acc
        {
            get { return accaunt; }
            private set { value = accaunt; }
        }


        HttpProvider _http;
     
        SQLiteService _storage;

        //IStorageProvider _storage;

        
        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;


    
        public AuthentificationModel(ISQLitePlatform platform, string documentsPath)
        {
            _storage = new SQLiteService(platform, documentsPath);

            _http = new HttpProvider();

            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated;
            _http.PostDataisIncorrect += _http_PostDataisIncorrect;
        }

        private void _http_PostDataisIncorrect(object sender, MessageEvent e)
        {
            IncorrectLoginANDPassword(this, new MessageEvent("BadLoginANDPass"));
        }

        private void _http_AuthenticationTokensOutdated(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("site problem"));
        }

        private void _http_ResponseImpossibleTo(object sender, MessageEvent e)
        {
                  NoNetwork(this, new MessageEvent("NoNetwork"));
        }
        


        public event EventHandler<MessageEvent> AccauntLoadet = delegate { };

        public event EventHandler<MessageEvent> AccauntNotLoadet = delegate { };



        public event EventHandler<MessageEvent> IncorrectLoginANDPassword = delegate { };

        public event EventHandler<MessageEvent> LoginSucsessfull = delegate { };

        public event EventHandler<MessageEvent> NoNetwork = delegate { };



        public event EventHandler<MessageEvent> LogoutSucsessfull = delegate { };
        public event EventHandler<MessageEvent> LogoutError = delegate { };


        public async Task<Account> LoadAccount()
        {
            List<StorageAccount> _acc = await ReadAccount();
            if (_acc != null)
            accaunt = new Account(_acc);

            if (accaunt != null && accaunt.AccCelii())
            {
                AccauntLoadet(this, new MessageEvent("polzovatel vhodil", accaunt));
                return _Acc;
            }
            else
            {
                AccauntNotLoadet(this, new MessageEvent("polzovatel ne vhodil", null));
                return null;
            }
        }




        public async void LogInAccaunt(string login, string password)
        {

            string token1 = ParseRequestVerificationToken(await GetRequestVerificationToken());

            if (String.IsNullOrWhiteSpace(token1))
            {
                IncorrectLoginANDPassword(this, new MessageEvent("RequestVerificationToken ne polychen"));
            }
            else
            {
                var values = new Dictionary<string, string>();
                FormUrlEncodedContent content = null;


                values.Add("__RequestVerificationToken", token1);
                values.Add("UserType", "2");
                values.Add("UserName", login);
                values.Add("Password", password);
                values.Add("RememberMe", "true");
                content = new FormUrlEncodedContent(values);

                string token2 = ParseASPXAUTHToken(await GetASPXAUTHToken(content));

                if (String.IsNullOrWhiteSpace(token2))
                {
                    IncorrectLoginANDPassword(this, new MessageEvent("BadLoginANDPass"));
                }
                else
                {
                    List<StorageAccount> items = new List<StorageAccount>() {
                    new StorageAccount() { Key ="UserName", Value =login},
                    new StorageAccount() { Key ="Password", Value =password},
                    new StorageAccount() { Key ="__RequestVerificationToken", Value =token1},
                    new StorageAccount() { Key =".ASPXAUTH", Value =token2} };
                    await UpdateAccount(items);

                    accaunt.Update(login, password, token1, token2);

                    LoginSucsessfull(this, new MessageEvent("logined", accaunt));

                }
            }

        }


        public async Task<bool> LogOutAccaunt()
        {
            var values = new Dictionary<string, string>();
            FormUrlEncodedContent content = null;
            values.Add("__RequestVerificationToken", await GetRequestVerificationToken());

            using (HttpResponseMessage response = await _http.HttpMethodPost("/Account/Login", content))
            {
                if (response != null)
                {
                    DeleteAccount();
                    LogoutSucsessfull(this, new MessageEvent("Exit to accaunt"));
                    return true;
                }
                else
                {
                    LogoutError(this, new MessageEvent("Exit to accaunt error"));
                    return false;
                }
            }
        }






        private async Task UpdateAccount(List<StorageAccount> items)
        {
            foreach (StorageAccount item in items)
                await _storage.InsertOrReplaceItem<StorageAccount>(item);
        }

        private async Task<List<StorageAccount>> ReadAccount()
        {
            return  await _storage.GetAllItems<StorageAccount>();
        }

        private async void DeleteAccount()
        {
            if(accaunt!=null)
            {
                await _storage.DeleteItem<StorageAccount>(new StorageAccount() { Key = "UserName", Value = accaunt.Email });
                await _storage.DeleteItem<StorageAccount>(new StorageAccount() { Key = "Password", Value = accaunt.Password });
                await _storage.DeleteItem<StorageAccount>(new StorageAccount() { Key = "__RequestVerificationToken", Value = accaunt.RequestVerificationToken });
                await _storage.DeleteItem<StorageAccount>(new StorageAccount() { Key = ".ASPXAUTH", Value = accaunt.ASPXAUTHToken });
            }
        }


        private string ParseASPXAUTHToken(string response)
        {
            Regex reg = new Regex("(ASPXAUTH=)(.*?);", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Regex reg1 = new Regex("(=)(.*?);", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            string result = reg1.Match(reg.Match(response).Value).Value;
            if (result.Length != 0)
                return result.Substring(1, result.Length - 2);
            else
                return String.Empty;
        }

        private string ParseRequestVerificationToken(string response)
        {

            Regex reg = new Regex("<input.*? name=[\"]*__RequestVerificationToken[\"] *.*?>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match imagesourse = reg.Match(response);

            reg = new Regex("[^`\"]*[^`\"]", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection t = reg.Matches(imagesourse.Value);
            if (t.Count > 5)
                return t[5].Value;
            else
                return String.Empty;
        }


        private async Task<string> GetRequestVerificationToken()
        {
            using (HttpResponseMessage response = await _http.HttpMethodGet("/Account/Login"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return null;
            }
        }

        private async Task<string> GetASPXAUTHToken(FormUrlEncodedContent content)
        {
            using (HttpResponseMessage response = await _http.HttpMethodPost("/Account/Login", content))
            {
                if (response != null)
                    return  response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                else
                    return null;
            }
        }


    }
}
