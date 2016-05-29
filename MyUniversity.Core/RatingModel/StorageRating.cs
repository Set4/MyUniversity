using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.RatingModel
{
    public class StorageRating
    {
        public static async Task<List<Lesson>> DownloadLessons(SQLite.Net.Interop.ISQLitePlatform platform, string documentsPath)
        {
            SQLiteService sql = new SQLiteService(platform, documentsPath);
            List<Lesson> lesson = await sql.GetAllItems<Lesson>();
            return lesson;
        }


        public static async void SaveLessons(SQLite.Net.Interop.ISQLitePlatform platform, string documentsPath, List<Lesson> items)
        {
            SQLiteService sql = new SQLiteService(platform, documentsPath);
            foreach (Lesson item in items)
                await sql.InsertOrReplaceItem<Lesson>(item);
        }
    }
}
