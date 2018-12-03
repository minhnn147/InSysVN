using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;


namespace WebApplication.Code
{
    public static class AppSettings
    {

        public static string FolderTemplate
        {
            get { return Get("Template", "/Templates/"); }
        }

        public static string FolderUpload
        {
            get
            {
                return Get("FolderUpload", "/FolderUpload/");
            }
        }

        public static string GmailFrom
        {
            get { return Get("GmailFrom", "user@gmail.com"); }
        }
        public static string GmailFromPass
        {
            get { return Get("GmailFromPass", "password"); }
        }

        public static string CaptchaSecretKey
        {
            get { return Get("CaptchaSecretKey", "123456789"); }
        }
        public static string CaptchaSiteKey
        {
            get { return Get("CaptchaSiteKey", "123456789"); }
        }

        public static string PasswordHash
        {
            get
            {
                return Get("PasswordHash", "pYqzM0oHqUGSX5tOqzin");
            }
        }
        private static T Get<T>(string key, T defaultValue = default(T))
        {
            try
            {
                var value = WebConfigurationManager.AppSettings[key];
                if (value != null)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch
            {
                // ignored
            }
            return defaultValue;
        }
    }
}