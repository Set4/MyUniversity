using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.ProfileModel
{
  
    ///// <summary>
    ///// класс для сохранения данных в бд
    ///// </summary>
    //public class StorageProfile : stringKey
    //{
    //    [PrimaryKey, MaxLength(100)]
    //    public string Key { get; set; }
    //    [MaxLength(200)]
    //    public string Value { get; set; }
    //}

    public static class StorageDataProfile
    {

        public static async void SaveProfile(ISQLitePlatform platform, string documentsPath, Dictionary<string, string> profile)
        {
            SQLiteService sql = new SQLiteService(platform, documentsPath);

            await sql.CreateTable<StorageProfile>();
            foreach (KeyValuePair<string, string> item in profile)
                await sql.InsertOrReplaceItem<StorageProfile>(new StorageProfile() { Key = item.Key, Value = item.Value });
        }
        
      
        public static async Task<Dictionary<string, string>> DownloadProfile(ISQLitePlatform platform, string documentsPath)
        {
            SQLiteService  sql = new SQLiteService(platform, documentsPath);

            Dictionary<string, string> acc = new Dictionary<string, string>();

            
            foreach (StorageProfile stp in await sql.GetAllItems<StorageProfile>())
                acc.Add(stp.Key, stp.Value);
            return acc;
        }

        public static async void DeleteProfile(ISQLitePlatform platform, string documentsPath)
        {
            SQLiteService sql = new SQLiteService(platform, documentsPath);
            foreach (KeyValuePair<string,string> stp in await DownloadProfile(platform, documentsPath))
                await sql.DeleteItem<StorageProfile>(new StorageProfile() { Key = stp.Key, Value = stp.Value });
        }

    }
}
