using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.ProfileModel
{
    public class Profile
    {
        private string fierstName = null;
        /// <summary>
        /// Имя
        /// </summary>
        public string FierstName
        {
            get { return fierstName; }
            set { value = fierstName; }
        }

        private string lastName = null;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set { value = lastName; }
        }

        private string groupNumber = null;
        /// <summary>
        /// Номер группы
        /// </summary>
        public string GroupNumber
        {
            get { return groupNumber; }
            set { value = groupNumber; }
        }


        private string department = null;
        /// <summary>
        /// Институт/факультет
        /// </summary>
        public string Department
        {
            get { return department; }
            set { value = department; }
        }

        private string chair = null;
        /// <summary>
        /// Кафедра
        /// </summary>
        public string Chair
        {
            get { return chair; }
            set { value = chair; }
        }

        private string specialty = null;
        /// <summary>
        /// Направление/специальность
        /// </summary>
        public string Specialty
        {
            get { return specialty; }
            set { value = specialty; }
        }

        private string trainingProfile = null;
        /// <summary>
        /// Профиль обучения
        /// </summary>
        public string TrainingProfile
        {
            get { return trainingProfile; }
            set { value = trainingProfile; }
        }

        private string modeofStudy = null;
        /// <summary>
        ///Форма обучения
        /// </summary>
        public string ModeofStudy
        {
            get { return modeofStudy; }
            set { value = modeofStudy; }
        }

        private string imageSource = null;
        /// <summary>
        /// Расположение фото
        /// </summary>
        public string ImageSource
        {
            get { return imageSource; }
            set { value = imageSource; }
        }

        private string email = null;
        /// <summary>
        /// email
        /// </summary>
        public string Email
        {
            get { return email; }
            set { value = email; }
        }

        public Profile(Dictionary<string, string> items)
        {
            items.TryGetValue("FierstName", out fierstName);
            items.TryGetValue("LastName", out lastName);

            items.TryGetValue("GroupNumber", out groupNumber);
            items.TryGetValue("Department", out department);
            items.TryGetValue("Chair", out chair);
            items.TryGetValue("Specialty", out specialty);
            items.TryGetValue("TrainingProfile", out trainingProfile);
            items.TryGetValue("ModeofStudy", out modeofStudy);
            items.TryGetValue("Email", out email);
            items.TryGetValue("ImageSource", out imageSource);
        }

        public bool ImageSourceNotEmpty()
        {
            if (!String.IsNullOrEmpty(ImageSource))
                return true;
            else
                return false;
        }

        public async Task<byte[]> GetHttpImageProfile(ISQLitePlatform platform, string documentsPath, string source, HttpProfile _httpauth)
        {
            if (!String.IsNullOrEmpty(source))
            {
                return await _httpauth.HttpDownloadImage(source);
            }
            else
            {
                return null;
            }
        }
    }


    public interface IProfileData
    {
        event EventHandler<MessageEvent> ProfileUpdated;
        event EventHandler<MessageEvent> ProfileUpdatedError;
        void UpdateProfile(ISQLitePlatform platform, string documentsPath);

        void LoadProfile(ISQLitePlatform platform, string documentsPath);
        event EventHandler<MessageEvent> ProfileLoaded;
        event EventHandler<MessageEvent> ProfileLoadedError;

        void UpdateNameProfile(ISQLitePlatform platform, string documentsPath, string fierstName, string lastName);
        event EventHandler<MessageEvent> ProfileNameUpdated;

    }

    public class ProfileData : IProfileData
    {



        HttpProfile _httpauth { get; set; }

        /// <summary>
        /// Успешний вход
        /// </summary>
        public event EventHandler<MessageEvent> ProfileUpdated = delegate { };

        public event EventHandler<MessageEvent> ProfileUpdatedError = delegate { };



        public event EventHandler<MessageEvent> ProfileLoaded = delegate { };

        public event EventHandler<MessageEvent> ProfileLoadedError = delegate { };


        public event EventHandler<MessageEvent> ProfileNameUpdated = delegate { };



        public ProfileData(AuthenticationModel.Account acc)
        {
            _httpauth = new HttpProfile("","");
          
        }

      

    public async void LoadProfile(ISQLitePlatform platform, string documentsPath)
        {
            Dictionary<string, string> items = await StorageDataProfile.DownloadProfile(platform, documentsPath);
            if (items.Count != 0)
                ProfileLoaded(this, new MessageEvent("Профиль успешно загружен", new Profile(items)));
            else
                ProfileLoadedError(this, new MessageEvent("Ошибка при загрузке профиля из БД"));
        }


        public async void UpdateProfile(ISQLitePlatform platform, string documentsPath)
        {
            Dictionary<string, string> items = ParseProfile.GetProfile(await _httpauth.HttpGetProfile());
            if (items != null)
            {
                SaveProfile(platform, documentsPath, items);
                ProfileUpdated(this, new MessageEvent("Профиль успешно обновленн", new Profile(items)));
                LoadProfile(platform, documentsPath);
            }
            else
            {
                ProfileUpdatedError(this, new MessageEvent("Ошибка при обновлении профиля", new Profile(items)));
                LoadProfile(platform, documentsPath);
            }
        }


        private void SaveProfile(ISQLitePlatform platform, string documentsPath, Dictionary<string, string> prof)
        {
            StorageDataProfile.SaveProfile(platform, documentsPath, prof);
        }



        public void UpdateNameProfile(ISQLitePlatform platform, string documentsPath, string fierstName, string lastName)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("FierstName", fierstName);
            items.Add("LastName", lastName);
            StorageDataProfile.SaveProfile(platform, documentsPath, items);
            LoadProfile(platform, documentsPath);
        }
    }
}