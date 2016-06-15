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

namespace MyUniversity.Core.RatingModel
{

    /// <summary>
    /// класс для сохранения данных в бд
    /// </summary>
    public class Lesson:stringKey
    {

        [PrimaryKey]
        public string Key { get; set; }

        [NotNull]
        public string NameLesson { get; set; }

        [NotNull, Default(value: 0)]
        public int TotalMiss { get; set; }

        [NotNull, Default(value: 0)]
        public int IndividualRating { get; set; }

        [NotNull, Default(value: 0)]
        public int ExtraPoints { get; set; }

        [NotNull, Default(value: 0)]
        public int TotalPoints { get; set; }

        [NotNull, Default(value: 0)]
        public int TotalPointsPercentage { get; set; }

        [MaxLength(300)]
        public string PointsInWeek { get; set; }


        public Lesson()
        { }

        public Lesson(string key, string namelesson, int totalmiss, int rating, int paints, int total, int persent, List<int> items)
        {
            Key = key;
            NameLesson = namelesson;
            TotalMiss = totalmiss;
            IndividualRating = rating;
            ExtraPoints = paints;
            TotalPoints = total;
            TotalPointsPercentage = persent;
            foreach (int i in items)
                PointsInWeek += items.ToString() + ";";
        }
    }

  

    public interface IRatingModel
    {
        event EventHandler<MessageEvent> UpdatingRating;

        event EventHandler<MessageEvent> NoNetwork;

        event EventHandler<MessageEvent> AccountIncorrect;
        void GetRating();
    }

  

  public  class RatingModel:IRatingModel
    {
        HttpProvider _http;
        AuthenticationModel.AuthentificationModel _account;

        SQLiteService _storage;

        //IStorageProvider _storage;


        List<Lesson> _lessons = null;



        static object storageLoker = new object();


        /// <summary>
        /// Время ожидания парсинга
        /// </summary>
        const int timewait = 1;


        public RatingModel(AuthenticationModel.AuthentificationModel account, ISQLitePlatform platform, string documentsPath)
        {
            _account = account;

            _storage = new SQLiteService(platform, documentsPath);



            _http = new HttpProvider(new CookieCollection()
            {
                new Cookie("__RequestVerificationToken", account._Acc.RequestVerificationToken),
                new Cookie(".ASPXAUTH", account._Acc.ASPXAUTHToken)
            });


            _lessons = new List<Lesson>();




            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated;


            account.LoginSucsessfull += Account_LoginSucsessfull;
            account.IncorrectLoginANDPassword += Account_IncorrectLoginANDPassword;


        }

        public event EventHandler<MessageEvent> UpdatingRating = delegate { };

        public event EventHandler<MessageEvent> NoNetwork = delegate { };

        public event EventHandler<MessageEvent> AccountIncorrect = delegate { };


        private void Account_IncorrectLoginANDPassword(object sender, MessageEvent e)
        {
            AccountIncorrect(this, new MessageEvent("Account_IncorrectLoginANDPassword"));
        }

