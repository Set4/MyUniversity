using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.AuthenticationModel;
using Android.Support.Design.Widget;
using Newtonsoft.Json;
using Android.Graphics;

namespace MyUniversity.Android
{

    public interface IViewProfile
    {
        void ViewProfile(StydentProfile prof);

        void ViewErrorLogOut();

        void ViewErrorNoNetwork();
        void ViewErrorAccountIncorrect();

        void Logout();
    }
    public class ProfileFragment : Fragment, IViewProfile
    {
        private ProfilePresenter presenter;
        AuthentificationModel _auth;

        View view;



        Button btn_logout;
        LinearLayout liner;
        ImageView image_profile;
        TextView txtview;

        //JsonConvert.DeserializeObject<AuthentificationModel>(Intent.GetStringExtra("_auth")
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here




            



        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
  
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
             view = inflater.Inflate(Resource.Layout.ProfileLayout, container, false);
            //return base.OnCreateView(inflater, container, savedInstanceState);



            var myActivity = (MenuActivity)this.Activity;


            _auth = myActivity._auth;
            presenter = new ProfilePresenter(this, new ProfileModel(_auth,
                         new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),
                         System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)),
                         new StorageServise());




            presenter.GetProfile();

            liner = view.FindViewById<LinearLayout>(Resource.Id.profile_layout);

            image_profile = view.FindViewById<ImageView>(Resource.Id.profile_image);



            btn_logout = view.FindViewById<Button>(Resource.Id.btn_login);
            btn_logout.Click += Btn_logout_Click;



            return view;
        }

        private void Btn_logout_Click(object sender, EventArgs e)
        {
            Snackbar.Make(liner, "Выход из Аккаунта", Snackbar.LengthLong)
              .SetAction("Выход", (v) =>
              {

                  presenter.Logout();

              }).Show();
           
        }

        public void ViewErrorLogOut()
        {
            Snackbar.Make(liner, "Ошибка Выхода Повторите попытку позже.", Snackbar.LengthLong)
               .SetAction("OK", (v) =>{  }).Show();
        }

        public async void ViewErrorNoNetwork()
        {
            Snackbar.Make(liner, "Ошибка NetSeti. Повторите попытку позже.", Snackbar.LengthLong)
              .SetAction("OK", (v) => { }).Show();
        }

        public async void ViewErrorAccountIncorrect()
        {

            Snackbar.Make(liner, "acc incorr", Snackbar.LengthLong)
               .SetAction("OK", (v) =>
               {

                   var intent = new Intent(this.Activity, typeof(AuthintificationActivity));

                   intent.PutExtra("_auth", JsonConvert.SerializeObject(_auth));
                   this.StartActivity(intent);
                  
               }).Show();
        }

        public void Logout()
        {

            Snackbar.Make(liner, "acc vihod", Snackbar.LengthLong).Show();
            var intent = new Intent(this.Activity, typeof(AuthintificationActivity));

                   intent.PutExtra("_auth", JsonConvert.SerializeObject(_auth));
                   this.StartActivity(intent);
        }

        public void ViewProfile(StydentProfile prof)
        {
            /*
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult( Intent.CreateChooser(imageIntent, "Select photo"), 0);
            */
            image_profile.SetImageBitmap(BitmapFactory.DecodeFile(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "imageprof.jpg"));

            txtview = view.FindViewById<TextView>(Resource.Id.profile_email);
            txtview.Text = prof.Email;

            txtview = view.FindViewById<TextView>(Resource.Id.profile_Group);
            txtview.Text = prof.GroupNumber;

            txtview = view.FindViewById<TextView>(Resource.Id.profile_Department);
            txtview.Text = prof.Department;

            txtview = view.FindViewById<TextView>(Resource.Id.profile_Chair);
            txtview.Text = prof.Chair;

            txtview = view.FindViewById<TextView>(Resource.Id.profile_Specialty);
            txtview.Text = prof.Specialty;

            txtview = view.FindViewById<TextView>(Resource.Id.profile_TrainingProfile);
            txtview.Text = prof.TrainingProfile;

            txtview = view.FindViewById<TextView>(Resource.Id.profile_ModeofStudy);
            txtview.Text = prof.ModeofStudy;
        }

      

    }
}