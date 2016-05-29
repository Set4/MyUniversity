using MyUniversity.Core.Сommon_Code;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.ProfileModel
{
   public  class HttpProfile
    {



        HttpProvider _provider;

        public HttpProfile(string RequestVerificationToken, string ASPXAUTHToken)
        {
            CookieCollection _coll = new CookieCollection()
            {
                new Cookie("__RequestVerificationToken", RequestVerificationToken),
                new Cookie(".ASPXAUTH", ASPXAUTHToken)
            };

            _provider= new HttpProvider();
        }



        public async Task<string> HttpGetProfile()
        {
            using (HttpResponseMessage response = await _provider.HttpMethodGet("/Student/DekanatInfo"))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                {

                    return null;
                }
            }
        }


        public async Task<byte[]> HttpDownloadImage(string uri)
        {
            using (HttpResponseMessage response = await _provider.HttpMethodGet(uri))
            {
                if (response.StatusCode == HttpStatusCode.OK && response.Content.Headers.ContentType.MediaType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                    return await response.Content.ReadAsByteArrayAsync();
                else
                {

                    return null;
                }
            }
        }


    }
}
