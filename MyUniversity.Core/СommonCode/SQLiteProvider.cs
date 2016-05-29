using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SQLite.Net.Async;
using SQLite.Net;

using System.Diagnostics;

using SQLite.Net.Interop;

namespace MyUniversity.Core
{
    public interface stringKey
    {
        string Key { get; set; }
    }

  
    public class SQLiteService
    {
        private SQLiteAsyncConnection _dbConnection;

  

        public SQLiteService(ISQLitePlatform platform, string documentsPath)
        {

            string path = System.IO.Path.Combine(documentsPath, "myuni.db3");

            var connectionFactory = new Func<SQLiteConnectionWithLock>
                    (() => new SQLiteConnectionWithLock(platform,  new SQLiteConnectionString(path, true)));

           _dbConnection = new SQLiteAsyncConnection(connectionFactory);
        }


      private async Task<int> Table<T>() where T: class
        {
            int i = await _dbConnection.ExecuteScalarAsync<int>(String.Format("SELECT count(*) FROM myuni.db3 WHERE type = 'table' AND name = '{0}'", typeof(T)), null).ContinueWith((t) => t.Result);

            return i;
        }



        public async Task CreateTable<T>() where T : class
        {
            try
            {
                if (await _dbConnection.ExecuteScalarAsync<int>(String.Format("SELECT count(name) FROM sqlite_master where name='{0}' ", typeof(T).Name)) == 0)
                    await _dbConnection.CreateTableAsync<T>().ContinueWith((results) =>
                    {
                        Debug.WriteLine("Table {0} created", typeof(T));
                    });
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }



        public async Task<int> InsertOrReplaceItem<T>(T item) where T : class
        {
            await CreateTable<T>();
            var result = await _dbConnection.InsertOrReplaceAsync(item);
            return result;
        }

        public async Task<int> UpdateItem<T>(T item) where T : class
        {
            await CreateTable<T>();
            var result = await _dbConnection.UpdateAsync(item);
            return result;
        }

        public async Task<int> DeleteItem<T>(T item) where T : class
        {
            await CreateTable<T>();
            var result = await _dbConnection.DeleteAsync(item);
            return result;
        }

        public async Task<List<T>> GetAllItems<T>() where T : class
        {
            await CreateTable<T>();
            List<T> result = await _dbConnection.Table<T>().ToListAsync();
            return result;
        }

        public async Task<T> GetItemsById<T>(string Key) where T : class, stringKey
        {
            await CreateTable<T>();
            var result = await _dbConnection.Table<T>().Where(i => i.Key == Key).FirstOrDefaultAsync();
            return result;
        }



    }


 

}
