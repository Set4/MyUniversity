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
using Android.Support.V7.Widget;
using MyUniversity.Core.ScheduleModel;

namespace MyUniversity.Android
{
    public class WeekViewHolder : RecyclerView.ViewHolder
    {
        public TextView MovieNameTextView { get; set; }


        public WeekViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            MovieNameTextView = itemView.FindViewById<TextView>(Resource.Id.schedule_header);

            itemView.Click += (s, e) => listener(Position);
        }
    }
    public class WeekAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        private readonly List<WeekData> movies;

        public WeekAdapter(List<WeekData> movies)
        {
            this.movies = movies;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var movieViewHolder = (WeekViewHolder)holder;
            movieViewHolder.MovieNameTextView.Text = movies[position].Key + " медекъ";

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ScheduleHeaderLayout, parent, false);

            return new WeekViewHolder(layout, OnItemClick);

        }

        public override int ItemCount
        {
            get { return movies.Count; }
        }

        void OnItemClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }


    public class DaysViewHolder : RecyclerView.ViewHolder
    {
        public TextView MovieNameTextView { get; set; }


        public DaysViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            MovieNameTextView = itemView.FindViewById<TextView>(Resource.Id.schedule_header);

            itemView.Click += (s, e) => listener(Position);
        }
    }
    public class DaysAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        private readonly List<DayData> movies;

        public DaysAdapter(List<DayData> movies)
        {
            this.movies = movies;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var movieViewHolder = (WeekViewHolder)holder;
            movieViewHolder.MovieNameTextView.Text = movies[position].Date + " "+ movies[position].DayWeek;

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ScheduleHeaderLayout, parent, false);

            return new WeekViewHolder(layout, OnItemClick);

        }

        public override int ItemCount
        {
            get { return movies.Count; }
        }

        void OnItemClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }


    public class ScheduleFragment : Fragment
    {
        View view;
        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;

        WeekAdapter weeksAdapter;
        List<WeekData> items;
        List<DayData> daysitems;

        bool isweek = true;

        DaysAdapter daysAdapter;

        ListView list;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            view = inflater.Inflate(Resource.Layout.LessonsLayout, container, false);
            //return base.OnCreateView(inflater, container, savedInstanceState);



            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.schedule_recyclerView);

            layoutManager = new LinearLayoutManager(this.Activity, LinearLayoutManager.Horizontal, false);

            recyclerView.SetLayoutManager(layoutManager);

            list = view.FindViewById<ListView>(Resource.Id.schedule_listview);

            weeksAdapter.ItemClick += MoviesAdapter_ItemClick;
            recyclerView.LongClick += RecyclerView_LongClick;

            return view;
        }



        public void SetSchedule(List<WeekData> items)
        {
            this.items = items;


            daysitems = new List<DayData>();
            foreach (WeekData item in items)
                daysitems.AddRange(item.Days);

        }



        private void RecyclerView_LongClick(object sender, View.LongClickEventArgs e)
        {
            if (isweek)
                isweek = false;
            else
                isweek = true;

            ViewSchedule();
        }




        public void ViewSchedule()
        {
            if (isweek)
            {
                weeksAdapter = new WeekAdapter(items);
                recyclerView.SetAdapter(weeksAdapter);
            }
            else
            {
                
                daysAdapter = new DaysAdapter(daysitems);
                recyclerView.SetAdapter(daysAdapter);
            }

        }




        private void MoviesAdapter_ItemClick(object sender, int e)
        {
            
            

                if(isweek)
                {
                    if (items.Count >= e)
                    {
                        DayListViewAdapter adapter = new DayListViewAdapter(this.Activity, items[e].Days);
                        list.Adapter = adapter;
                    }
                }
                else
                {
                    if (daysitems.Count >= e)
                    {
                    ScheduleListViewAdapter adapter = new ScheduleListViewAdapter(this.Activity, daysitems[e].Schedules);
                        list.Adapter = adapter;
                    }
                }
             
            

        }

    }



 

    class ScheduleListViewAdapter : BaseAdapter<ScheduleItem>
    {

        public List<ScheduleItem> _items;
        private Context _context;

        public ScheduleListViewAdapter(Context context, List<ScheduleItem> items)
        {
            _context = context;
            _items = items;
        }

        public override ScheduleItem this[int position]
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
                row = LayoutInflater.From(_context).Inflate(Resource.Layout.ScheduleItemLayout, null, false);

            

            return row;
        }
    }


    class DayListViewAdapter : BaseAdapter<DayData>
    {

        public List<DayData> _items;
        private Context _context;

        public DayListViewAdapter(Context context, List<DayData> items)
        {
            _context = context;
            _items = items;
        }

        public override DayData this[int position]
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
                row = LayoutInflater.From(_context).Inflate(Resource.Layout.DayItemLayout, null, false);

            

            return row;
        }
    }
}