using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.NotificationModel;
using MyUniversity.Core.ÑommonCode;
using MyUniversity.Core.RatingModel;
using MyUniversity.Core.ScheduleModel;

namespace MyUniversity.Android
{
    public class MenuPresenter
    {
        readonly IViewMainPage _view;

        readonly IStorageServise _storage;

        readonly IProfileModel _modelprofile;

        readonly INotificationModel _modelmessages;

        readonly IRatingModel _modelrating;

        readonly IScheduleModel _modelschedule;

        public MenuPresenter(IViewMainPage view, IStorageServise storage, IProfileModel modelprofile, INotificationModel modelmessages, IRatingModel modelrating, IScheduleModel modelschedule)
        {
            _view = view;
            _storage = storage;
            _modelprofile = modelprofile;
            _modelmessages = modelmessages;
            _modelrating = modelrating;
            _modelschedule = modelschedule;

            _modelprofile.LoadImage += _modelprofile_LoadImage;

            _modelprofile.AccountIncorrect += _AccountIncorrect;
            _modelprofile.NoNetwork += _NoNetwork;

            _modelprofile.LogoutError += _modelprofile_LogoutError;
            _modelprofile.LogoutSucsessfull += _modelprofile_LogoutSucsessfull;

            _modelprofile.UpdatingStydentProfile += _modelprofile_UpdatingStydentProfile;



            _modelmessages.AccountIncorrect += _AccountIncorrect;
            _modelmessages.NoNetwork += _NoNetwork;

            _modelmessages.UpdatingNotification += _modelmessages_UpdatingNotification;


            _modelrating.AccountIncorrect += _AccountIncorrect;
            _modelrating.NoNetwork += _NoNetwork;


            _modelrating.UpdatingRating += _modelrating_UpdatingRating;

            _modelschedule.AccountIncorrect += _AccountIncorrect;
            _modelschedule.NoNetwork += _NoNetwork;
            _modelschedule.ScheduleItemsUpdated += _modelschedule_ScheduleItemsUpdated;
            _modelschedule.WeeksListCreated += _modelschedule_WeeksListCreated;

        }

        private void _modelschedule_WeeksListCreated(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.SetSchedule(e.Item as List<WeekData>);
            _view.ViewSchedule();
        }

        private void _modelschedule_ScheduleItemsUpdated(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _modelschedule.CreateListWeeks();
        }

        private void _modelrating_UpdatingRating(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewRating(e.Item as List<Lesson>);
        }

        private void _modelmessages_UpdatingNotification(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewMessages(e.Item as List<Core.NotificationModel.Notification>);
        }


        private void _modelprofile_UpdatingStydentProfile(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewProfile(e.Item as StydentProfile);
        }

        private void _modelprofile_LoadImage(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _storage.LoadImage(e.Item as byte[]);
        }

        private void _AccountIncorrect(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewErrorAccountIncorrect();
        }

        private void _NoNetwork(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewErrorNoNetwork();
        }

        private void _modelprofile_LogoutSucsessfull(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewLogoutSucsessfull();
        }

        private void _modelprofile_LogoutError(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewErrorLogOut();
        }


        public void GetProfile()
        {
            _modelprofile.GetStydentProfile();
        }

        public void LogOut()
        {
            _modelprofile.Logout();
        }

        public void GetMessages()
        {
            _modelmessages.GetNotifications();
        }

        public void GetRating()
        {
            _modelrating.GetRating();
        }

        public void GetSchedule()
        {
            _modelschedule.GetSchedule();
        }

    }
}