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

using Java.Lang;
using MyUniversity.Core.RatingModel;
using Android.Support.Design.Widget;
using MyUniversity.Core.AuthenticationModel;
using Android.Support.V4.View;
using Newtonsoft.Json;

using Android.Support.V7.Widget;

namespace MyUniversity.Android
{
    public class LessonViewHolder : RecyclerView.ViewHolder
    {
        public TextView MovieNameTextView { get; set; }
        public ImageView MovieImageView { get; set; }

        public LessonViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            MovieNameTextView = itemView.FindViewById<TextView>(Resource.Id.cardViewText);
    
            itemView.Click += (s, e) => listener(Position);
        }
    }
    public class LessonAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        private readonly List<Lesson> movies;

        public LessonAdapter(List<Lesson> movies)
        {
            this.movies = movies;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var movieViewHolder = (LessonViewHolder)holder;
            movieViewHolder.MovieNameTextView.Text = movies[position].NameLesson;
       
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LessonHeaderLayout, parent, false);

            return new LessonViewHolder(layout, OnItemClick);

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

   

    public class LessonsFragment :Fragment
    {
        View view;
       

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



 recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lesson_recyclerView);

            layoutManager = new LinearLayoutManager(this.Activity, LinearLayoutManager.Horizontal, false);

            recyclerView.SetLayoutManager(layoutManager);


 moviesAdapter.ItemClick += MoviesAdapter_ItemClick;

            return view;
        }



        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;

        LessonAdapter moviesAdapter;
        List<Lesson> items;

     
        public void ViewLessons(List<Lesson> items)
        {
            this.items = items;
            moviesAdapter = new LessonAdapter(items);

            recyclerView.SetAdapter(moviesAdapter);

        }

        private void MoviesAdapter_ItemClick(object sender, int e)
        {
            if(items.Count>=e)
            {
                var textv = view.FindViewById<TextView>(Resource.Id.lesson_nameLesson);
                textv.Text = items[e].NameLesson;
            }

        }
    



    }

    
}