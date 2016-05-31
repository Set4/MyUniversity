using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using MyUniversity.Core.AuthenticationModel;
using Newtonsoft.Json;
using MyUniversity.Core.СommonCode;
using Android.Graphics;
using Java.IO;

namespace MyUniversity.Android
{
    [Activity(Label = "MyUniversity.Android")]
    public class MainActivity : AppCompatActivity
    {


        public AuthentificationModel _authmodel;





      //  DrawerLayout drawerLayout;




        protected async override void OnCreate(Bundle savedInstanceStater)
        {
            base.OnCreate(savedInstanceStater);

            _authmodel = new AuthentificationModel(
                new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), 
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

            _authmodel.AccauntLoadet += _authmodel_AccauntLoadet;
            _authmodel.AccauntNotLoadet += _authmodel_AccauntNotLoadet;


           await _authmodel.LoadAccount();

 /*
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            
           
            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
   
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            //load default home screen
            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Add(Resource.Id.HomeFrameLayout, new HomeFragment());
            ft.Commit();

            // Get our button from the layout resource,
            // and attach an event to it


            // Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            */
}

        private void _authmodel_AccauntNotLoadet(object sender, Core.Сommon_Code.MessageEvent e)
        {
            var intent = new Intent(this, typeof(AuthintificationActivity));
            intent.PutExtra("_auth", JsonConvert.SerializeObject(_authmodel));
            this.StartActivity(intent);
            this.Finish();
        }

        private void _authmodel_AccauntLoadet(object sender, Core.Сommon_Code.MessageEvent e)
        {
            var intent = new Intent(this, typeof(MenuActivity));
            intent.PutExtra("_auth", JsonConvert.SerializeObject(_authmodel));
            this.StartActivity(intent);
            this.Finish();
        }

        /*
        //define custom title text
        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.app_name);
            base.OnResume();
        }

        //define action for navigation menu selection
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_home):
                    Toast.MakeText(this, "Home selected!", ToastLength.Short).Show();
                    break;
                case (Resource.Id.nam_messages):
                    Toast.MakeText(this, "Message selected!", ToastLength.Short).Show();
                    break;
                case (Resource.Id.nav_friends):
                    // React on 'Friends' selection
                    break;
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }

        //add custom icon to tolbar
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            if (menu != null)
            {
                menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
                menu.FindItem(Resource.Id.ation_attach).SetVisible(false);
            }
            return base.OnCreateOptionsMenu(menu);
        }

        //define action for tolbar icon press
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.home:
                    //this.Activity.Finish();
                    return true;
                case Resource.Id.ation_attach:
                    //FnAttachImage();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        //to avoid direct app exit on backpreesed and to show fragment from stack
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
            {
                FragmentManager.PopBackStack();// fragmentManager.popBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

    */
    }






    class StorageServise : IStorageServise
    {
        public async void LoadImage(byte[] image)
        {

            Bitmap bm = BitmapFactory.DecodeByteArray(image, 0, image.Length);          
            File myPath = new File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "imageprof.jpg");

            try
            {
                using (var os = new System.IO.FileStream(myPath.AbsolutePath, System.IO.FileMode.Create))
                {
                    bm.Compress(Bitmap.CompressFormat.Jpeg, 100, os);
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
            }

        }

    }


}

