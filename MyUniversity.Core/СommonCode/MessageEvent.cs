using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUniversity.Core.Сommon_Code
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
   public class MessageEvent:EventArgs
    {
        public string Message { get; private set; }
        public object Item { get; private set; }
        public MessageEvent(string message, object item = null)
        {
            Message = message;
            Item = item;
        }
    }
}
