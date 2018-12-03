using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ADOTableAttribute : System.Attribute
    {
        public readonly string Table;
        public ADOTableAttribute(string table)  // url is a positional parameter
        {
            this.Table = table;
        }
    }
}
