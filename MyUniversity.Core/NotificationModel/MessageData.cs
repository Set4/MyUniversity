using MyUniversity.Core.ProfileModel;
using MyUniversity.Core.Сommon_Code;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.NotificationModel
{
   public class MessageData
    {
        HttpMessage Http { get; set; }

        /// <summary>
        /// Успешний 
        /// </summary>
        public event EventHandler<MessageEvent> UpdatedMessage = delegate { };

       

        public MessageData(AuthenticationModel.Account acc)
        {
            Http = new HttpMessage(acc);
        }

        public async Task<List<Notification>> LoadMessage(ISQLitePlatform platform, string documentsPath)
        {
            return await StorageMessage.DownloadMessage(platform, documentsPath);
        }

        public async Task<List<Notification>> GetHttpUnreadMessage(ISQLitePlatform platform, string documentsPath)
        {
            return ParseMessage.GetUnreadMessage(await Http.HttpGetMessage());
        }

        public async Task<List<Notification>> GetHttpReadMessage(ISQLitePlatform platform, string documentsPath)
        {
            return ParseMessage.GetReadMessage(await Http.HttpGetMessage());
        }

        public async void Update(ISQLitePlatform platform, string documentsPath, List<Notification> items)
        {
            StorageMessage.SaveMessage(platform, documentsPath, items);
            List<Notification> items1 = await LoadMessage(platform, documentsPath);
            UpdatedMessage(this, new MessageEvent("Уведомления обновлены",items1 ));
        }


      
    }
}
