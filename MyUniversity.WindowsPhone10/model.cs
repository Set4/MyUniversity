using MyUniversity.Core.СommonCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage;

namespace MyUniversity.WindowsPhone10
{
   class StorageServise: IStorageServise
    {
        public async void LoadImage(byte[] image)
        {
            try
            {

                StorageFile savefile = await ApplicationData.Current.LocalFolder.CreateFileAsync("imageprof.jpg", CreationCollisionOption.ReplaceExisting);
                using (System.IO.Stream st = await savefile.OpenStreamForWriteAsync())
                {
                    await st.WriteAsync(image, 0, image.Length);
                }

                //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

                //StorageFile file = await storageFolder.CreateFileAsync("imageprof.jpg", CreationCollisionOption.ReplaceExisting);
                //await FileIO.WriteBytesAsync(file, image);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Доступ к файлу не получен:  {0}", ex.Message);
            }
        }

    }

  static  class model
    {
       
        public static async Task<bool> TestConnection()
        {
          
            var tcpClient = new StreamSocket();
            try
            {
               

                   await tcpClient.ConnectAsync(new Windows.Networking.HostName("e.kgeu.ru"), "80", SocketProtectionLevel.PlainSocket);

                if (!String.IsNullOrEmpty(tcpClient.Information.RemoteAddress.DisplayName))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147013895)
                {
                    Debug.WriteLine( "Error: No such host is known");
                }
                else if (ex.HResult == -2147014836)
                {
                    Debug.WriteLine("Error: Timeout when connecting (check hostname and port");
                }
                return false;
            }
            finally
            {
                tcpClient.Dispose();
            }
        }
    }
}
