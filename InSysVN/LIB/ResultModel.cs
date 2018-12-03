using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LIB
{
    public class ResultModel
    {
    }
    public class ResultMessageModel
    {
        public string message { get; set; }
        public int type { get; set; }
        /// <summary>
        /// //type: 0 = Info;
        /// 1 = Success;
        /// 2 = Warning;
        /// 3 = Error
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        public ResultMessageModel(string Message, int Type)
        {
            message = Message;
            type = Type;
        }
    }
}