using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyUniversity.Core.ProfileModel
{
    public class StydentProfile
    {

        #region Свойства и поля
      

        private string groupNumber = null;
        /// <summary>
        /// Номер группы
        /// </summary>
        public string GroupNumber
        {
            get { return groupNumber; }
            private set { value = groupNumber; }
        }


        private string department = null;
        /// <summary>
        /// Институт/факультет
        /// </summary>
        public string Department
        {
            get { return department; }
            private set { value = department; }
        }

        private string chair = null;
        /// <summary>
        /// Кафедра
        /// </summary>
        public string Chair
        {
            get { return chair; }
            private set { value = chair; }
        }

        private string specialty = null;
        /// <summary>
        /// Направление/специальность
        /// </summary>
        public string Specialty
        {
            get { return specialty; }
            private set { value = specialty; }
        }

        private string trainingProfile = null;
        /// <summary>
        /// Профиль обучения
        /// </summary>
        public string TrainingProfile
        {
            get { return trainingProfile; }
            private set { value = trainingProfile; }
        }

        private string modeofStudy = null;
        /// <summary>
        ///Форма обучения
        /// </summary>
        public string ModeofStudy
        {
            get { return modeofStudy; }
            private set { value = modeofStudy; }
        }

        private string imageSource = null;
        /// <summary>
        /// Расположение фото
        /// </summary>
        public string ImageSource
        {
            get { return imageSource; }
            private set
            {
                value = imageSource;
            }
        }

        private string email = null;
        /// <summary>
        /// email
        /// </summary>
        public string Email
        {
            get { return email; }
            private set { value = email; }
        }
        #endregion

        /// <summary>
        /// Картинка(url) в профиле обновилась, необходимо скачать
        /// </summary>
        public event EventHandler<MessageEvent> ImageSourseUpdated = delegate { };

        public StydentProfile(List<StorageProfile> items)
        {
            StorageProfile _stp = null;

            _stp = items.Where(i => i.Key == "GroupNumber").FirstOrDefault();
            if (_stp != null)
                groupNumber = _stp.Value;
            else
                groupNumber = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "Department").FirstOrDefault();
            if (_stp != null)
                department = _stp.Value;
            else
                department = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "Chair").FirstOrDefault();
            if (_stp != null)
                chair = _stp.Value;
            else
                chair = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "Specialty").FirstOrDefault();
            if (_stp != null)
                specialty = _stp.Value;
            else
                specialty = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "TrainingProfile").FirstOrDefault();
            if (_stp != null)
                trainingProfile = _stp.Value;
            else
                trainingProfile = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "ModeofStudy").FirstOrDefault();
            if (_stp != null)
                modeofStudy = _stp.Value;
            else
                modeofStudy = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "Email").FirstOrDefault();
            if (_stp != null)
                email = _stp.Value;
            else
                email = "Данные отсутствуют";

            _stp = items.Where(i => i.Key == "ImageSource").FirstOrDefault();
            if (_stp != null)
                imageSource = _stp.Value;
            else
                imageSource = String.Empty;

        }

        public List<StorageProfile> UpdateStydentProfile(List<StorageProfile> items)
        {
            List<StorageProfile> updatedfields = new List<StorageProfile>();

            StorageProfile _stp = null;

            _stp = items.Where(i => i.Key == "GroupNumber").FirstOrDefault(j => j.Value != groupNumber);
            if (_stp != null)
            {
                groupNumber = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "Department").FirstOrDefault(j => j.Value != department);
            if (_stp != null)
            {
                department = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "Chair").FirstOrDefault(j => j.Value != chair);
            if (_stp != null)
            {
                chair = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "Specialty").FirstOrDefault(j => j.Value != specialty);
            if (_stp != null)
            {
                specialty = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "TrainingProfile").FirstOrDefault(j => j.Value != trainingProfile);
            if (_stp != null)
            {
                trainingProfile = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "ModeofStudy").FirstOrDefault(j => j.Value != modeofStudy);
            if (_stp != null)
            {
                modeofStudy = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "Email").FirstOrDefault(j => j.Value != email);
            if (_stp != null)
            {
                email = _stp.Value;
                updatedfields.Add(_stp);
            }

            _stp = items.Where(i => i.Key == "ImageSource").FirstOrDefault();
            if (_stp != null)
            { 
                if (_stp.Value.Contains(imageSource))
                {
                    imageSource = _stp.Value;
                    updatedfields.Add(_stp);
                    ImageSourseUpdated(this, new MessageEvent("ImageSourseUpdated", _stp.Value));
                }
            }
            else
            {
                if (imageSource!=String.Empty)
                {
                    imageSource = String.Empty;
                    ImageSourseUpdated(this, new MessageEvent("ImageSourseUpdated", null));
                }
            }

            return updatedfields;
        }

    }

    /// <summary>
    /// класс для сохранения данных в бд
    /// </summary>
    public class StorageProfile : stringKey
    {
        [PrimaryKey, MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(200)]
        public string Value { get; set; }
    }

    public interface IProfileModel
    {

        event EventHandler<MessageEvent> UpdatingStydentProfile;

        event EventHandler<MessageEvent> LoadImage;

        event EventHandler<MessageEvent> NoNetwork;

        event EventHandler<MessageEvent> AccountIncorrect;

        void GetStydentProfile();

        Task Logout();

        event EventHandler<MessageEvent> LogoutSucsessfull;

        event EventHandler<MessageEvent> LogoutError;

    }

  public  class ProfileModel: IProfileModel
    {

        HttpProvider _http;
        AuthenticationModel.AuthentificationModel _account;

        SQLiteService _storage;

        //IStorageProvider _storage;


        StydentProfile _stProfile = null;
    


       static object storageLoker = new object();


        /// <summary>
        /// Время ожидания парсинга Токенов
        /// </summary>
        const int timewait = 1;


        public event EventHandler<MessageEvent> UpdatingStydentProfile = delegate { };

        public event EventHandler<MessageEvent> LoadImage = delegate { };

        public event EventHandler<MessageEvent> NoNetwork = delegate { };

        public event EventHandler<MessageEvent> AccountIncorrect = delegate { };


        public event EventHandler<MessageEvent> LogoutSucsessfull = delegate { };

        public event EventHandler<MessageEvent> LogoutError = delegate { };


        public ProfileModel(AuthenticationModel.AuthentificationModel account, ISQLitePlatform platform, string documentsPath)
        {
            _account = account;

            _storage = new SQLiteService(platform, documentsPath);

           
           
            _http = new HttpProvider(new CookieCollection()
            {
                new Cookie("__RequestVerificationToken", account._Acc.RequestVerificationToken),
                new Cookie(".ASPXAUTH", account._Acc.ASPXAUTHToken)
            });


            _stProfile = new StydentProfile(new List<StorageProfile>());



            _http.ResponseImpossibleTo += _http_ResponseImpossibleTo;
            _http.AuthenticationTokensOutdated += _http_AuthenticationTokensOutdated;

            _stProfile.ImageSourseUpdated += Stydent_ImageSourseUpdated;

            account.LoginSucsessfull += Account_LoginSucsessfull;
            account.IncorrectLoginANDPassword += Account_IncorrectLoginANDPassword;

            account.LogoutSucsessfull += Account_LogoutSucsessfull;
            account.LogoutError += Account_LogoutError;
        }



        private void Account_LogoutError(object sender, MessageEvent e)
        {
            LogoutError(this, new MessageEvent(""));
        }

        private void Account_LogoutSucsessfull(object sender, MessageEvent e)
        {
            LogoutSucsessfull(this, new MessageEvent(""));
        }

        private void Account_IncorrectLoginANDPassword(object sender, MessageEvent e)
        {
            //login or pass ne vernii vihod  v authpage + message
            AccountIncorrect(this, new MessageEvent("Account_IncorrectLoginANDPassword"));
        }

        private void Account_LoginSucsessfull(object sender, MessageEvent e)
        {
            //avtorizachia povtorena tokeni obnovleni

            Task<Task<bool>> HttpUpdateTask = new Task<Task<bool>>(async () =>
            {
                return await HttpUpdateStydentProfile();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async (t) =>
            {
                if (await t.Result)
                    UpdatingStydentProfile(this, new MessageEvent("", _stProfile));
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void _http_AuthenticationTokensOutdated(object sender, MessageEvent e)
        {
            //sobitie tokini ystareli 
            _account.LogInAccaunt(_account._Acc.Email, _account._Acc.Password);
        }



        private async void Stydent_ImageSourseUpdated(object sender, MessageEvent e)
        {
            LoadImage(this, new MessageEvent("image zagrizeno", await HttpDownloadImage(e.Item as String)));
        }

        private void _http_ResponseImpossibleTo(object sender, MessageEvent e)
        {
            NoNetwork(this, new MessageEvent("Net seti"));
        }




        private async Task<string> HttpGetProfile()
        {
            using (HttpResponseMessage response = await _http.HttpMethodGet("/Student/DekanatInfo"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return null;
            }
        }

        private async Task<byte[]> HttpDownloadImage(string uri)
        {
            using (HttpResponseMessage response = await _http.HttpMethodGet(uri))
            {
                if (response.Content.Headers.ContentType.MediaType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                    return await response.Content.ReadAsByteArrayAsync();
                else
                    return null;
            }
        }
        
        private List<StorageProfile> ParseProfile(string response)
        {
            List<StorageProfile> items = new List<StorageProfile>();

            Regex regex = new Regex("<ul.*?class=[\"]*list-group[\"]*.*?>+(.*?|\\s)+<\\/ul>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match pfofil = regex.Match(response);

            regex = new Regex("<strong>.*?<\\/li>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection info = regex.Matches(pfofil.Value);


            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            MatchCollection k = regex.Matches(info[0].Value);
            items.Add(new StorageProfile() { Key = "GroupNumber", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
            
            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[1].Value);
            items.Add(new StorageProfile() { Key = "Department", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
           
            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[2].Value);
            items.Add(new StorageProfile() { Key = "Chair", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
           
            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[3].Value);
            items.Add(new StorageProfile() { Key = "Specialty", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
           
            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[4].Value);
            items.Add(new StorageProfile() { Key = "TrainingProfile", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
           
            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[5].Value);
            items.Add(new StorageProfile() { Key = "ModeofStudy", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
            
            regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            k = regex.Matches(info[6].Value);
            items.Add(new StorageProfile() { Key = "Email", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });
           

            Regex reg = new Regex("<img.*?alt=[\"]*Фотография[\"]*.*?>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            Match imagesourse = reg.Match(response);

            reg = new Regex("\\/Image\\/UserPhoto\\/[^\\s]*\"", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timewait));
            imagesourse = reg.Match(response);
            items.Add(new StorageProfile() { Key = "ImageSource", Value = imagesourse.Value.Substring(0, imagesourse.Value.Length - 1) });
            

            return items;
        }

        private List<StorageProfile> SearChchangetFields(List<StorageProfile> items)
        {
            List<StorageProfile> changetfields = new List<StorageProfile>();

            if (_stProfile != null)
                changetfields = _stProfile.UpdateStydentProfile(items);
            else
            {
                _stProfile = new StydentProfile(new List<StorageProfile>());
                changetfields = _stProfile.UpdateStydentProfile(items);
            }

            
          

            return changetfields;
        }

        private async void UpdateProfile(List<StorageProfile> items)
        {
            foreach (StorageProfile item in items)
              await  _storage.InsertOrReplaceItem<StorageProfile>(item);

        }

       private Task<bool> StorageUpdateStydentProfile()
        {
            Monitor.Enter(storageLoker);
            try
            {
                List<StorageProfile> changetfields = _stProfile.UpdateStydentProfile( _storage.GetAllItems<StorageProfile>().Result);
                if (changetfields.Count > 0)
                    return Task.Run(() =>
                    {
                        return true;
                    });
                else
                    return Task.Run(() =>
                    {
                        return false;
                    });

            }
            finally
            {
                Monitor.Exit(storageLoker);
            }

        }


      


        public async Task Logout()
        {
          await  _account.LogOutAccaunt();
        }




        private Task<bool> HttpUpdateStydentProfile()
        {
            
            string response =  HttpGetProfile().Result;

            List<StorageProfile> asd = null;

            if (response != null)
                asd = ParseProfile(response);

            List<StorageProfile> asss = null;


            //
            Task.Delay(5000).Wait();
            //


            Monitor.Enter(storageLoker);
            try
            {

                if (asd != null && asd.Count != 0)
                    asss = SearChchangetFields(asd);

                if (asss != null && asss.Count != 0)
                {

                    UpdateProfile(asss);

                    return Task.Run(() =>
                    {
                        return true;
                    });
                }
                else
                    return Task.Run(() =>
                    {
                        return false;
                    });
            }
            finally
            {
                Monitor.Exit(storageLoker);
               
            }

          
        }
      


        public async void GetStydentProfile()
        {
          
            UpdatingStydentProfile(this, new MessageEvent("", _stProfile));


           // //oshibka
           //if(await StorageUpdateStydentProfile())
           //     UpdatingStydentProfile(this, new MessageEvent("", _stProfile));


            Task<Task<bool>> StorageUpdateTask = new Task<Task<bool>>(async () =>
            {
                return await StorageUpdateStydentProfile();
                   
            });

            StorageUpdateTask.Start();

            Task taskUI1 = StorageUpdateTask.ContinueWith(async (t) =>
            {
                if (await t.Result)
                    UpdatingStydentProfile(this, new MessageEvent("", _stProfile));
            }, TaskScheduler.FromCurrentSynchronizationContext());



            Task<Task<bool>> HttpUpdateTask = new Task<Task<bool>>(async () =>
            {
                return await HttpUpdateStydentProfile();
            });

            HttpUpdateTask.Start();

            Task taskUI2 = HttpUpdateTask.ContinueWith(async(t) =>
             {
                if(await t.Result)
                     UpdatingStydentProfile(this, new MessageEvent("", _stProfile));
             }, TaskScheduler.FromCurrentSynchronizationContext());
  
        }
    }
}
