using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class ExtendMethod
    {
        public static T2 GetOrDefault<T, T1, T2>(this Dictionary<T, T1> dic, T key, T2 _default)
        {
            var temp = default(T1);
            T2 temp1 = default(T2);
            if (dic.TryGetValue(key, out temp))
            {
                return (T2)Convert.ChangeType(temp, _default.GetType());
            }
            else
            {
                return _default;
            }
        }
    }
    public static class Extensions
    {
        #region Parse
        /// <summary>
        /// Gets the prop value.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="propName">Name of the prop.</param>
        /// <returns></returns>
        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        /// <summary>
        /// Sets the prop value.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="valueSet">The value set.</param>
        public static void SetPropValue(this object src, string propName, object valueSet)
        {
            var obj = src.GetType().GetProperty(propName);
            if (obj != null) obj.SetValue(src, valueSet, null);
        }

        /// <summary>
        /// Determines whether the specified obj has property.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj has property; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Gets the prop value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        /// <summary>
        /// Casts the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static T? Cast<T>(this object obj) where T : struct
        {
            return obj as T?;
        }
        /// <summary>
        /// Gets the primary key object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKeyObject(this Object obj)
        {
            var property = obj.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            return property;
        }

        /// <summary>
        /// Gets the type of the primary key from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKeyFromType(Type type)
        {
            var property = type.GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            return property;
        }       
        #endregion
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
    public static class NumericTypeExtension
    {
        public static bool IsNumeric(Type dataType)
        {
            return (dataType == typeof(int)
                    || dataType == typeof(double)
                    || dataType == typeof(long)
                    || dataType == typeof(short)
                    || dataType == typeof(float)
                    || dataType == typeof(Int16)
                    || dataType == typeof(Int32)
                    || dataType == typeof(Int64)
                    || dataType == typeof(uint)
                    || dataType == typeof(UInt16)
                    || dataType == typeof(UInt32)
                    || dataType == typeof(UInt64)
                    || dataType == typeof(sbyte)
                    || dataType == typeof(Single)
                    || dataType == typeof(Decimal)
                   );
        }
    }
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
