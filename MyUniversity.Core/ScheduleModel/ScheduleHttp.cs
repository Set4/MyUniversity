using MyUniversity.Core.Сommon_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.AzureModel
{
    class ScheduleHttp
    {
        HttpProvider _provider;

        public ScheduleHttp(AuthenticationModel.AuthentificationModel acc)
        {
            CookieCollection coll = new CookieCollection()
            { new Cookie("__RequestVerificationToken",
            acc._Acc.RequestVerificationToken), new Cookie(".ASPXAUTH", acc._Acc.ASPXAUTHToken) };

            _provider = new HttpProvider();
        }

        public async Task<string> HttpGetWeeks()
        {
            using (HttpResponseMessage response = await _provider.HttpMethodGet("/Student/Brs?IdRup=64&SemestrP=8&Year=2015"))
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
