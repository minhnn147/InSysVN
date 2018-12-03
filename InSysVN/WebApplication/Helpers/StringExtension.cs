using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WebApplication.Helpers
{
    public static class StringExtension
    {
        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        //https://chodounsky.net/2013/11/27/replace-multiple-strings-effectively/
        public static string ReplaceWithStringBuilder(this string value, Dictionary<string, string> toReplace)
        {
            var result = new StringBuilder(value);
            foreach (var item in toReplace)
            {
                result.Replace(item.Key, item.Value);
            }
            return result.ToString();
        }
    }
}