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
using Android.Support.Design.Widget;
using SQLite.Net.Interop;
using Newtonsoft.Json;
using static Android.Support.V7.Widget.ActivityChooserView;

namespace MyUniversity.Android
{
   
    public interface IAuthentificationPage
    {
        void ViewEmailError();
        void ViewPasswordError();
        void ViewLogInError();

        void NotNetwork();

        void ClearViewError();

        void Navigation(IAuthentificationModel _auth);
    }

    //[Activity(Label = "AuthintificationActivity")]
    public class AuthintificationActivity : Activity, IAuthentificationPage
    {
        private AuthentificationPresenter presenter;

        EditText etxt_username;
        EditText etxt_password;
        Button btn_login;
        LinearLayout liner;
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AuthintificationLayout);
            // Create your application here

     

            presenter = new AuthentificationPresenter(this, JsonConvert.DeserializeObject<AuthentificationModel>(Intent.GetStringExtra("_auth")));

            etxt_username = FindViewById<EditText>(Resource.Id.username);
            etxt_password = FindViewById<EditText>(Resource.Id.password);

            liner=FindViewById<LinearLayout>(Resource.Id.layout); 

            btn_login = FindViewById<Button>(Resource.Id.btn_login);
            btn_login.Click += Btn_login_Click;

        }

        private void Btn_login_Click(object sender, EventArgs e)
        {
            presenter.LogInEvent(etxt_username.Text.Trim(), etxt_password.Text.Trim());          
        }

        public void ClearViewError()
        {
            
        }



        public void Navigation(IAuthentificationModel _auth)
        {
            Snackbar.Make(liner, "You are now Logged in!", Snackbar.LengthLong)
                .SetAction("Войти", (v) =>
                {

                    var intent = new Intent(this, typeof(MenuActivity));

                    intent.PutExtra("_auth", JsonConvert.SerializeObject(_auth));
                    this.StartActivity(intent);
                    this.Finish();
                }).Show();

                            /*
                             
                             Intent intent = new Intent(this, typeof(MainActivity));
        intent.PutExtra("RootData", RootObject);
        StartActivity(intent);


 if (this.Intent.Extras.ContainsKey("RootData"))
    {
        var rootData = (RootObject)this.Intent.Extras.Get("RootData");
    }




//To pass:
intent.putExtra("MyClass", obj);

// To retrieve object in second Activity
getIntent().getSerializableExtra("MyClass");




                            Activity 1

var second = new Intent(this, typeof(Activity2));
 second.PutExtra("RowID", Convert.ToString(RowID));
 StartActivity(second);

Activity 2

// Add this line to the Activity 2

string text = Intent.GetStringExtra("RowID") ?? "0";

 
                             */

                       
        }

        public void NotNetwork()
        {
            Snackbar.Make(liner, "No Network", Snackbar.LengthLong)
                       .Show();
        }

        public void ViewEmailError()
        {
            Snackbar.Make(liner, "Invalid username.", Snackbar.LengthLong)
                    .SetAction("OK", (v) => { etxt_username.Text = String.Empty; })
                    .Show();
        }

        public void ViewLogInError()
        {
            Snackbar.Make(liner, "Invalid Username or Password", Snackbar.LengthLong)
                       .SetAction("Clear", (v) => {
                           etxt_username.Text = String.Empty;
                           etxt_password.Text = String.Empty;
                       })
                        .Show();
        }

        public void ViewPasswordError()
        {
            Snackbar.Make(liner, "Invalid Password", Snackbar.LengthLong)
                       .SetAction("Clear", (v) => { etxt_password.Text = String.Empty; })
                       .Show();
        }

    }
}