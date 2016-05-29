using Microsoft.WindowsAzure.MobileServices;
using MyUniversity.Core.Сommon_Code;
using Newtonsoft.Json;
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

namespace MyUniversity.Core.ScheduleModel
{
    public class ScheduleItem : stringKey
    {
        [PrimaryKey, JsonProperty(PropertyName = "id")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "lessonName")]
        public string LessonName { get; set; }

        [JsonProperty(PropertyName = "lessonType")]
        public string LessonType { get; set; }

        [JsonProperty(PropertyName = "audience")]
        public string Audience { get; set; }

        [JsonProperty(PropertyName = "teacherName")]
        public string TeacherName { get; set; }

        [MaxLength(5), JsonProperty(PropertyName = "timeStart")]
        public string TimeStart { get; set; }

        [MaxLength(5), JsonProperty(PropertyName = "timeEnd")]
        public string TimeEnd { get; set; }

        [MaxLength(4), JsonProperty(PropertyName = "evenWeek")]
        public string EvenWeek { get; set; }

        [JsonProperty(PropertyName = "dayWeek")]
        public string DayWeek { get; set; }

        [MaxLength(5), JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
    }


    public class WeekData : stringKey
    {
        /// <summary>
        /// Номер недели
        /// </summary>
        [PrimaryKey]
        public string Key { get; set; }

        [NotNull]
        public string StartDate { get; set; }

        [NotNull]
        public string EndDate { get; set; }

        [Ignore]
        public string IsEvenWeek
        {
            get
            {
                if (Convert.ToInt32(Key) % 2 == 0)
                    return "**";
                else
                    return "*";
            }
        }


        public List<DayData> days;
         [Ignore]
        public List<DayData> Days
        {
            get
            {
                //List<DayData> items = new List<DayData>();
                //for (DateTime date = DateTime.Parse(StartDate); date <= DateTime.Parse(EndDate); date.AddDays(1))
                //{
                //    items.Add(new DayData(date, IsEvenWeek, _storage));
                //}
                //return items;
                return days;
            }
        }

        SQLiteService _storage;

        public WeekData()
        {
           
        }

        public WeekData(string key, string startDate, string endDate, SQLiteService _storage)
        {
            this.Key = key;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this._storage = _storage;
            this.days = GetDays();
        }

        private List<DayData> GetDays()
        {
            List<DayData> items = new List<DayData>();
            DateTime date = DateTime.Parse(StartDate);
            for (; date <= DateTime.Parse(EndDate); date=date.AddDays(1))
            {
                items.Add(new DayData(date, IsEvenWeek, _storage));
            }
            return items;
        }
    }



    public class DayData
    {
             
        DateTime date;
        string EvenWeek;
        SQLiteService _storage;

        /// <summary>
        /// дата число.месяц
        /// </summary>
        public string Date
        {
            get
            {
                return date.Day.ToString() + " " + new System.Globalization.CultureInfo("ru-RU").DateTimeFormat.GetMonthName(date.Month).ToString().ToUpper().Substring(0, 3);
            }
        }

        /// <summary>
        /// день недели
        /// </summary>
        public string DayWeek
        {
            get
            {
                return new System.Globalization.CultureInfo("ru-RU").DateTimeFormat.GetDayName(date.DayOfWeek).ToString().ToUpper();
            }
        }

        
        /// <summary>
        /// начало первого занятия
        /// </summary>
        public string TimeStart
        {
            get
            {
                if (Schedules.Count == 0)
                    return "Нет занятий";
                else
                    return Schedules.First().TimeStart;
            }
        }


        List<ScheduleItem> schedules=new List<ScheduleItem>();
        public List<ScheduleItem> Schedules
        {
            get
            {
                if (schedules != null)
                    return schedules;
                else
                    return new List<ScheduleItem>();
            }
        }

        async void GetSheduleItems()
        {
            schedules= await _storage.GetAllItems<ScheduleItem>().ContinueWith<List<ScheduleItem>>
                ((i) => i.Result.Where
                (item => ((item.EvenWeek == EvenWeek || item.EvenWeek == "*/**") && item.DayWeek == DayWeek) || item.Date == Date).ToList());
        }

        public DayData(DateTime date, string evenWeek, SQLiteService _storage)
        {
            this.date = date;
            this.EvenWeek = evenWeek;
            this._storage = _storage;
            GetSheduleItems();
        }


   
    }


    class AzureSchedule
    {
        static MobileServiceClient MobileService =
            new MobileServiceClient("http://myuniversity.azurewebsites.net");

        private IMobileServiceTable<ScheduleItem> scheduleItemTable = MobileService.GetTable<ScheduleItem>();

        public event EventHandler<MessageEvent> ErrorGetItems = delegate { };



