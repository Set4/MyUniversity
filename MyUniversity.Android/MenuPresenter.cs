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

namespace MyUniversity.Android
{
   public class MenuPresenter
    {
        public readonly IViewMainPage _view;

        public readonly IAuthentificationModel _model;

        public MenuPresenter(IViewMainPage view, IAuthentificationModel model)
        {
            _view = view;
            _model = model;
        }

    }
}