        private void Account_LoginSucsessfull(object sender, MessageEvent e)
        {
            Task<Task<List<Lesson>>> HttpUpdateTask = new Task<Task<List<Lesson>>>(async () =>
            {
                return await HttpUpdateNotifications();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async (t) =>
            {
                List<Lesson> i = await t.Result;
                if (i.Count > 0)
                    UpdatingRating(this, new MessageEvent("", i));
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




        private async Task<string> HttpGetRating()
        {
            using (HttpResponseMessage response = await _http.HttpMethodGet("/Student/Brs?IdRup=64&SemestrP=8&Year=2015"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return null;
            }
        }


        private static List<string> GetNameLessons(string response)
        {
            List<string> prifil = new List<string>();

            Regex regex = new Regex("<td rowspan=[\"][2][\"]>(.*?)<\\/td>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection p = regex.Matches(response);

            regex = new Regex(">(.*?)<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            foreach (Match m in p)
            {
                Match m1 = regex.Match(m.Value);

                prifil.Add(m1.Value.Substring(1, m1.Value.Length - 2));
            }


            return prifil;
        }

        private static List<int[]> GetMainRatingLessons(string response)
        {
            List<int[]> prifil = new List<int[]>();

            Regex regex = new Regex("<table.*?>([\\s\\S]*?)<\\/table>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match table = regex.Match(response);

            regex = new Regex("<tbody>([\\s\\S]*?)<\\/tbody>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match info = regex.Match(table.Value);

            regex = new Regex("<th class=[\"]text-center[\"]>([\\s\\S]*?)<\\/th>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection k = regex.Matches(info.Value);

            
             prifil.Add(new Int32[] { 0, 0, 0, 54, 90 });
            prifil.Add(new Int32[] { 0, 0, 45, 45, 75 });
            prifil.Add(new Int32[] { 0, 0, 10, 48, 80 });
            prifil.Add(new Int32[] { 0, 0, 0, 52, 86 });
            prifil.Add(new Int32[] { 0, 0, 37, 50, 83 });
            //foreach (Match m in k)
            //{
            //    regex = new Regex(">([\\d]*?)<\\/", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            //    Match k1 = regex.Match(m.Value);




            //    //string s = Convert.ToInt32(k1.Value.Substring(1, k1.Value.Length - 2)).ToString();

            //    //prifil.Add(new Int32[] { Convert.ToInt32(k1[0].Value.Substring(1, k1[0].Value.Length - 2)),
            //    //Convert.ToInt32(k1[1].Value.Substring(1, k1[1].Value.Length - 2)),
            //    //Convert.ToInt32(k1[2].Value.Substring(1, k1[2].Value.Length - 2)),
            //    //Convert.ToInt32(k1[3].Value.Substring(1, k1[3].Value.Length - 2)),
            //    //Convert.ToInt32(k1[4].Value.Substring(1, k1[4].Value.Length - 2))});
            //}

            return prifil;
        }


        private static List<List<Int32>> GetWeekRatinLessons(string response)
        {
            List<List<Int32>> prifil = new List<List<Int32>>();

            Regex regex = new Regex("<table.*?>([\\s\\S]*?)<\\/table>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match table = regex.Match(response);

            regex = new Regex("<tbody>([\\s\\S]*?)<\\/tbody>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match info = regex.Match(table.Value);


            //< td > ([\d] *?) <\/ td >/ g //поставлении балы в таблице 
            //> ([\d] *?) <\//g //поставлении балы в таблице -> получили цифры >ЦИСЛО</

            regex = new Regex("<td>([\\d]*?)<\\/td>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection k = regex.Matches(info.Value);

            foreach (Match m in k)
            {
                List<Int32> b = new List<int>();

                regex = new Regex(">([\\d]*?)<\\/", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                MatchCollection k1 = regex.Matches(m.Value);
                foreach (Match m1 in k1)
                {
                    // b.Add(Convert.ToInt32(m1.Value.Substring(1, m1.Value.Length - 1)));
                    b.Add(10);
                }
                prifil.Add(b);
            }
            return prifil;
        }


        private static List<Lesson> ParseRating(string response)
        {
            List<Lesson> lesson = new List<Lesson>();

            List<string> _less = GetNameLessons(response);
            List<int[]> _points = GetMainRatingLessons(response);
            List<List<int>> rating = GetWeekRatinLessons(response);



            for (int i = 0; i < _less.Count; i++)
            {
                lesson.Add(new Lesson(Convert.ToString(i + 1), _less[i], _points[i][0], _points[i][1], _points[i][2], _points[i][3], Convert.ToInt32(_points[i][4] * 3.5), rating[i]));
            }

            return lesson;
        }






        private List<Lesson> SearChchangetFields(List<Lesson> items)
        {
            List<Lesson> changetfields = items;



            foreach (Lesson item in _storage.GetAllItems<Lesson>().Result)
                changetfields.RemoveAll((i) => i.NameLesson == item.NameLesson && i.TotalPoints == item.TotalPoints);

            return changetfields;
        }


        private Task<List<Lesson>> StorageUpdateNotifications()
        {
            Monitor.Enter(storageLoker);
            try
            {
                List<Lesson> items = _storage.GetAllItems<Lesson>().Result;
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


        private Task<List<Lesson>> HttpUpdateNotifications()
        {
            List<Lesson> items;

            string response = HttpGetRating().Result;

            if (response != null)
            {
                items = ParseRating(response);


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
                            foreach (Lesson i in items)
                                j = _storage.InsertOrReplaceItem<Lesson>(i).Result;

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
                return new List<Lesson>();
            });
        }


        public async void GetRating()
        {
           
                       Task<Task<List<Lesson>>> StorageUpdateTask = new Task<Task<List<Lesson>>>(async () =>
            {
                return await StorageUpdateNotifications();
            });

            StorageUpdateTask.Start();

            Task taskUI1 = StorageUpdateTask.ContinueWith(async (t) =>
            {
                List<Lesson> i = await t.Result;
                if (i.Count > 0)
                    UpdatingRating(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());



            Task<Task<List<Lesson>>> HttpUpdateTask = new Task<Task<List<Lesson>>>(async () =>
            {
                return await HttpUpdateNotifications();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async (t) =>
            {
                List<Lesson> i = await t.Result;
                if (i.Count > 0)
                    UpdatingRating(this, new MessageEvent("", i));
            }, TaskScheduler.FromCurrentSynchronizationContext());


        }


    }
}
