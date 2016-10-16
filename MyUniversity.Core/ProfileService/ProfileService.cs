using MyUniversity.Core.Сommon_Code;
using MyUniversity.Core.СommonCode;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyUniversity.Core.ProfileModel
{
    public class StydentProfile
    {

        #region Свойства и поля

        private string fio = null;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get
            {
                if(fio!=null)
                {
                string[] s = fio.Split(' ');
                if (s.Length > 0)
                    return s[0];
               
                else
                    return String.Empty;
                }
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FierstName
        {
            get
            {
                if (fio != null)
                {
                    string[] s = fio.Split(' ');
                    if (s.Length > 1)
                        return s[1];

                    else
                        return String.Empty;
                }
                else
                    return String.Empty;
            }
        
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic
        {
            get
            {
                if (fio != null)
                {
                    string[] s = fio.Split(' ');
                    if (s.Length > 2)
                        return s[2];

                    else
                        return String.Empty;
                }
                else
                    return String.Empty;
            }
        }

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

            _stp = items.Where(i => i.Key == "FIO").FirstOrDefault();
            if (_stp != null)
                fio = _stp.Value;
            else
                groupNumber = "Данные отсутствуют";

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
                if (imageSource != String.Empty)
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



    class ProfileService
    {
        ProfileHttp _profilehttp;
        ProfileParse _profileparse;
        ProfileStorage _profilestorage;

        IStorageServise _storageserv;


        public ProfileService(HttpProvider http, ISQLitePlatform platform, string documentsPath, IStorageServise storageserv)
        {
            _profilehttp = new ProfileHttp(http);
            _profileparse = new ProfileParse(1);
            _profilestorage = new ProfileStorage(platform, documentsPath);

            _storageserv = storageserv;
        }



        public async Task<List<StorageProfile>> GetProfile()
        {
            List<StorageProfile> items = await _profilestorage.Load();

            List<StorageProfile> newitems = await _profileparse.ParseProfile(await _profilehttp.HttpGetProfile());

            if (newitems != null)
                foreach (StorageProfile item in newitems)
                    if (!items.Contains(item))
                    {
                        items.Add(item);
                      await  _profilestorage.Save(item);

                        if (item.Key == "ImageSource")
                            UpdatePhoto(await _profilehttp.HttpDownloadImage(item.Value));
                    }

            return items;
        }



        private void UpdatePhoto(byte[] image)
        {
            _storageserv.LoadImage(image);
        }


        public void LoadProfile()
        {

        }


        private void StorageUpdateStydentProfile()
        { }

    }

    class ProfileHttp
    {
        HttpProvider _httpprovider;

        public ProfileHttp(HttpProvider httpprovider)
        {
            _httpprovider = httpprovider;
        }

        public async Task<string> HttpGetProfile()
        {
            using (HttpResponseMessage response = await _httpprovider.HttpMethodGet("/Student/DekanatInfo"))
            {
                if (response != null)
                    return await response.Content.ReadAsStringAsync();
                else
                    return String.Empty;
            }
        }

        public async Task<byte[]> HttpDownloadImage(string uri)
        {
            using (HttpResponseMessage response = await _httpprovider.HttpMethodGet(uri))
            {
                if (response.Content.Headers.ContentType.MediaType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                    return await response.Content.ReadAsByteArrayAsync();
                else
                    return null;
            }
        }

       
    }
    class ProfileParse
    {
        /// <summary>
        /// Время ожидания операции поиска Токенов
        /// </summary>
        double _timewait;

        public ProfileParse(double timewait)
        {
            _timewait = timewait;
        }

        public async Task<List<StorageProfile>> ParseProfile(string response)
        {

            List<StorageProfile> profilelist = null;

            try
            {
                if (String.IsNullOrEmpty(response))
                    return profilelist;

                profilelist =await Task.Run(() =>
                {

                    List<StorageProfile> items = new List<StorageProfile>();

                    Regex regex = new Regex("<ul.*?class=[\"]*list-group[\"]*.*?>+(.*?|\\s)+<\\/ul>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match pfofil = regex.Match(response);


                    Regex regexfio = new Regex("<li.*?class=[\"]*user-name[\"]*.*?>+(.*?|\\s)+<\\/li>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match fio = regexfio.Match(response);

                    regexfio = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    fio = regexfio.Match(fio.Value);
                    items.Add(new StorageProfile() { Key = "FIO", Value = fio.Value.Substring(1, fio.Value.Length - 2) });

                    regex = new Regex("<strong>.*?<\\/li>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection info = regex.Matches(pfofil.Value);


                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    MatchCollection k = regex.Matches(info[0].Value);
                    items.Add(new StorageProfile() { Key = "GroupNumber", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });

                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    k = regex.Matches(info[1].Value);
                    items.Add(new StorageProfile() { Key = "Department", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });

                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    k = regex.Matches(info[2].Value);
                    items.Add(new StorageProfile() { Key = "Chair", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });

                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    k = regex.Matches(info[3].Value);
                    items.Add(new StorageProfile() { Key = "Specialty", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });

                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    k = regex.Matches(info[4].Value);
                    items.Add(new StorageProfile() { Key = "TrainingProfile", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });

                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    k = regex.Matches(info[5].Value);
                    items.Add(new StorageProfile() { Key = "ModeofStudy", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });

                    regex = new Regex(">.*?<", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    k = regex.Matches(info[6].Value);
                    items.Add(new StorageProfile() { Key = "Email", Value = k[1].Value.Substring(1, k[1].Value.Length - 2) });


                    Regex reg = new Regex("<img.*?alt=[\"]*Фотография[\"]*.*?>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    Match imagesourse = reg.Match(response);

                    reg = new Regex("\\/Image\\/UserPhoto\\/[^\\s]*\"", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(_timewait));
                    imagesourse = reg.Match(response);
                    items.Add(new StorageProfile() { Key = "ImageSource", Value = imagesourse.Value.Substring(0, imagesourse.Value.Length - 1) });

                    return items;

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse Profile Error: {0}", ex.Message);
            }

            return profilelist;
        }
    }

    class ProfileStorage
    {
        private SQLiteService st;

        public ProfileStorage(ISQLitePlatform platform, string documentsPath)
        {
            st = new SQLiteService(platform, documentsPath);
        }


        public async Task Save(StorageProfile item)
        {
            await st.InsertOrReplaceItem<StorageProfile>(item);
        }
        public Task<List<StorageProfile>> Load()
        {
            return st.GetAllItems<StorageProfile>();
        }
        public void Saerch()
        {

        }
    }
}
