using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.NotificationModel;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.RatingModel;
using MyUniversity.Core.ScheduleModel;
using MyUniversity.Core.Сommon_Code;
using MyUniversity.Core.СommonCode;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.Model
{
    public interface IMainModel
    {
        event EventHandler<MessageEvent> IncorrectAuthData;
        event EventHandler<MessageEvent> NoNetwork;
        event EventHandler<MessageEvent> AccessToSiteProblem;

        Task<List<StorageProfile>> GetProfile();
        Task<List<Notification>> GetNotifications();
        Task UpdateNotofication(string key);
        Task<List<Lesson>> GetLessons();

        Task<List<WeekData>> GetWeeks();
        Task<List<ScheduleItem>> GetSchedules(int evenweek, int dayweek, string date);
        Task<Tuple<List<WeekData>, List<ScheduleItem>>> GetAllSchedules();
    }
    public class MainModel : IMainModel
    {

        HttpProvider _http;
        AuthentificationService _authservice;
        ProfileService _profservice;
        NotificationService _notificationService;
        LessonsService _lessonsService;
        ScheduleService _scheduleservice;

        Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> _authdata;

        //list Notification
        //obj profile
        //obj acc

        public MainModel(ISQLitePlatform platform, string documentsPath, IStorageServise storageservice, Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> authdata)
        {
            _authdata = authdata;
            _http = new HttpProvider(new Uri("http://e.kgeu.ru"), new CookieCollection()
            {
                 new Cookie("__RequestVerificationToken", _authdata.Item3.Value),
                 new Cookie(".ASPXAUTH", _authdata.Item4.Value)
            });

            _authservice = new AuthentificationService(new HttpProvider(new Uri("http://e.kgeu.ru")), platform, documentsPath);
            _profservice = new ProfileService(_http, platform, documentsPath, storageservice);
            _notificationService = new NotificationService(_http, platform, documentsPath);
            _lessonsService = new LessonsService(_http, platform, documentsPath);
            _scheduleservice = new ScheduleService(_http, platform, documentsPath);

            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo;
            _http.PostDataisIncorrect += _http_PostDataisIncorrect;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated;
        }

       

        #region events
        public event EventHandler<MessageEvent> IncorrectAuthData = delegate { };
        public event EventHandler<MessageEvent> NoNetwork = delegate { };
        public event EventHandler<MessageEvent> AccessToSiteProblem = delegate { };
        public event EventHandler<MessageEvent> AutarizationDataUpdated = delegate { };


        private void _http_AuthenticationTokensOutdated(object sender, MessageEvent e)
        {
            UpdatingAutharizationData();
        }

        private void _http_PostDataisIncorrect(object sender, MessageEvent e)
        {
            AccessToSiteProblem(this, new MessageEvent(@"Неверные Username и\или Password"));
        }

        private void _http_ResponseImpossibleTo(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("Нет соединения"));
        }
        #endregion

        private async void UpdatingAutharizationData()
        {
            _authdata = await _authservice.LogIn(_authdata.Item1.Value, _authdata.Item2.Value);

            if (_authdata != null)
            {
                AutarizationDataUpdated(this, new MessageEvent("Данные доступа обновлены повторите попытку получения данных"));
                _http = new HttpProvider(new Uri("http://e.kgeu.ru"), new CookieCollection()
            {
                 new Cookie("__RequestVerificationToken", _authdata.Item3.Value),
                 new Cookie(".ASPXAUTH", _authdata.Item4.Value)
            });
            }
        }

        private async Task LoadAutharizationData()
        {
            Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> authdata = await _authservice.LoadAutharizationData();
            
            _http = new HttpProvider(new Uri("http://e.kgeu.ru"),    new CookieCollection() {        
                 new Cookie("__RequestVerificationToken", authdata.Item3.Value),
                    new Cookie(".ASPXAUTH", authdata.Item4.Value)
             });
        }




        public async Task<List<StorageProfile>> GetProfile()
        {
            return await _profservice.GetProfile();
        }

        public async Task<List<Notification>> GetNotifications()
        {
            return await _notificationService.GetNotification();
        }

        public async Task UpdateNotofication(string key)
        {
            await _notificationService.Update(key);
        }

        public async Task<List<Lesson>> GetLessons()
        {
            return await _lessonsService.GetLessons();
        }

        public async Task<Tuple<List<WeekData>, List<ScheduleItem>>> GetAllSchedules()
        {
            return new Tuple<List<WeekData>, List<ScheduleItem>>(await _scheduleservice.GetWeeks(), await _scheduleservice.GetAllShedules());
        }

        public async Task<List<ScheduleItem>> GetSchedules(int evenweek, int dayweek, string date)
        {
            return await _scheduleservice.GetSheduleItems(evenweek, dayweek, date);
        }

        public async Task<List<WeekData>> GetWeeks()
        {
            return await _scheduleservice.GetWeeks();
        }
    }
}
