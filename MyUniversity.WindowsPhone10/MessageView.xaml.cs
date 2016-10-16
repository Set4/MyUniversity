using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyUniversity.Core;
using MyUniversity.Core.NotificationModel;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyUniversity.WindowsPhone10
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ViewMessage : Page
    {
        public Notification mess;
        public ViewMessage()
        {
            this.InitializeComponent();
          
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mess= e.Parameter as Notification;
            
            txblckDate.Text =mess.Date;
            txblckHeader.Text = mess.Header;
            txblockText.Text = mess.Text;

        }
    }
}
