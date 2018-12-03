using System;
using System.Web;
using log4net;

namespace LibCore.Helper.Logging
{
    public static class Logging
    {
        //private static readonly ILog Log = LogManager.GetLogger("log4netLogger");
        private static readonly ILog Log = LogManager.GetLogger("ForAllApplication");

        public static void PutError(string message, Exception e)
        {
            Log.Error(message + "; Url: " + HttpContext.Current.Request.Url.AbsoluteUri + "; Error: ", e);

            var logMailFlg = Configuration.Config.GetConfigByKey("SystemMailLog");
            if (logMailFlg != null)
            {
                var sendMailFlg = Convert.ToBoolean(logMailFlg);
                var sysName = Configuration.Config.GetConfigByKey("SystemName") ?? "";
                if (sendMailFlg)
                    LogManager.GetLogger("ECSLogger").Error(sysName + " : " + message, e);
            }
        }
        public static void PushString(string message)
        {
            Log.Error(message);
        }
        // Other Custom Logging Functions

    }
}
