using Microsoft.WindowsAzure.MobileServices;
using MyUniversity.Core.Сommon_Code;
using Newtonsoft.Json;
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

namespace MyUniversity.Core.ScheduleModel
{
    public class ScheduleItem : stringKey
    {
        [PrimaryKey, JsonProperty(PropertyName = "id")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "lessonName")]
        public string LessonName { get; set; }

        [JsonProperty(PropertyName = "lessonType")]
        public int LessonType { get; set; }

        [JsonProperty(PropertyName = "audience")]
        public string Audience { get; set; }

        [JsonProperty(PropertyName = "teacherName")]
        public string TeacherName { get; set; }

        [MaxLength(5), JsonProperty(PropertyName = "timeStart")]
        public string TimeStart { get; set; }

        [MaxLength(5), JsonProperty(PropertyName = "timeEnd")]
        public string TimeEnd { get; set; }

        [JsonProperty(PropertyName = "evenWeek")]
        public int EvenWeek { get; set; }

        [JsonProperty(PropertyName = "dayWeek")]
        public int DayWeek { get; set; }

        [MaxLength(5), JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
    }


    class ScheduleService
    {
        ScheduleHttp _schedulehttp;
        ScheduleParse _scheduleparse;
        ScheduleStorage _schedulestorage;
        AzureService _azure;


        public ScheduleService(HttpProvider http, ISQLitePlatform platform, string documentsPath)
        {

            _schedulehttp = new ScheduleHttp(http);
            _scheduleparse = new ScheduleParse(1);
            _schedulestorage = new ScheduleStorage(platform, documentsPath);
            _azure = new AzureService();

        }

       public async Task<List<ScheduleItem>> GetSheduleItems(int EvenWeek, int DayWeek, string Date)
        {
            List<ScheduleItem> schedules = new List<ScheduleItem>();
            try
            {
                schedules = await _schedulestorage.LoadShedules().ContinueWith<List<ScheduleItem>>
                    ((i) => i.Result.Where
                    (item => ((item.EvenWeek == EvenWeek || item.EvenWeek == 3) && item.DayWeek == DayWeek) || item.Date == Date).ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message.ToString());
            }

            return schedules;
        }

        public async Task<List<ScheduleItem>> GetAllShedules()
        {
           await UpdateSchedule();
            return await _schedulestorage.LoadShedules();
        }

        public async Task<List<WeekData>> GetWeeks()
        {
            List<WeekData> items = await _schedulestorage.Load();

            List<WeekData> newitems = await _scheduleparse.ParseWeeks(await _schedulehttp.HttpGetWeeks());

            if (newitems != null)
                foreach (WeekData item in items)
                    newitems.RemoveAll((i) => i.Key == item.Key && i.StartDate == item.StartDate);

            if (newitems.Count > 0)
                await _schedulestorage.Save(newitems);

            items.AddRange(newitems);



            return items;
        }

        public async Task UpdateSchedule()
        {
            await _schedulestorage.SaveShedules(await _azure.GetScheduleItem());
        }
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
        public int IsEvenWeek
        {
            get
            {
                if (Convert.ToInt32(Key) % 2 == 0)
                    return 2;
                else
                    return 1;
            }
        }
        

        enum EvenWeek
        {
            even,//четная
            odd,//нечетная
            any//любая
        }

        [Ignore]
        public List<DateTime> DaysInWeek
        {
            get
            {
                List<DateTime> items = new List<DateTime>();
                DateTime date = DateTime.Parse(StartDate);
                for (; date <= DateTime.Parse(EndDate); date = date.AddDays(1))
                {
                    items.Add(date);
                }
                return items;
            }
        }

      

        public WeekData()
        {

        }

        public WeekData(string key, string startDate, string endDate)
        {
            this.Key = key;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }

    class AzureService
    {
    

 static MobileServiceClient _MobileService =
            new MobileServiceClient("http://myuniversity.azurewebsites.net");

      


