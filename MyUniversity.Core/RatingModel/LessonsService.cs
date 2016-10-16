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

namespace MyUniversity.Core.RatingModel
{

    /// <summary>
    /// класс для сохранения данных в бд
    /// </summary>
    public class Lesson : stringKey
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

        [Ignore]
        public int Percentage
        {
            get
            {
                return Convert.ToInt32(TotalPointsPercentage * 3.5);
            }

        }

        [MaxLength(300)]
        public string PointsInWeek { get; set; }


        public Lesson()
        { }

        public Lesson(string key, string namelesson, int totalmiss, int rating, int paints, int total, int persent, List<string> items)
        {
            Key = key;
            NameLesson = namelesson;
            TotalMiss = totalmiss;
            IndividualRating = rating;
            ExtraPoints = paints;
            TotalPoints = total;
            TotalPointsPercentage = persent;
            foreach (string i in items)
                PointsInWeek += i + ";";
        }
    }



    class LessonsService
    {
        LessonsHttp _lessonshttp;
        LessonsParse _lessonsparse;
        LessonsStorage _lessonsstorage;



        public LessonsService(HttpProvider http, ISQLitePlatform platform, string documentsPath)
        {
            _lessonshttp = new LessonsHttp(http);
            _lessonsparse = new LessonsParse(1);
            _lessonsstorage = new LessonsStorage(platform, documentsPath);
        }

        public async Task<List<Lesson>> GetLessons()
        {
            List<Lesson> items = await _lessonsstorage.Load();

            List<Lesson> newitems = await _lessonsparse.ParseLessonss(await _lessonshttp.HttpGetRating());

            if (newitems != null)
                foreach (Lesson item in items)
                    newitems.RemoveAll((i) => i.TotalPoints== item.TotalPoints);

            if (newitems.Count > 0)
                await _lessonsstorage.Save(newitems);

            items.AddRange(newitems);
            return items;
        }


    }

    class LessonsHttp
    {
        HttpProvider _httpprovider;

        public LessonsHttp(HttpProvider httpprovider)
        {
            _httpprovider = httpprovider;
        }

        public async Task<string> HttpGetRating()
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

    class LessonsParse
    {
        /// <summary>
        /// Время ожидания операции поиска Токенов
        /// </summary>
        double _timewait;

        public LessonsParse(double timewait)
        {
            _timewait = timewait;
        }


        private async Task<List<string>> ParseNameLessons(string response)
        {
            List<string> lessons = new List<string>();

            try
            {
                lessons = await Task.Run(() =>
                {
                    List<string> items = new List<string>();

                    Regex regex = new Regex("<td rowspan=[\"][2][\"]>(.*?)<\\/td>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection p = regex.Matches(response);

                    regex = new Regex(">(.*?)<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    foreach (Match m in p)
                    {
                        Match m1 = regex.Match(m.Value);

                        items.Add(m1.Value.Substring(1, m1.Value.Length - 2));
                    }


                    return items;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
            }

            return lessons;
        }


        private async Task<List<int[]>> ParseRatingLessons(string response)
        {
            List<int[]> rating = new List<int[]>();

            try
            {
                rating = await Task.Run(() =>
                {
                    List<int[]> items = new List<int[]>();

                    Regex regex = new Regex("<table.*?>([\\s\\S]*?)<\\/table>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match table = regex.Match(response);

                    regex = new Regex("<tbody>([\\s\\S]*?)<\\/tbody>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match info = regex.Match(table.Value);

                    regex = new Regex("<th class=[\"]text-center[\"]>([\\s\\S]*?)<\\/th>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection k = regex.Matches(info.Value);

                    //
                    //демонстрация возможности отображения информации
                    //введена статичная информация в связи изменением в html разметки сайта
                    items.Add(new Int32[] { 0, 0, 0, 54, 90 });
                    items.Add(new Int32[] { 0, 0, 45, 45, 75 });
                    items.Add(new Int32[] { 0, 0, 10, 48, 80 });
                    items.Add(new Int32[] { 0, 0, 0, 52, 86 });
                    items.Add(new Int32[] { 0, 0, 37, 50, 83 });


                    return items;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
            }

            return rating;
        }


        private async Task<List<List<string>>> GetWeekRatinLessons(string response)
        {
            List<List<string>> weekrating = new List<List<string>>();

            try
            {
                weekrating = await Task.Run(() =>
                {
                    List<List<string>> items = new List<List<string>>();

                    Regex regex = new Regex("<table.*?>([\\s\\S]*?)<\\/table>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match table = regex.Match(response);

                    regex = new Regex("<tbody>([\\s\\S]*?)<\\/tbody>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match info = regex.Match(table.Value);


                    //< td > ([\d] *?) <\/ td >/ g //поставлении балы в таблице 
                    //> ([\d] *?) <\//g //поставлении балы в таблице -> получили цифры >ЦИСЛО</

                    regex = new Regex("<td>([\\d]*?)<\\/td>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection k = regex.Matches(info.Value);

                    foreach (Match m in k)
                    {
                        List<string> b = new List<string>();

                        regex = new Regex(">([\\d]*?)<\\/", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                        MatchCollection k1 = regex.Matches(m.Value);
                        foreach (Match m1 in k1)
                        {
                            // b.Add(Convert.ToInt32(m1.Value.Substring(1, m1.Value.Length - 1)));
                            b.Add("10");
                        }
                        items.Add(b);
                    }
                    return items;

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
            }

            return weekrating;
        }


        public async Task<List<Lesson>> ParseLessonss(string response)
        {


            List<Lesson> notifications = new List<Lesson>();

            try
            {
                notifications = await Task.Run(async () =>
                {
                    List<Lesson> lesson = new List<Lesson>();
                    List<string> _less = await ParseNameLessons(response);
                    List<int[]> _points = await ParseRatingLessons(response);
                    List<List<string>> rating = await GetWeekRatinLessons(response);



                    for (int i = 0; i < _less.Count; i++)
                    {
                        lesson.Add(new Lesson(Convert.ToString(i + 1), _less[i], _points[i][0], _points[i][1], _points[i][2], _points[i][3], Convert.ToInt32(_points[i][4]), rating[i]));
                    }

                    return lesson;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
            }

            return notifications;
        }



    }

    class LessonsStorage
    {
        private SQLiteService st;

        public LessonsStorage(ISQLitePlatform platform, string documentsPath)
        {
            st = new SQLiteService(platform, documentsPath);
        }

        public async Task<List<Lesson>> Load()
        {
            return await st.GetAllItems<Lesson>();
        }

        public async Task Save(List<Lesson> items)
        {
            foreach (Lesson item in items)
               await st.InsertOrReplaceItem<Lesson>(item);
        }
    }
    }
