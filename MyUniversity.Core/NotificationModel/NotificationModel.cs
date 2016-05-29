using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyUniversity.Core.NotificationModel
{
    /// <summary>
    /// класс для сохранения данных в бд
    /// </summary>
    public class Notification:stringKey
    {
        [PrimaryKey]
        public string Key { get; set; }

        [MaxLength(100)]
        public string Header { get; set; }

        [MaxLength(300)]
        public string Text { get; set; }

        [MaxLength(100)]
        public string Date { get; set; }


        public bool IsRead { get; set; }

        public Notification()
        {

        }

        public Notification(string header, string text, string date, bool isread)
        {
            Header = header;
            Text = text;
            Date = DateTime.Parse(date).Date.ToString();
            IsRead = isread;
        }

    }

  
        public interface INotificationModel
    {
        event EventHandler<MessageEvent> UpdatingNotification;

        event EventHandler<MessageEvent> NoNetwork;

        event EventHandler<MessageEvent> AccountIncorrect;

        void GetNotifications();
        void ReadNotifications(Notification item);
    }

    public class NotificationModel:INotificationModel
    {

        HttpProvider _http;
        AuthenticationModel.AuthentificationModel _account;

        SQLiteService _storage;

        //IStorageProvider _storage;


        List<Notification> _notifications = null;



        static object storageLoker = new object();


        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;



        public NotificationModel(AuthenticationModel.AuthentificationModel account, ISQLitePlatform platform, string documentsPath)
        {
            _account = account;

            _storage = new SQLiteService(platform, documentsPath);



            _http = new HttpProvider(new CookieCollection()
            {
                new Cookie("__RequestVerificationToken", account._Acc.RequestVerificationToken),
                new Cookie(".ASPXAUTH", account._Acc.ASPXAUTHToken)
            });


            _notifications = new List<Notification>();




            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated;


            account.LoginSucsessfull += Account_LoginSucsessfull;
            account.IncorrectLoginANDPassword += Account_IncorrectLoginANDPassword;

          
        }

        private void Account_IncorrectLoginANDPassword(object sender, MessageEvent e)
        {
            AccountIncorrect(this, new MessageEvent("Account_IncorrectLoginANDPassword"));
        }

        private void Account_LoginSucsessfull(object sender, MessageEvent e)
        {
            Task<Task<List<Notification>>> HttpUpdateTask = new Task<Task<List<Notification>>>(async () =>
            {
                return await HttpUpdateNotifications();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async (t) =>
            {
                List<Notification> i = await t.Result;
                if (i.Count > 0)
                    UpdatingNotification(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void _http_AuthenticationTokensOutdated(object sender, MessageEvent e)
        {
            _account.LogInAccaunt(_account._Acc.Email, _account._Acc.Password);
        }

        private void _http_ResponseImpossibleTo(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("Net seti"));
        }

        public event EventHandler<MessageEvent> UpdatingNotification = delegate { };

        public event EventHandler<MessageEvent> NoNetwork = delegate { };

        public event EventHandler<MessageEvent> AccountIncorrect = delegate { };



        private async Task<string> HttpGetMessage()
        {
            using (HttpResponseMessage response = await _http.HttpMethodGet("/Message/Index"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return null;
            }
        }



     

    


        private List<Notification> GetMessage(string response, string recuest, bool readet)
        {

            List<Notification> mess = new List<Notification>();

            Regex regex = new Regex(recuest, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection info = regex.Matches(response);

            foreach (Match m in info)
            {
                regex = new Regex("<strong>([\\s\\S]*?)<\\/strong>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                Match k = regex.Match(m.Value);

                regex = new Regex("<\\/strong>([\\s\\S]*?)<\\/p>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                Match k1 = regex.Match(m.Value);

                regex = new Regex("<\\/strong>([\\s\\S]*?)<\\/p>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                Match k2 = regex.Match(m.Value);



                mess.Add(new Notification(k.Value, k2.Value, k1.Value, readet));
            }
            return mess;
        }
    private List<Notification> ParseNotifications(string response)
        {
            List<Notification> items = new List<Notification>();
            items.AddRange(GetMessage(response, "<div.*?class=[\"]*message-div well message-unreaded bg-warning[\"]*.*?>((.*?|\\s)*?)<\\/div>", false));
            items.AddRange(GetMessage(response, "<div.*?class=[\"]*message-div well message-readed[\"]*.*?>((.*?|\\s)*?)<\\/div>", true));

            return items;
        }



        private List<Notification> SearChchangetFields(List<Notification> items)
        {
            List<Notification> changetfields = items;



            foreach (Notification item in _storage.GetAllItems<Notification>().Result)
                changetfields.RemoveAll((i) => i.Header == item.Header && i.Date == item.Date);

            return changetfields;
        }


        private  Task<List<Notification>> StorageUpdateNotifications()
        {
            Monitor.Enter(storageLoker);
            try
            {
                List<Notification> items =  _storage.GetAllItems<Notification>().Result;
                return Task.Run(() =>
                {
                    return items;
                });
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                return Task.Run(() =>
                {
                    return new List<Notification>();
                });
            }
            finally
            {
                Monitor.Exit(storageLoker);
            }

        }


        private Task<List<Notification>> HttpUpdateNotifications()
        {

           
            List<Notification> items;

            string response = HttpGetMessage().Result;

            if (response != null)
            {
                items = ParseNotifications(response);


                if (items.Count > 0)
                {
                    items = SearChchangetFields(items);
                    if (items.Count > 0)
                    {

                        //
                        Task.Delay(5000).Wait();
                        //

                        Monitor.Enter(storageLoker);
                        try
                        {
                            int j;
                            foreach (Notification i in items)
                             j=    _storage.InsertOrReplaceItem<Notification>(i).Result;

                            return Task.Run(() =>
                            {
                                return items;
                            });
                        }
                        finally
                        {
                            Monitor.Exit(storageLoker);

                        }
                    }
                }

             


              
            }
            return Task.Run(() =>
            {
                return new List<Notification>();
            });
        }


        public async void GetNotifications()
        {

             Task<Task<List<Notification>>> StorageUpdateTask = new Task<Task<List<Notification>>>(async () =>
            {
                return await StorageUpdateNotifications();
            });

            StorageUpdateTask.Start();

            Task taskUI1 = StorageUpdateTask.ContinueWith(async (t) =>
            {
                 List < Notification > i = await t.Result;
                if (i.Count>0)
                    UpdatingNotification(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());



            Task<Task<List<Notification>>> HttpUpdateTask = new Task<Task<List<Notification>>>(async () =>
            {
                return await HttpUpdateNotifications();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async (t) =>
            {
                List<Notification> i = await t.Result;
                if (i.Count > 0)
                    UpdatingNotification(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());


        }


        public async void ReadNotifications(Notification item)
        {
            item.IsRead = true;
            Monitor.Enter(storageLoker);
            try
            {
              await  _storage.UpdateItem<Notification>(item);
            }
            finally
            {
                Monitor.Exit(storageLoker);
            }
        }
    }
}
