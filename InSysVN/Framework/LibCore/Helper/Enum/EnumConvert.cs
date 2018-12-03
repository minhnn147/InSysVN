using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPro.Core.Helper.Enum
{
    public class EnumConvert
    {
        public static T Parse<T>(int value)
        {
            return (T)System.Enum.ToObject(typeof(T), value);
        }

        public static T Parse<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }
    }
}
