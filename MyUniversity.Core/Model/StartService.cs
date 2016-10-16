using MyUniversity.Core.AuthenticationModel;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.StartModel
{
    /// <summary>
    /// classn pervonachalnoi zagryzki resursov i nastroiki sistemi
    /// </summary>
    public static class StartService
    {
        //izmenit
        //proverka polzovatela na 1 vhozdenie
        public static async Task<Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>> VerificationAuthorized(ISQLitePlatform platform, string documentsPath)
        {
            SQLiteService st = new SQLiteService(platform, documentsPath);
            List<StorageAccount> items = await st.GetAllItems<StorageAccount>();

            StorageAccount _stp = null;

            _stp = items.Where(i => i.Key == "UserName").FirstOrDefault();

            StorageAccount _stp1 = null;
            _stp1 = items.Where(i => i.Key == "Password").FirstOrDefault();

            StorageAccount _stp2 = null;
            _stp2 = items.Where(i => i.Key == "__RequestVerificationToken").FirstOrDefault();

            StorageAccount _stp3 = null;
            _stp3 = items.Where(i => i.Key == ".ASPXAUTH").FirstOrDefault();


            if (_stp != null && _stp1 != null && _stp2 != null && _stp3 != null)
                return new Tuple<StorageAccount, StorageAccount, StorageAccount, StorageAccount>(_stp, _stp1, _stp2, _stp3);
            else
                return null;
        }


        public static async Task LogOUT(ISQLitePlatform platform, string documentsPath)
        {
            SQLiteService st = new SQLiteService(platform, documentsPath);
            // ydalit vse table
        }
    }
}
