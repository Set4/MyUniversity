using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.NotificationModel
{
   public class HttpMessage
    {
        HttpProvider _provider;

        public HttpMessage(AuthenticationModel.Account acc)
        {
            CookieCollection coll = new CookieCollection()
            { new Cookie("__RequestVerificationToken",
            acc.RequestVerificationToken), new Cookie(".ASPXAUTH", acc.ASPXAUTHToken) };

            _provider = new HttpProvider();
        }

        public async Task<string> HttpGetMessage()
        {
            using (HttpResponseMessage response = await _provider.HttpMethodGet("/Message/Index"))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                {

                    return null;
                }
            }
        }


    }
}
