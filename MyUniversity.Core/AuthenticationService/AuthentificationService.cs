using MyUniversity.Core.Сommon_Code;
using MyUniversity.Core.СommonCode;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.AuthenticationModel
{
    public interface IAuthentificationModelLogOut
    {
        Task<bool> LogOutAccaunt();
    }

    public class AuthentificationModelLogOut : IAuthentificationModelLogOut
    {

        HttpProvider _http;
        AuthentificationService _authservice;
        ISQLitePlatform _platform;
        string _documentsPath;

        public AuthentificationModelLogOut(ISQLitePlatform platform, string documentsPath)
        {
            _http = new HttpProvider(new Uri("http://e.kgeu.ru"));

            _platform = platform;
            _documentsPath = documentsPath;
            _authservice = new AuthentificationService(_http, _platform, _documentsPath);
        }

        public async Task<bool> LogOutAccaunt()
        {
            await StartModel.StartService.LogOUT(_platform, _documentsPath);
            return await _authservice.LogOut();
        }
    }


    public interface IAuthentificationModel
    {

        event EventHandler<MessageEvent> IncorrectAuthData;
        event EventHandler<MessageEvent> NoNetwork;
        event EventHandler<MessageEvent> AccessToSiteProblem;


        Task<Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>> LogInAccaunt(string username, string password);


    }



    public class AuthentificationModel:IAuthentificationModel
    {
        HttpProvider _http;
        AuthentificationService _authservice;
       public AuthentificationModel(ISQLitePlatform platform, string documentsPath)
        {
            _http = new HttpProvider(new Uri("http://e.kgeu.ru"));


            _authservice = new AuthentificationService(_http, platform, documentsPath);

            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo;
            _http.PostDataisIncorrect += _http_PostDataisIncorrect;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated;
        }

        public event EventHandler<MessageEvent> IncorrectAuthData = delegate { };
        public event EventHandler<MessageEvent> NoNetwork = delegate { };
        public event EventHandler<MessageEvent> AccessToSiteProblem = delegate { };

        //nezvestnaia oshibka na saite
        private void _http_AuthenticationTokensOutdated(object sender, MessageEvent e)
        {
            AccessToSiteProblem(this, new MessageEvent("Сайт http://e.kgeu.ru не доступен."));
        }

        //nevernii login pass token1
        private void _http_PostDataisIncorrect(object sender, MessageEvent e)
        {
            IncorrectAuthData(this, new MessageEvent("Неверные данный: логин и/или пароль."));
        }

        //no netvork
        private void _http_ResponseImpossibleTo(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("Нет соединения."));
        }


        public async Task<Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>> LogInAccaunt(string username, string password)
        {
            Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> authdata = await _authservice.LogIn(username, password);
            if ( authdata != null)
                return authdata;
            else
                return null;
        }


      
    }


    class AuthentificationService
    {

      //  HttpProvider _httpprovider;
      //  IStorageServise _storage;

        AuthentificationParse _authparse;
        AuthentificationHttp _authhttp;
        AuthentificationStorage _authstorage;



        public AuthentificationService(HttpProvider http, ISQLitePlatform platform, string documentsPath)
        {
            _authhttp = new AuthentificationHttp(http);
            _authparse = new AuthentificationParse(1);
            _authstorage = new AuthentificationStorage(platform,documentsPath);
        }

     

        public async Task<Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>> LogIn(string username, string password)
        {
           
            string response = await _authhttp.GetRequestVerificationToken();
            if (String.IsNullOrWhiteSpace(response))
            {
                Debug.WriteLine("LogIn Error: {0}");
                return null;
            }
            string token1 = await _authparse.ParseRequestVerificationToken(response);
            if (String.IsNullOrWhiteSpace(token1))
            {
                Debug.WriteLine("LogIn Error: {0}");
                return null;
            }
            else
            {
                var values = new Dictionary<string, string>();
                FormUrlEncodedContent content = null;


                values.Add("__RequestVerificationToken", token1);
                values.Add("UserType", "2");
                values.Add("UserName", username);
                values.Add("Password", password);
                values.Add("RememberMe", "true");
                content = new FormUrlEncodedContent(values);

                response = await _authhttp.GetASPXAUTHToken(content);
                if (String.IsNullOrWhiteSpace(response))
                {
                    Debug.WriteLine("LogIn Error: {0}");
                    return null;
                }
                string token2 = await _authparse.ParseASPXAUTHToken(response);

                if (String.IsNullOrWhiteSpace(token2))
                {
                    Debug.WriteLine("LogIn Error: {0}");
                    return null;
                }
                else
                {
                    List<StorageAccount> items = new List<StorageAccount>() {
                    new StorageAccount() { Key ="UserName", Value =username},
                    new StorageAccount() { Key ="Password", Value =password},
                    new StorageAccount() { Key ="__RequestVerificationToken", Value =token1},
                    new StorageAccount() { Key =".ASPXAUTH", Value =token2} };


                    await _authstorage.SaveAccount(items);



                    return new Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>(new StorageAccount() { Key = "UserName", Value = username },
                       new StorageAccount() { Key = "Password", Value = password },
                       new StorageAccount() { Key = "__RequestVerificationToken", Value = token1 },
                       new StorageAccount() { Key = ".ASPXAUTH", Value = token2 });

                }
            }
   
        }

        public async Task<bool> LogOut()
        {
           await _authstorage.DeleteAccount();
            return true;
        }

        public async Task<Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>> LoadAutharizationData()
        {


            List<StorageAccount> items = await _authstorage.ReadAccount();
            StorageAccount _stp = null;

            _stp = items.Where(i => i.Key == "UserName").FirstOrDefault();



            StorageAccount _stp1 = null;
            _stp1 = items.Where(i => i.Key == "Password").FirstOrDefault();

            StorageAccount _stp2 = null;
            _stp2 = items.Where(i => i.Key == "__RequestVerificationToken").FirstOrDefault();

            StorageAccount _stp3 = null;
            _stp3 = items.Where(i => i.Key == ".ASPXAUTH").FirstOrDefault();

            if (_stp != null && _stp1 != null && _stp2 != null && _stp3 != null)
                return new Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>(_stp, _stp1, _stp2, _stp3);
            else
                return null;
        }


      
    }

    class AuthentificationParse
    {
        /// <summary>
        /// Время ожидания операции поиска Токенов
        /// </summary>
        double _timewait;

        public AuthentificationParse(double timewait)
        {
            _timewait = timewait;
        }





        public async Task<string> ParseASPXAUTHToken(string response)
        {
            string token = null;
            try
            {
                token = await Task.Run(() =>
                {
                    Regex reg = new Regex("(ASPXAUTH=)(.*?);", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Regex reg1 = new Regex("(=)(.*?);", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    string result = reg1.Match(reg.Match(response).Value).Value;
                    if (result.Length != 0)
                        return result.Substring(1, result.Length - 2);
                    else
                        return String.Empty;

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse ParseASPXAUTHToken Error: {0}", ex.Message);
            }

            return token;
        }


        public async Task<string> ParseRequestVerificationToken(string response)
        {
            string token = null;
            try
            {
                token = await Task.Run(() =>
                             {

                                 Regex reg = new Regex("<input.*? name=[\"]*__RequestVerificationToken[\"] *.*?>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                                 Match imagesourse = reg.Match(response);

                                 reg = new Regex("[^`\"]*[^`\"]", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                                 MatchCollection t = reg.Matches(imagesourse.Value);
                                 if (t.Count > 5)
                                     return t[5].Value;
                                 else
                                     return String.Empty;
                             });

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse ParseRequestVerificationToken Error: {0}", ex.Message);
            }

            return token;
        }

     
    }

    class AuthentificationHttp
    {
        HttpProvider _httpprovider;

        public AuthentificationHttp(HttpProvider httpprovider)
        {
            _httpprovider = httpprovider;
        }

        public async Task<string> GetRequestVerificationToken()
        {
            using (HttpResponseMessage response = await _httpprovider.HttpMethodGet("/Account/Login"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return String.Empty;
            }
        }

        public async Task<string> GetASPXAUTHToken(FormUrlEncodedContent content)
        {
            using (HttpResponseMessage response = await _httpprovider.HttpMethodPost("/Account/Login", content))
            {
                if (response != null)
                    return response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                else
                    return String.Empty;
            }
        }
    }


  


    //public async Task<bool> LogOutAccaunt()
    //{
    //    var values = new Dictionary<string, string>();
    //    FormUrlEncodedContent content = null;
    //    values.Add("__RequestVerificationToken", await GetRequestVerificationToken());

    //    using (HttpResponseMessage response = await _http.HttpMethodPost("/Account/Login", content))
    //    {
    //        if (response != null)
    //        {
    //            DeleteAccount();
    //            LogoutSucsessfull(this, new MessageEvent("Exit to accaunt"));
    //            return true;
    //        }
    //        else
    //        {
    //            LogoutError(this, new MessageEvent("Exit to accaunt error"));
    //            return false;
    //        }
    //    }
    //}




    /// <summary>
    /// класс представления аккаунта в программе
    /// </summary>
    public class Account
    {
        private string username = null;
        public string Username
        {
            get { return username; }
            private set { value = username; }
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



        public Account(Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> items)
        {
            username = items.Item1.Value;
            password = items.Item2.Value;
            requestVerificationToken = items.Item3.Value;
            aspxauthtToken = items.Item4.Value;
        }

        public Account(List<StorageAccount> items)
        {
            StorageAccount _stp = null;

            _stp = items.Where(i => i.Key == "UserName").FirstOrDefault();
            if (_stp != null)
                username = _stp.Value;
            else
                username = String.Empty;

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
            this.username = email;
            password = pass;
            requestVerificationToken = token1;
            aspxauthtToken = token2;
        }

        public bool AccCelii()
        {
            if (Username != String.Empty && Password != String.Empty && RequestVerificationToken != String.Empty && ASPXAUTHToken != String.Empty)
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


    class AuthentificationStorage
    {
        private SQLiteService st;

        public AuthentificationStorage(ISQLitePlatform platform, string documentsPath)
        {
             st = new SQLiteService(platform, documentsPath);
        }

        //metod zapros sql

            //metodi....

        public async Task SaveAccount(List<StorageAccount> items)
        {
            foreach (StorageAccount item in items)
                await st.InsertOrReplaceItem<StorageAccount>(item);
        }

        public async Task<List<StorageAccount>> ReadAccount()
        {
            return await st.GetAllItems<StorageAccount>();
        }

        public async Task DeleteAccount()
        {
            List<StorageAccount> items = await st.GetAllItems<StorageAccount>();
            foreach (StorageAccount item in items)
                await st.DeleteItem<StorageAccount>(item);
        }
    }

    

}