        public async Task<List<ScheduleItem>> GetScheduleItem()
        {
            MobileServiceCollection<ScheduleItem, ScheduleItem> items;

            try
            {
   IMobileServiceTable<ScheduleItem> scheduleItemTable = _MobileService.GetTable<ScheduleItem>();

                items = await scheduleItemTable.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine("AzureService Error: ",e.Message);
                return null;
            }



            return items.ToList();

        }


        //await scheduleItemTable.InsertAsync(new ScheduleItem()
        //{
        //    Group = "ТРП-1-12",
        //    LessonName = "Экономика",
        //    LessonType = "Лекция",
        //    Audience = "А302",
        //    TeacherName = "Фамилия И.О.",
        //    TimeStart = "12:10",
        //    TimeEnd = "13:10",
        //    EvenWeek = "*/**",
        //    DayWeek = "Понедельник"

        //});

        //await scheduleItemTable.InsertAsync(new ScheduleItem()
        //{
        //    Group = "ТРП-1-12",
        //    LessonName = "Экономика",
        //    LessonType = "Практика",
        //    Audience = "А302",
        //    TeacherName = "Фамилия И.О.",
        //    TimeStart = "13:20",
        //    TimeEnd = "14:10",
        //    EvenWeek = "*",
        //    DayWeek = "Вторник"

        //});


        //await scheduleItemTable.InsertAsync(new ScheduleItem()
        //{
        //    Group = "ТРП-1-12",
        //    LessonName = "Экономика",
        //    LessonType = "Лр. работа",
        //    Audience = "А302",
        //    TeacherName = "Фамилия И.О.",
        //    TimeStart = "13:20",
        //    TimeEnd = "14:10",
        //    EvenWeek = "**",
        //    DayWeek = "Среда"

        //});

    }

    class ScheduleHttp
    {
        HttpProvider _httpprovider;

        public ScheduleHttp(HttpProvider httpprovider)
        {
            _httpprovider = httpprovider;
        }
        public async Task<string> HttpGetWeeks()
        {
            using (HttpResponseMessage response = await _httpprovider.HttpMethodGet("/Student/Brs?IdRup=64&SemestrP=8&Year=2015"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return String.Empty;
            }
        }
    }

    class ScheduleParse
    {
        /// <summary>
        /// Время ожидания операции поиска Токенов
        /// </summary>
        double _timewait;

        public ScheduleParse(double timewait)
        {
            _timewait = timewait;
        }

        public async Task<List<WeekData>> ParseWeeks(string response)
        {
            List<WeekData> weeks = new List<WeekData>();

            try
            {
                weeks = await Task.Run(() =>
                {
                    List<WeekData> items = new List<WeekData>();

                    Regex regex = new Regex("<table.*?>([\\s\\S]*?)<\\/table>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match table = regex.Match(response);


                    regex = new Regex("<thead([\\s\\S]*?)<\\/thead>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match info = regex.Match(table.Value);


                    regex = new Regex("<small>([\\s\\S]*?)<\\/small>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection p = regex.Matches(info.Value);


                    for (int i = 0; i < p.Count; i++)
                    {

                        regex = new Regex(">([\\s\\S]*?)<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                        MatchCollection k = regex.Matches(p[i].Value);

                        items.Add(new WeekData((i + 1).ToString(),
                            k[0].Value.Substring(1, k[0].Value.Length - 2),
                            k[1].Value.Substring(1, k[1].Value.Length - 2)));
                    }


                    return items;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
            }

            return weeks;
        }
    }
    class ScheduleStorage
    {
        private SQLiteService st;

        public ScheduleStorage(ISQLitePlatform platform, string documentsPath)
        {
            st = new SQLiteService(platform, documentsPath);
        }

        public async Task<List<WeekData>> Load()
        {
            return await st.GetAllItems<WeekData>();
        }

        public async Task Save(List<WeekData> items)
        {
            foreach (WeekData item in items)
              await  st.InsertOrReplaceItem<WeekData>(item);
        }
        public async Task<List<ScheduleItem>> LoadShedules()
        {
            return await st.GetAllItems<ScheduleItem>();
        }
        public async Task SaveShedules(List<ScheduleItem> items)
        {
            foreach(ScheduleItem item in items)
             await st.InsertOrReplaceItem<ScheduleItem>(item);
        }

    }

}
