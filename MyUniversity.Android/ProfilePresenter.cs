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
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.ÑommonCode;

namespace MyUniversity.Android
{
    class ProfilePresenter
    {
        public readonly IViewProfile _view;
        public readonly IProfileModel _model;
        public readonly IStorageServise _stor;



        public ProfilePresenter(IViewProfile view, IProfileModel model, IStorageServise stor)
        {
            this._view = view;
            this._model = model;
            this._stor = stor;



            _model.AccountIncorrect += _model_AccountIncorrect;
            _model.LoadImage += _model_LoadImage;
            _model.NoNetwork += _model_NoNetwork;
            _model.UpdatingStydentProfile += _model_UpdatingStydentProfile;

            _model.LogoutSucsessfull += _model_LogoutSucsessfull;
            _model.LogoutError += _model_LogoutError;
        }

        private void _model_LogoutError(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewErrorLogOut();
        }

        private void _model_LogoutSucsessfull(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.Logout();
        }

        private void _model_UpdatingStydentProfile(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewProfile(e.Item as StydentProfile);
        }

        private void _model_NoNetwork(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewErrorNoNetwork();
        }

        private void _model_LoadImage(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _stor.LoadImage(e.Item as byte[]);
        }

        private void _model_AccountIncorrect(object sender, Core.Ñommon_Code.MessageEvent e)
        {
            _view.ViewErrorAccountIncorrect();
        }



        public void GetProfile()
        {
            _model.GetStydentProfile();
        }


        public async void Logout()
        {
            await _model.Logout();
        }


    }


}
 