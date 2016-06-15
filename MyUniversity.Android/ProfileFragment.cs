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

   
    public class ProfileFragment : Fragment
    {
      
        View view;
        Button btn_logout;
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

            image_profile = view.FindViewById<ImageView>(Resource.Id.profile_image);



            btn_logout = view.FindViewById<Button>(Resource.Id.btn_login);
            btn_logout.Click += Btn_logout_Click;



            return view;
        }

        private void Btn_logout_Click(object sender, EventArgs e)
        {
            
           
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