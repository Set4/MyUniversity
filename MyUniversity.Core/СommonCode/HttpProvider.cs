using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.Сommon_Code
{
    
    public class HttpProvider
    {
        /// <summary>
        /// Начальный адрес запроса new Uri("http://e.kgeu.ru");
        /// </summary>
        Uri BaseUri { get; set; }
      

        //pri izmenenii cookie peresozdaiem httpclient
     public  CookieContainer Cookiecontainer { get; set; }
      
       // ?????oshibka
        HttpClient httpclient = null;
        HttpClient Httpclient
        {
            get
            {
                HttpClientHandler handler = new HttpClientHandler(); //static???????????

                handler.CookieContainer = Cookiecontainer;
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                handler.AllowAutoRedirect = false;

                if (httpclient == null)
                {
                    httpclient = new HttpClient(handler);
                    httpclient.BaseAddress = BaseUri;

                    #region Устанавливаем заголовки запроса httpclient.DefaultRequestHeaders.Add() 

                    httpclient.DefaultRequestHeaders.Add("Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

                    httpclient.DefaultRequestHeaders.Add("Connection", @"keep-alive");

                    httpclient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

                    httpclient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.109 Safari/537.36");

                    httpclient.DefaultRequestHeaders.Add("Referer", "http://e.kgeu.ru/Account/Login");

                    httpclient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");

                    httpclient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");

                    httpclient.DefaultRequestHeaders.Add("Origin", "http://e.kgeu.ru");

                    httpclient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
                    #endregion
                }
                return httpclient;
            }
        }

        public HttpProvider(CookieCollection _collection = null)
        {
        }
        public HttpProvider(Uri baseuri, CookieCollection _collection=null)
        {
            BaseUri = baseuri;

            Cookiecontainer = new CookieContainer();
            if(_collection!=null)
            Cookiecontainer.Add(BaseUri, _collection);
        }


        #region events

        /// <summary>
        /// данные post-запроса неверны
        /// </summary>
        public event EventHandler<MessageEvent> PostDataisIncorrect = delegate { };

        /// <summary>
        /// токены устарели, неоходим повторный вход(получение токенов)
        /// </summary>
        public event EventHandler<MessageEvent> AuthenticationTokensOutdated = delegate { };

        /// <summary>
        /// невозможно получить ответ
        /// </summary>
        public event EventHandler<MessageEvent> ResponseImpossibleTo = delegate { };

        #endregion

        #region Methods: get, post
        /// <summary>
        /// Http get-запрос к сайту http://e.kgeu.ru
        /// </summary>
        /// <param name="uri">2 часть URI вида "/uri"</param>
        /// <returns>ответ от сервера, типа HttpResponseMessage или null(если ответ не получен\не верен)</returns>
        public async Task<HttpResponseMessage> HttpMethodGet(string uri)
        {
            try
            {
                HttpResponseMessage response = await Httpclient.GetAsync(uri);
               
                if (response.StatusCode == HttpStatusCode.OK)
                    return response;
                else if (response.StatusCode == HttpStatusCode.Found)
                {
                    AuthenticationTokensOutdated(this, new MessageEvent("Токены устарели, неоходим повторный вход(получение токенов)"));
                    Debug.WriteLine("get-метод: {0}", "Токены устарели");
                    return null;
                }
                else
                    throw new Exception(response.StatusCode.ToString());

            }
            catch (Exception ex)
            {
                Debug.WriteLine("get-метод, ответ не получен:  {0}", ex.Message);
                ResponseImpossibleTo(this, new MessageEvent("get-метод, ответ не получен: " + ex.Message.ToString()));
                return null;
            }
        }





        //var values = new Dictionary<string, string>();
        //values.Add("UserType", "2");
        //var content = new FormUrlEncodedContent(values);


        /// <summary>
        /// Http post-запрос к сайту http://e.kgeu.ru
        /// </summary>
        /// <param name="uri">2 часть URI вида "/uri"</param>
        /// <param name="data">данные отправляемые в запросе</param>
        /// <returns>ответ от сервера, типа HttpResponseMessage или null(если ответ не получен\не верен)</returns>
        public async Task<HttpResponseMessage> HttpMethodPost(string uri, FormUrlEncodedContent data)
        {
            //foreach (KeyValuePair<string, string> h in headers)
            //    Httpclient.DefaultRequestHeaders.Add(h.Key, h.Value);
            try
            {
                HttpResponseMessage response = await Httpclient.PostAsync(uri, data);


                if (response.StatusCode == HttpStatusCode.Found)
                    return response;
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    PostDataisIncorrect(this, new MessageEvent("данные post-запроса неверны"));
                    Debug.WriteLine("post-метод: {0}", "данные post - запроса неверны");
                    return null;
                }
                else
                    throw new Exception(response.StatusCode.ToString());

            }
            catch (Exception ex)
            {
                Debug.WriteLine("post-метод, ответ не получен:   {0}", ex.Message);
                ResponseImpossibleTo(this, new MessageEvent("post-метод, ответ не получен: " + ex.Message.ToString()));
                return null;
            }
        }
        #endregion
    }

}
