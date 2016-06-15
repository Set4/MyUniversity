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
using Newtonsoft.Json;

using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Graphics;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.NotificationModel;
using MyUniversity.Core.RatingModel;
using MyUniversity.Core.ScheduleModel;

namespace MyUniversity.Android
{
    public interface IViewMainPage
    {
        void ViewProfileNavHeader();

        void ViewProfile(StydentProfile prof);

        

        void ViewErrorNoNetwork();
        void ViewErrorAccountIncorrect();



        void Logout();
        void ViewLogoutSucsessfull();
        void ViewErrorLogOut();

        void ViewMessages(List<Core.NotificationModel.Notification> items);

        void ViewRating(List<Lesson> items);

        void SetSchedule(List<WeekData> items);
            void ViewSchedule();
    }

    [Activity(Label = "MenuActivity")]
    public class MenuActivity : AppCompatActivity, IViewMainPage
    {
       public AuthentificationModel _auth;
       public MenuPresenter _presenter;


        DrawerLayout drawerLayout;
        TextView txtV_email;
        ImageView imgV_profile;

        FragmentTransaction frManager;

        ProfileFragment pf;
        MessagesFragment mf;
        LessonsFragment lf;
        ScheduleFragment schf;

        Button btn_logout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            _auth = JsonConvert.DeserializeObject<AuthentificationModel>(Intent.GetStringExtra("_auth"));
            _presenter = new MenuPresenter(this,new StorageServise(), 
                         new ProfileModel(_auth, new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),
                         System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)),
                         new NotificationModel(_auth, new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),
                         System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)), 
                         new RatingModel(_auth, new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),
                         System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)), 
                         new ScheduleModel(_auth, new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),
                         System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)));

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);




            // Init toolbar
          
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            // Attach item selected handler to navigation view
            //Присоединить  обработчик навигации
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;



            // Create ActionBarDrawerToggle button and add it to the toolbar
            // var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            //drawerLayout.SetDrawerListener(drawerToggle);
            //  drawerToggle.SyncState();


            //load default home screen
            //экран загрузки по умолчанию
            frManager = FragmentManager.BeginTransaction();
            frManager.AddToBackStack(null);
            frManager.Add(Resource.Id.HomeFrameLayout, new InformationFragment());
            frManager.Commit();


           
            //otobrajenie foto i email
            ViewProfileNavHeader();


            btn_logout = FindViewById<Button>(Resource.Id.btn_login);
            btn_logout.Click += Btn_logout_Click;
        }

        //?
        private void Btn_logout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        //define action for navigation menu selection
        //
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.header_layout):
                    pf = new ProfileFragment();
                    frManager.Add(Resource.Id.HomeFrameLayout, pf);
                    frManager.Commit();
                    _presenter.GetProfile();

                    break;
                case (Resource.Id.nav_messages):
                    mf = new MessagesFragment();
                    frManager.Add(Resource.Id.HomeFrameLayout, mf);
                    frManager.Commit();
                    _presenter.GetMessages();
                    break;
                case (Resource.Id.nav_brs):
                    lf = new LessonsFragment();
                    frManager.Add(Resource.Id.HomeFrameLayout, lf);
                    frManager.Commit();
                    _presenter.GetRating();
                    break;
                case (Resource.Id.nav_schedules):
                    schf = new ScheduleFragment();
                    frManager.Add(Resource.Id.HomeFrameLayout, schf);
                    frManager.Commit();
                    _presenter.GetSchedule();
                    break;
                case (Resource.Id.nav_information):
                    frManager.Add(Resource.Id.HomeFrameLayout, new InformationFragment());
                    frManager.Commit();
                    break;
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }



        //define custom title text
        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.app_name);
            base.OnResume();
        }
        //define action for tolbar icon press
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.home:
                   // this.Activity.Finish();
                    return true;
              
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        //to avoid direct app exit on backpreesed and to show fragment from stack
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
                FragmentManager.PopBackStack();// fragmentManager.popBackStack();
            else
                base.OnBackPressed();
        }



        public void ViewProfileNavHeader()
        {

            txtV_email = FindViewById<TextView>(Resource.Id.header_email);
            txtV_email.Text = _auth._Acc.Email;

            imgV_profile = FindViewById<ImageView>(Resource.Id.header_image);
            imgV_profile.SetImageBitmap(BitmapFactory.DecodeFile
                (System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal) + "imageprof.jpg"));
        }



        public void ViewProfile(StydentProfile prof)
        {
            pf.ViewProfile(prof);
            //vizvar metod frama
            //elsi fragment eshe activen otobrapbnm novii dannii
        }


        public void Logout()
        {
            _presenter.LogOut();
        }



        public void ViewMessages(List<Core.NotificationModel.Notification> items)
        {
            mf.ViewNotifications(items);
            //vizvar metod frama
            //elsi fragment eshe activen otobrapbnm novii dannii
        }



        public void ViewErrorNoNetwork()
        {
            Snackbar.Make(drawerLayout, "Ошибка NetSeti. Повторите попытку позже.", Snackbar.LengthLong)
              .SetAction("OK", (v) => { }).Show();
        }

        public void ViewErrorAccountIncorrect()
        {
            Snackbar.Make(drawerLayout, "acc incorr", Snackbar.LengthLong)
              .SetAction("OK", (v) =>
              {

                  var intent = new Intent(this, typeof(AuthintificationActivity));

                  intent.PutExtra("_auth", JsonConvert.SerializeObject(_auth));
                  this.StartActivity(intent);

              }).Show();
        }

      

        public void ViewLogoutSucsessfull()
        {
             Snackbar.Make(drawerLayout, "acc vihod", Snackbar.LengthLong).Show();
            var intent = new Intent(this, typeof(AuthintificationActivity));

            intent.PutExtra("_auth", JsonConvert.SerializeObject(_auth));
            this.StartActivity(intent);
        }

        public void ViewErrorLogOut()
        {
            Snackbar.Make(drawerLayout, "Ошибка Выхода Повторите попытку позже.", Snackbar.LengthLong)
               .SetAction("OK", (v) => { }).Show();
        }

        public void ViewRating(List<Lesson> items)
        {
            lf.ViewLessons(items);
        }



        public void SetSchedule(List<WeekData> items)
        {
            schf.SetSchedule(items);
        }

        public void ViewSchedule()
        {
            schf.ViewSchedule();
        }
    }
}