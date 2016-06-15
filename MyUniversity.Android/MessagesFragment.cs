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
using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.NotificationModel;
using Android.Support.Design.Widget;
using Newtonsoft.Json;

namespace MyUniversity.Android
{

  
    public class MessagesFragment : Fragment
    {
        
        View view;
        LinearLayout liner;
        ListView listview;

       

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


         

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            view = inflater.Inflate(Resource.Layout.MessagesLayout, container, false);
            //return base.OnCreateView(inflater, container, savedInstanceState);



            var myActivity = (MenuActivity)this.Activity;


        


            liner = view.FindViewById<LinearLayout>(Resource.Id.message_layout);

            listview = view.FindViewById<ListView>(Resource.Id.message_listview);

            return view;
        }

        public void ViewNotifications(List<Core.NotificationModel.Notification> items)
        {
            //rabotaet?
            MessagesListViewAdapter adapter = new MessagesListViewAdapter(this.Activity, items);
            listview.Adapter = adapter;
        }

      
    }


    class MessagesListViewAdapter : BaseAdapter<Core.NotificationModel.Notification>
    {

        public List<Core.NotificationModel.Notification> _items;
        private Context _context;

        public MessagesListViewAdapter(Context context, List<Core.NotificationModel.Notification> items)
        {
            _context = context;
            _items = items;
        }

        public override Core.NotificationModel.Notification this[int position]
        {
            get
            {
                return _items[position];
            }
        }



        public override int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }



        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row != null)
                row = LayoutInflater.From(_context).Inflate(Resource.Layout.MessageItemLayout, null, false);

            TextView txt_header = row.FindViewById<TextView>(Resource.Id.message_header);
            txt_header.Text = _items[position].Header;

            TextView txt_date = row.FindViewById<TextView>(Resource.Id.message_date);
            txt_date.Text = _items[position].Date;

            TextView txt_text = row.FindViewById<TextView>(Resource.Id.message_text);
            txt_text.Text = _items[position].Text;

            return row;
        }
    }


}