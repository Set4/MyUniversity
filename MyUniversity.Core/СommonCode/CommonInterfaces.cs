using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.СommonCode
{
    public interface IStorageServise
    {
        /// <summary>
        /// сохраение загруженного изображения
        /// </summary>
        /// <param name="image">массив byte хранящий изовражение</param>
        void LoadImage(byte[] image);
    }
}
