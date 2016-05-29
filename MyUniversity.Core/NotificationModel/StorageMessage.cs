using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.NotificationModel
{
   public static class StorageMessage
    {
        public static async Task<List<Notification>> DownloadMessage(SQLite.Net.Interop.ISQLitePlatform platform, string documentsPath)
        {

            SQLiteService sql = new SQLiteService(platform, documentsPath);
        
           List<Notification> Message =await sql.GetAllItems<Notification>();
            return Message;
        }


        public static async void SaveMessage(SQLite.Net.Interop.ISQLitePlatform platform, string documentsPath, List<Notification> items)
        {
            SQLiteService sql = new SQLiteService(platform, documentsPath);
            foreach (Notification item in items)
                await sql.InsertOrReplaceItem<Notification>(item);
        }
    }
}
