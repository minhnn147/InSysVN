using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper.ExtendedMethods
{
    public static class ExtendedMethods
    {
        private static DateTime _jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0);

        #region Date Time
        public static long ToLong(this DateTime from)
        {
            return System.Convert.ToInt64((from - _jan1st1970).TotalMilliseconds);
        }
        public static string ToString(this DateTime from, string format = "dd/MM/YYYY")
        {
            return from.ToString(format);
        }
        #endregion

        #region Dictionary
        public static T1 GetOrDefault<T, T1>(this Dictionary<T, T1> dic, T key, T1 _default)
        {
            T1 temp = default(T1);
            if (dic.TryGetValue(key, out temp))
            {
                return temp;
            }
            else
            {
                return _default;
            }
        }
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }
        public static void Sort<T, T1>(this Dictionary<T, T1> dic, bool asc = true, bool keyOrValue = true)
        {
            var temp = asc ? dic.OrderBy(e => e.Key) : dic.OrderByDescending(e => e.Key);
            dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        }
        #endregion

        #region int
        public static int ToInt(this int? value)
        {
            return value ?? 0;
        }
        #endregion

        #region long
        public static DateTime ToDateTime(this long from)
        {
            return _jan1st1970.AddMilliseconds(from);
        }
        public static int ToInt(this long value)
        {
            try
            {
                return (int)value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region string
        public static int? ToInt(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DateTime? ConvertToDate(this string value, string format = "MM/dd/yyyy")
        {
            try
            {
                DateTime _return;
                double tempValue1;
                if (DateTime.TryParse(value, out _return))
                {

                }
                else if (double.TryParse(value, out tempValue1))
                {
                    _return = DateTime.FromOADate(tempValue1);
                }
                else
                {
                    _return = DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
                }
                return _return;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region enum
        public static int? ToInt(this Enum _enum)
        {
            try
            {
                return (int)((object)_enum);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string GetDescription(this Enum value)
        {
            try
            {

                // Get the type
                Type type = value.GetType();

                // Get fieldinfo for this type
                FieldInfo fieldInfo = type.GetField(value.ToString());

                //// Get the stringvalue attributes
                DescriptionAttribute[] attribs = fieldInfo.GetCustomAttributes(
                    typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                //// Return the first if there was a match.
                return attribs.Length > 0 ? attribs[0].Description : value.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetDescription<T>(this Enum _enum, T valueType)
        {
            try
            {
                Enum value = valueType as Enum;
                return value.GetDescription();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static Dictionary<int, string> EnumToDictionary(Type type)
        {
            //Type type = _enum.GetType();
            var _return = new Dictionary<int, string>();
            try
            {
                foreach (object item in Enum.GetValues(type))
                {
                    _return.Add(((int)item), GetDescription((Enum)item));
                }

            }
            catch (Exception ex)
            {

            }
            return _return;

        }
        #endregion

        #region object
        public static List<Object> GenericWithParent(this List<Object> _this, List<Object> parent)
        {
            for (int i = 0; i < parent.Count; i++)
            {
                _this[i] = _this[i].GenericWithParent(parent[i]);
            }
            return _this;
        }
        public static Object GenericWithParent(this Object _this, Object parent)
        {
            var properties_this = _this.GetType().GetProperties().ToList();
            var properties_parent = parent.GetType().GetProperties().ToList();
            foreach (var pro in properties_this)
            {
                var proParent = properties_parent.Find(e => e.Name.Equals(pro.Name));
                if (proParent == null)
                {
                    continue;
                }
                if (!pro.CanWrite)
                {
                    continue;
                }
                pro.SetValue(_this, proParent.GetValue(parent, null));
            }
            return _this;
        }
        public static List<Object> GenericWithChild(this IEnumerable<Object> _this_, IEnumerable<Object> child_)
        {
            var _this = _this_.ToList();
            var child = child_.ToList();
            for (int i = 0; i < child.Count; i++)
            {
                _this[i] = _this[i].GenericWithChild(child[i]);
            }
            return _this;
        }
        public static Object GenericWithChild<T>(this Object _this, T child)
        {
            var properties_this = _this.GetType().GetProperties().ToList();
            var properties_child = child.GetType().GetProperties().ToList();
            foreach (var pro in properties_this)
            {
                var proChild = properties_child.Find(e => e.Name.Equals(pro.Name));
                if (proChild == null)
                {
                    continue;
                }
                if (!pro.CanWrite)
                {
                    continue;
                }
                pro.SetValue(_this, proChild.GetValue(child, null));
            }
            return _this;
        }

        public static Object Clone<T>(this Object parent) where T : new()
        {

            var temp = new T();
            temp = (T)temp.GenericWithParent(parent);
            return temp;
        }

        public static T ConvertIDictionaryToObject<T>(this IDictionary<string, object> _this) where T : new()
        {
            T temp = new T();
            foreach (var item in temp.GetType().GetProperties())
            {
                if (item.CanWrite && _this[item.Name] != null)
                {
                    item.SetValue(temp, _this[item.Name]);
                }
            }
            return temp;
        }
        #endregion
    }
}
