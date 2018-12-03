using System;
using System.Configuration;

namespace Framework.Configuration
{
    /// <summary>
    /// Get config from .config file dynamic key
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets the config by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetConfigByKey(string key)
        {
            try
            {
                var sconfig = ConfigurationManager.AppSettings[key];
                return sconfig;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T GetConfigValue<T>(string key)
        {
            try
            {
                string rawValue = ConfigurationManager.AppSettings[key];
                return (T)Convert.ChangeType(rawValue, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
