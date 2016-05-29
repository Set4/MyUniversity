using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.RatingModel
{
    public class ParseRating
    {
        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;

        private static List<string> GetLessons(string response)
        {
            List<string> prifil = new List<string>();

            Regex regex = new Regex("<td rowspan=[\"][2][\"]>(.*?)<\\/td>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection p = regex.Matches(response);

            regex = new Regex(">(.*?)<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            foreach (Match m in p)
            {
                Match m1 = regex.Match(m.Value);

                prifil.Add(m1.Value.Substring(1,m1.Value.Length-2));
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

            foreach (Match m in k)
            {
                regex = new Regex(">([\\d]*?)<\\/", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                MatchCollection k1 = regex.Matches(m.Value);

                prifil.Add(new Int32[] { 1, 2, 3, 4, 5 });

                //prifil.Add(new Int32[] { Convert.ToInt32(k1[0].Value.Substring(1, k1[0].Value.Length - 2)),
                //Convert.ToInt32(k1[1].Value.Substring(1, k1[1].Value.Length - 2)),
                //Convert.ToInt32(k1[2].Value.Substring(1, k1[2].Value.Length - 2)),
                //Convert.ToInt32(k1[3].Value.Substring(1, k1[3].Value.Length - 2)),
                //Convert.ToInt32(k1[4].Value.Substring(1, k1[4].Value.Length - 2))});
            }

            return prifil;
        }


        private static  List<List<Int32>> GetRatinLessons(string response)
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


        public static List<Lesson> GetRating(string response)
        {
            List<Lesson> lesson = new List<Lesson>();

            List<string> _less = GetLessons(response);
            List<int[]> _points = GetMainRatingLessons(response);
            List<List<int>> rating = GetRatinLessons(response);

            for(int i=0; i<_less.Count; i++)
            {
                lesson.Add(new Lesson(Convert.ToString(i+1), _less[i], _points[i][0], _points[i][1], _points[i][2], _points[i][3], _points[i][4], rating[i]));
            }

            return lesson;
        }

    }
}
