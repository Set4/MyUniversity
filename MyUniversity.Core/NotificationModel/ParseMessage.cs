using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.NotificationModel
{
    class ParseMessage
    {
        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;

        public static List<Notification> GetReadMessage(string response)
        {
            return  GetMessage(response, "<div.*?class=[\"]*message-div well message-readed[\"]*.*?>((.*?|\\s)*?)<\\/div>", true);
        }

        public static  List<Notification> GetUnreadMessage(string response)
        {
            return  GetMessage(response, "<div.*?class=[\"]*message-div well message-unreaded bg-warning[\"]*.*?>((.*?|\\s)*?)<\\/div>", false);
        }



        static List<Notification> GetMessage(string response, string recuest, bool readet)
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

              

               mess.Add(new Notification( k.Value,k2.Value,k1.Value,readet));
            }
            return mess;
        }

    }
}