        public async Task<List<ScheduleItem>> GetScheduleItem(string group)
        {
            MobileServiceCollection<ScheduleItem, ScheduleItem> items;

            try
            {
                items = await scheduleItemTable
                    .Where(item => item.Group == group)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                ErrorGetItems(this, new MessageEvent("Error loading items:\n" + e.Message.ToString()));
                return null;
            }



            return items.ToList();

        }


        public async Task UpdateCheckedTodoItem()
        {
            await scheduleItemTable.InsertAsync(new ScheduleItem()
            {
                Group = "ТРП-1-12",
                LessonName = "Экономика",
                LessonType = "Лекция",
                Audience = "А302",
                TeacherName = "Фамилия И.О.",
                TimeStart = "12:10",
                TimeEnd = "13:10",
                EvenWeek = "*/**",
                DayWeek = "Понедельник"

            });

            await scheduleItemTable.InsertAsync(new ScheduleItem()
            {
                Group = "ТРП-1-12",
                LessonName = "Экономика",
                LessonType = "Практика",
                Audience = "А302",
                TeacherName = "Фамилия И.О.",
                TimeStart = "13:20",
                TimeEnd = "14:10",
                EvenWeek = "*",
                DayWeek = "Вторник"

            });


            await scheduleItemTable.InsertAsync(new ScheduleItem()
            {
                Group = "ТРП-1-12",
                LessonName = "Экономика",
                LessonType = "Лр. работа",
                Audience = "А302",
                TeacherName = "Фамилия И.О.",
                TimeStart = "13:20",
                TimeEnd = "14:10",
                EvenWeek = "**",
                DayWeek = "Среда"

            });
        }
    }


    public interface IScheduleModel
    {
        event EventHandler<MessageEvent> NoNetwork;
        event EventHandler<MessageEvent> AccountIncorrect;

        event EventHandler<MessageEvent> ScheduleItemsUpdated;
        event EventHandler<MessageEvent> WeeksListCreated;

        Task<List<WeekData>> CreateListWeeks();
        Task<bool> AzureUpdateScheduleItems();
        void GetSchedule();
    }

  public class ScheduleModel:IScheduleModel
    {

        HttpProvider _http;
        AuthenticationModel.AuthentificationModel _account;

        SQLiteService _storage;

            AzureSchedule _azure;


        //IStorageProvider _storage;


        



        static object storageLoker = new object();


        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;

        public ScheduleModel(AuthenticationModel.AuthentificationModel account, ISQLitePlatform platform, string documentsPath)
        {
            _account = account;

            _storage = new SQLiteService(platform, documentsPath);

            _http = new HttpProvider(new CookieCollection()
            {
                new Cookie("__RequestVerificationToken", account._Acc.RequestVerificationToken),
                new Cookie(".ASPXAUTH", account._Acc.ASPXAUTHToken)
            });


           _azure = new AzureSchedule();



            _azure.ErrorGetItems += _azure_ErrorGetItems;


            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo1;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated1;

            account.LoginSucsessfull += Account_LoginSucsessfull1;
            account.IncorrectLoginANDPassword += Account_IncorrectLoginANDPassword1;
        }

        private void Account_IncorrectLoginANDPassword1(object sender, MessageEvent e)
        {
            //login or pass ne vernii vihod  v authpage + message
            AccountIncorrect(this, new MessageEvent("Account_IncorrectLoginANDPassword"));
        }

