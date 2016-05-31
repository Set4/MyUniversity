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

namespace MyUniversity.Android
{
    public interface IViewMainPage
    {
        void ViewProfile();
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            _auth = JsonConvert.DeserializeObject<AuthentificationModel>(Intent.GetStringExtra("_auth"));
            _presenter = new MenuPresenter(this, _auth);

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
            ViewProfile();


        }
  //define action for navigation menu selection
  //
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.header_layout):
                    frManager.Add(Resource.Id.HomeFrameLayout, new ProfileFragment());
                    frManager.Commit();
                    break;
                case (Resource.Id.nav_messages):

                    break;
                case (Resource.Id.nav_brs):

                    break;
                case (Resource.Id.nav_schedules):

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

        public void ViewProfile()
        {

            txtV_email = FindViewById<TextView>(Resource.Id.header_email);
            txtV_email.Text = _auth._Acc.Email;

            imgV_profile = FindViewById<ImageView>(Resource.Id.header_image);
            imgV_profile.SetImageBitmap(BitmapFactory.DecodeFile
                (System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal) + "imageprof.jpg"));
        }

    }
}