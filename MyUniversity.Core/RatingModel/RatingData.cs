using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.RatingModel
{

    /// <summary>
    /// класс для сохранения данных в бд
    /// </summary>
   
    public class RatingData
    {
        HttpRating Http { get; set; }

        /// <summary>
        /// Успешний вход
        /// </summary>
        public event EventHandler<MessageEvent> UpdatedRating = delegate { };


        public RatingData(AuthenticationModel.Account acc)
        {
            Http = new HttpRating(acc);
        }

        public async Task<List<Lesson>> LoadLessons(ISQLitePlatform platform, string documentsPath)
        {
            return await StorageRating.DownloadLessons(platform, documentsPath);
        }

    

        public async Task<List<Lesson>> GetHttpLessons(ISQLitePlatform platform, string documentsPath)
        {
            return ParseRating.GetRating(await Http.HttpGetRating());
        }

        public async void Update(ISQLitePlatform platform, string documentsPath)
        {
           // StorageRating.SaveLessons(platform, documentsPath, ));

            UpdatedRating(this, new MessageEvent("Рейтинг обновлен",await GetHttpLessons(platform, documentsPath )));
        }


    }
}
