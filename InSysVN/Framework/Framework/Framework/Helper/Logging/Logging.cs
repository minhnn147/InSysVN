using System;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using log4net;

namespace Framework.Helper.Logging
{
    public static class Logging
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Error(string strError)
        {
            var UrlController = HttpContext.Current.Request.Url;
            var UserName = HttpContext.Current.User.Identity.Name;
            log.Error(UserName + " | " + UrlController + " | " + strError);
            //Send Mail????
        }
        public static void Info(string strInfo)
        {
            var UrlController = HttpContext.Current.Request.Url;
            var UserName = HttpContext.Current.User.Identity.Name;
            log.Info(UserName + " | " + UrlController + " | " + strInfo);
            //Send Mail????
        }
        public static void Warning(string strWarning)
        {
            var UrlController = HttpContext.Current.Request.Url;
            var UserName = HttpContext.Current.User.Identity.Name;
            log.Warn(UserName + " | " + UrlController + " | " + strWarning);
            //Send Mail????
        }
        public static void Debug(string strDebug)
        {
            var UrlController = HttpContext.Current.Request.Url;
            var UserName = HttpContext.Current.User.Identity.Name;
            log.Debug(UserName + " | " + UrlController + " | " + strDebug);
            //Send Mail????
        }
        public static void Fatal(string strFatal)
        {
            var UrlController = HttpContext.Current.Request.Url;
            var UserName = HttpContext.Current.User.Identity.Name;
            log.Fatal(UserName + " | " + UrlController + " | " + strFatal);
            //Send Mail????
        }
        
    }
}
