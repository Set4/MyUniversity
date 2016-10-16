using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.Сommon_Code;
using Windows.Storage;
using System.Threading.Tasks;
using MyUniversity.Core.StartModel;
using SQLite.Net.Interop;

namespace MyUniversity.WindowsPhone10
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
   public sealed partial class App : Application
    {
        public static ISQLitePlatform platform = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
        public static string documentsPath = ApplicationData.Current.LocalFolder.Path;

        //  public AuthentificationModel _authmodel= new AuthentificationModel(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), ApplicationData.Current.LocalFolder.Path);



        Frame startFrame= Window.Current.Content as Frame;


        /// <summary>
        /// Инициализирует одноэлементный объект приложения.  Это первая выполняемая строка разрабатываемого
        /// кода; поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;



          


          //  _authmodel.AccauntLoadet += _auth_AccauntLoadet;
         //   _authmodel.AccauntNotLoadet += _auth_AccauntNotLoadet;
          
        }

        //private void _auth_AccauntNotLoadet(object sender, MessageEvent e)
        //{
        //   // Frame.Navigate(typeof(MainPage), _auth);

        //    startFrame = new Frame();
        //        startFrame.Navigate(typeof(AuthentificationPage), _authmodel);
        //        Window.Current.Content = startFrame;
        //        Window.Current.Activate();
        //}

        //private void _auth_AccauntLoadet(object sender, MessageEvent e)
        //{
        //    startFrame = new Frame();
        //    startFrame.Navigate(typeof(MainPage), _authmodel);
        //    Window.Current.Content = startFrame;
        //    Window.Current.Activate();

        //}







        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем.  Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>
        /// protected override void OnLaunched(LaunchActivatedEventArgs e)
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {


            // await _authmodel.LoadAccount();


            Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount> authdata = await StartService.VerificationAuthorized(platform, documentsPath);
            if (authdata != null)
            {
                startFrame = new Frame();
                startFrame.Navigate(typeof(MainPage), authdata);
                Window.Current.Content = startFrame;
                Window.Current.Activate();
            }
            else
            {
                startFrame = new Frame();
                startFrame.Navigate(typeof(AuthentificationPage));
                Window.Current.Content = startFrame;
                Window.Current.Activate();
            }




#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Если стек навигации не восстанавливается для перехода к первой странице,
                // настройка новой страницы путем передачи необходимой информации в качестве параметра
                // параметр



               

                //  rootFrame.Navigate(typeof(AuthentificationPage));

                //   rootFrame.Navigate(await StartModel.Start(), e.Arguments);


            }
            // Обеспечение активности текущего окна
            Window.Current.Activate();
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }
    }
}
