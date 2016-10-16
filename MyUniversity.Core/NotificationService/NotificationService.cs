using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.NotificationModel
{
    /// <summary>
    /// класс для сохранения данных в бд
    /// </summary>
    public class Notification : stringKey
    {
        [PrimaryKey]
        public string Key { get; set; }

        [MaxLength(100)]
        public string Header { get; set; }

        [MaxLength(300)]
        public string Text { get; set; }

        [MaxLength(100)]
        public string Date { get; set; }


        public bool State { get; set; }

        public Notification()
        {

        }

        public Notification(string header, string text, string date, bool state)
        {
            Header = header;
            Text = text;
            Date = DateTime.Parse(date).Date.ToString();
            State = state;
        }

    }


    class NotificationService
    {

        NotificationHttp _notificationhttp;
        NotificationParse _notificationparse;
        NotificationStorage _notificationstorage;

       

        public NotificationService(HttpProvider http, ISQLitePlatform platform, string documentsPath)
        {
            _notificationhttp = new NotificationHttp(http);
            _notificationparse = new NotificationParse(1);
            _notificationstorage = new NotificationStorage(platform, documentsPath);
        }

        public async Task<List<Notification>> GetNotification()
        {
            List<Notification> items = await _notificationstorage.Load();

            List<Notification> newitems = await _notificationparse.ParseAllNotifications(await _notificationhttp.HttpGetMessage());

            if (newitems != null)
                foreach (Notification item in items)
                    newitems.RemoveAll((i) => i.Header == item.Header && i.Date == item.Date);

            if (newitems.Count > 0)
                await _notificationstorage.Save(newitems);

            items.AddRange(newitems);



            return items;
        }

        public async Task Update(string key)
        {
            Notification item = await _notificationstorage.Saerch(key);
            if (item != null)
            {
                item.State = true;
                await _notificationstorage.Update(item);
            }
        }

    
    }

    class NotificationHttp
    {
        HttpProvider _httpprovider;

        public NotificationHttp(HttpProvider httpprovider)
        {
            _httpprovider = httpprovider;
        }

        public async Task<string> HttpGetMessage()
        {
            using (HttpResponseMessage response = await _httpprovider.HttpMethodGet("/Message/Index"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return String.Empty;
            }
        }

    }

    class NotificationParse
    {
        /// <summary>
        /// Время ожидания операции поиска Токенов
        /// </summary>
        double _timewait;

        public NotificationParse(double timewait)
        {
            _timewait = timewait;
        }


        private async Task<List<Notification>> ParseNotifications(string response, string recuest, bool state)
        {
            List<Notification> notifications = new List<Notification>();

            try
            {
                notifications = await Task.Run(() =>
                {

                    List<Notification> mess = new List<Notification>();

                    Regex regex = new Regex(recuest, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection info = regex.Matches(response);

                    foreach (Match m in info)
                    {
                        regex = new Regex("<strong>([\\s\\S]*?)<\\/strong>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                        Match k = regex.Match(m.Value);

                        regex = new Regex("<\\/strong>([\\s\\S]*?)<\\/p>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                        Match k1 = regex.Match(m.Value);

                        regex = new Regex("<\\/strong>([\\s\\S]*?)<\\/p>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                        Match k2 = regex.Match(m.Value);



                        mess.Add(new Notification(k.Value, k2.Value, k1.Value, state));
                    }
                    return mess;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
                notifications.Add(new Notification("Demo Message", "Если вы видите это сообщение значит разметка сайта снова поменялась :(", "00.00.0000", false));
            }

            return notifications;
        }

        public async  Task<List<Notification>> ParseAllNotifications(string response)
        {
            List<Notification> items = new List<Notification>();
            items.AddRange(await ParseNotifications(response, "<div.*?class=[\"]*message-div well message-unreaded bg-warning[\"]*.*?>((.*?|\\s)*?)<\\/div>", false));
            items.AddRange(await ParseNotifications(response, "<div.*?class=[\"]*message-div well message-readed[\"]*.*?>((.*?|\\s)*?)<\\/div>", true));

            return items;
        }


    }
    class NotificationStorage
    {
        private SQLiteService st;

        public NotificationStorage(ISQLitePlatform platform, string documentsPath)
        {
            st = new SQLiteService(platform, documentsPath);
        }

 public async Task<List<Notification>> Load()
        {
            return await st.GetAllItems<Notification>();
        }


        public async Task Save(List<Notification> items)
        {
            foreach(Notification item in items)
           await st.InsertOrReplaceItem<Notification>(item);
        }
       
        public async Task<Notification> Saerch(string key)
        {
            return await st.GetItemsById<Notification>(key);
        }

        public async Task Update(Notification item)
        {
            await st.InsertOrReplaceItem<Notification>(item);
        }
    }
}