        private void Account_LoginSucsessfull1(object sender, MessageEvent e)
        {
            Task<Task<List<WeekData>>> CreateListWeeksTask = new Task<Task<List<WeekData>>>(async () =>
            {
                return await CreateListWeeks();

            });

            CreateListWeeksTask.Start();

            Task taskUI1 = CreateListWeeksTask.ContinueWith(async (t) =>
            {
                List<WeekData> _weeks = await t.Result;
                if (_weeks.Count > 0)
                    WeeksListCreated(this, new MessageEvent("", _weeks));
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void _http_AuthenticationTokensOutdated1(object sender, MessageEvent e)
        {
            //sobitie tokini ystareli 
            _account.LogInAccaunt(_account._Acc.Email, _account._Acc.Password);
        }

        private void _http_ResponseImpossibleTo1(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("Net seti"));
        }

        private void _azure_ErrorGetItems(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("Net dostypa k azure"));
        }




     

      

      


        public event EventHandler<MessageEvent> NoNetwork = delegate { };

        public event EventHandler<MessageEvent> AccountIncorrect = delegate { };


        public event EventHandler<MessageEvent> ScheduleItemsUpdated = delegate { };
        public event EventHandler<MessageEvent> WeeksListCreated = delegate { };


        private async Task<string> HttpGetWeeks()
        {
            using (HttpResponseMessage response = await _http.HttpMethodGet("/Student/Brs?IdRup=64&SemestrP=8&Year=2015"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return null;
            }
        }

        private List<Tuple<string, string, string>> ParseWeeks(string response)
        {
            List<Tuple<string, string, string>> prifil = new List<Tuple<string, string, string>>();

            Regex regex = new Regex("<table.*?>([\\s\\S]*?)<\\/table>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match table = regex.Match(response);


            regex = new Regex("<thead([\\s\\S]*?)<\\/thead>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match info = regex.Match(table.Value);


            regex = new Regex("<small>([\\s\\S]*?)<\\/small>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection p = regex.Matches(info.Value);


            for (int i = 0; i < p.Count; i++)
            {

                regex = new Regex(">([\\s\\S]*?)<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                MatchCollection k = regex.Matches(p[i].Value);

                prifil.Add(new Tuple<string, string, string>((i + 1).ToString(),
                    k[0].Value.Substring(1, k[0].Value.Length - 2), 
                    k[1].Value.Substring(1, k[1].Value.Length - 2)));
            }


            return prifil;
        }

        private async void SaveWeeks(List<WeekData> items)
        {
            foreach (WeekData item in items)
               await _storage.InsertOrReplaceItem<WeekData>(item);
        }

        private List<ScheduleItem> SearchChangesItems(List<ScheduleItem> newitems, List<ScheduleItem> items)
        {
            List<ScheduleItem> changetitems = new List<ScheduleItem>();

            foreach(ScheduleItem item in newitems)
            {
                if (!items.Contains(item))
                    changetitems.Add(item);
            }

            return changetitems;
        }



        private async Task<List<WeekData>> GetWeeks()
        {
            List<WeekData> items = new List<WeekData>();

            string response=String.Empty;

            response =await HttpGetWeeks();

            if (response != null)
                foreach (Tuple<string, string, string> item in ParseWeeks(response))
                    items.Add(new WeekData(item.Item1, item.Item2, item.Item3, _storage));

            Monitor.Enter(storageLoker);
            try
            {

                if (items.Count > 0)
                {
                    SaveWeeks(items);
                    return items;
                }
                else
                    return items;
                  

            }
            finally
            {
                Monitor.Exit(storageLoker);
            }
        }




        public Task<List<WeekData>> CreateListWeeks()
        {
            List<WeekData> items = new List<WeekData>();


            Monitor.Enter(storageLoker);
            try
            {
                foreach (WeekData item in _storage.GetAllItems<WeekData>().Result)
                    items.Add(new WeekData(item.Key, item.StartDate, item.EndDate, _storage));
            }
            finally
            {
                Monitor.Exit(storageLoker);
            }

            if (items.Count > 0)
                return Task.Run(() =>
                {
                    return items;
                });
            else
            {
                return Task.Run(() =>
                {
                    return GetWeeks().Result;
                });
            }
        }



        public Task<bool> AzureUpdateScheduleItems()
        {
            Monitor.Enter(storageLoker);
            try
            {
                List<ScheduleItem> changetitems = SearchChangesItems(_azure.GetScheduleItem("ТРП-1-12").Result, _storage.GetAllItems<ScheduleItem>().Result);
                if (changetitems.Count > 0)
                    return Task.Run(() =>
                    {
                        foreach(ScheduleItem item in changetitems)
                        {
                            _storage.InsertOrReplaceItem<ScheduleItem>(item);
                        }

                        return true;
                    });
                else
                    return Task.Run(() =>
                    {
                        return false;
                    });

            }
            finally
            {
                Monitor.Exit(storageLoker);
            }

        }


        public void GetSchedule()
        {
            Task<Task<List<WeekData>>> CreateListWeeksTask = new Task<Task<List<WeekData>>>(async () =>
            {
                return await CreateListWeeks();

            });

            CreateListWeeksTask.Start();

            Task taskUI1 = CreateListWeeksTask.ContinueWith(async (t) =>
            {
                List<WeekData> _weeks =await t.Result;
                if (_weeks.Count>0)
                    WeeksListCreated(this, new MessageEvent("", _weeks));
            }, TaskScheduler.FromCurrentSynchronizationContext());



            //Task<Task<bool>> AzureUpdateScheduleItemsTask = new Task<Task<bool>>(async () =>
            //{
            //    return await AzureUpdateScheduleItems();
            //});

   
            //AzureUpdateScheduleItemsTask.Start();

            //Task taskUI2 = AzureUpdateScheduleItemsTask.ContinueWith(async (t) =>
            //{
            //    if (await t.Result)
            //       ScheduleItemsUpdated(this, new MessageEvent(""));
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
