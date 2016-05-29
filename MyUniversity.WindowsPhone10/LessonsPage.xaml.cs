﻿using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.RatingModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyUniversity.WindowsPhone10
{
    public interface ILessonsPage
    {
        void ViewProfile(List<Lesson> items);
      void  ViewErrorNoNetwork();
      void  ViewErrorAccountIncorrect();
    }
     class LessonView
    {

       
        public string Key { get; set; }

        public string NameLesson { get; set; }

      
        public int TotalMiss { get; set; }

       
        public int IndividualRating { get; set; }

      
        public int ExtraPoints { get; set; }

     
        public int TotalPoints { get; set; }

       
        public int TotalPointsPercentage { get; set; }

        public List<string> PointsInWeek { get; set; }



        public LessonView(Lesson item)
        {
            Key = item.Key;
            NameLesson = item.NameLesson;
            TotalMiss = item.TotalMiss;
            IndividualRating = item.IndividualRating;
            ExtraPoints = item.ExtraPoints;
            TotalPoints = item.TotalPoints;
            TotalPointsPercentage = item.TotalPointsPercentage;

            PointsInWeek = new List<string>();
            foreach (string i in item.PointsInWeek.Split(';'))
                if (!String.IsNullOrWhiteSpace(i))
                    PointsInWeek.Add(i);
        }
    }

    public sealed partial class LessonsPage : Page, ILessonsPage
    {
        private LessonsModelPresenter presenter;

       
        Account acc;

        public LessonsPage()
        {
            this.InitializeComponent();

           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             acc = e.Parameter as Account;
            presenter = new LessonsModelPresenter(this, new RatingModel(e.Parameter as AuthentificationModel, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), ApplicationData.Current.LocalFolder.Path));

            presenter.GetNotifications();
        }


        public void ViewProfile(List<Lesson> items)
        {
            LessonView less;
            pivotLessons.Items.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                less = new LessonView(items[i]);
                pivotLessons.Items.Add(less);
            }
        }
        public async void ViewErrorNoNetwork()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(" Повторите попытку позже.", "Ошибка NEySEti");

            await dialog.ShowAsync();
        }

        public async void ViewErrorAccountIncorrect()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
              "acc incorr");

            await dialog.ShowAsync();
            Frame.Navigate(typeof(AuthentificationPage), acc);
        }

    }

}
