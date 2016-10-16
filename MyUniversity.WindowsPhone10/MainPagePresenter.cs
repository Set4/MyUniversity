using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.Model;
using MyUniversity.Core.NotificationModel;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.RatingModel;
using MyUniversity.Core.ScheduleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyUniversity.WindowsPhone10
{

   

    public class MainPagePresenter
    {
        public readonly IViewMainPage _view;

        public readonly IMainModel _model;


        public MainPagePresenter(IViewMainPage view, IMainModel model)
        {
            _view = view;
            _model = model;

            _model.NoNetwork += _model_NoNetwork;
            _model.IncorrectAuthData += _model_IncorrectAuthData;
            _model.AccessToSiteProblem += _model_AccessToSiteProblem;
        }

        private void _model_AccessToSiteProblem(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewMessage(e.Message);
        }

        private void _model_IncorrectAuthData(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewMessage("Проведена повторная авторизация. Повторите попытку получения данных");
        }

        private void _model_NoNetwork(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewMessage(e.Message);
        }

        public async Task<StydentProfile> GetProfile()
        {
            return new StydentProfile(await _model.GetProfile());
        }


        public async Task<List<Notification>> GetNotification()
        {
            return await _model.GetNotifications();
        }

        public async Task<List<Lesson>> GetLessons()
        {
            return await _model.GetLessons();
        }

       
        public async Task<Tuple<List<WeekData>, List<ScheduleItem>>> GetSchedulse()
        {

            return await _model.GetAllSchedules();
        }


    }
}
