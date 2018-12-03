using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Helpers
{
   public static class Logging
    {
        private static readonly ILog Log = LogManager.GetLogger("ForAllApplication");

        public static void PutError(string message, Exception e)
        {
            Log.Error(message + "; Url: " + HttpContext.Current.Request.Url.AbsoluteUri + "; Error: ", e);
        }
        public static void PushString(string message)
        {
            Log.Error(message);
        }
        public static void PushInfo(string message)
        {
            Log.Info(message);
        }
    }
}
