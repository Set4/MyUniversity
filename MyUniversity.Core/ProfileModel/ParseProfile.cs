using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.ProfileModel
{
    class ParseProfile
    {
        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;

        public static string GetImageSourse(string response)
        {
            Regex reg = new Regex("<img.*?alt=[\"]*Фотография[\"]*.*?>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match imagesourse = reg.Match(response);

            reg = new Regex("\\/Image\\/UserPhoto\\/[^\\s]*\"", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            return reg.Match(imagesourse.Value).Value;
        }



        public static  Dictionary<string,string> GetProfile(string response)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            Regex regex = new Regex("<ul.*?class=[\"]*list-group[\"]*.*?>+(.*?|\\s)+<\\/ul>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match pfofil = regex.Match(response);

            regex = new Regex("<strong>.*?<\\/li>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection info = regex.Matches(pfofil.Value);

           
                regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
                MatchCollection k = regex.Matches(info[0].Value);
                items.Add("GroupNumber", k[1].Value.Substring(1, k[1].Value.Length - 2));

            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
             k = regex.Matches(info[1].Value);
            items.Add("Department", k[1].Value.Substring(1, k[1].Value.Length - 2));

            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
             k = regex.Matches(info[2].Value);
            items.Add("Chair", k[1].Value.Substring(1, k[1].Value.Length - 2));


            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[3].Value);
            items.Add("Specialty", k[1].Value.Substring(1, k[1].Value.Length - 2));


            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[4].Value);
            items.Add("TrainingProfile", k[1].Value.Substring(1, k[1].Value.Length - 2));

            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[5].Value);
            items.Add("ModeofStudy", k[1].Value.Substring(1, k[1].Value.Length - 2));

            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[6].Value);
            items.Add("Email", k[1].Value.Substring(1, k[1].Value.Length - 2));

            items.Add("ImageSource", GetImageSourse(response));

            return items;
        }

    }
}
