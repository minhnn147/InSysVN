using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data
{

    /// <summary>
    /// Class DataSettingsHelper
    /// </summary>
    public class DataSettingsHelper
    {
        /// <summary>
        /// The _database is installed
        /// </summary>
        private static bool? _databaseIsInstalled;
        /// <summary>
        /// Databases the is installed.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var manager = new DataSettingsManager();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = settings != null && !String.IsNullOrEmpty(settings.DataConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        /// <summary>
        /// Resets the cache.
        /// </summary>
        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }
    }
}
