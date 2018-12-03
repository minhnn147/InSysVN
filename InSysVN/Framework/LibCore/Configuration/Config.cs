using System.Configuration;

namespace LibCore.Configuration
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
            var sconfig = ConfigurationManager.AppSettings[key];
            return sconfig;
        }
    }
}
