using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDictionaryGenerator
{
    /// <summary>
    /// 
    /// </summary>
    public class ReturnMessage
    {
        public bool isSuccess { get; set; }
        public string Messages { get; set; }

        public ReturnMessage(string Msg, bool isSucc)
        {
             isSuccess = isSucc;
             Messages = Msg;
        }
    }
}
