using Framework.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Framework.EF
{
    public class SingletonPerRequest
    {
        /// <summary>
        /// Gets the object per request.
        /// </summary>
        /// <value>
        /// The object per request.
        /// </value>
        public static Hashtable ObjectPerRequest
        {
            get
            {
                return (HttpContext.Current.Items[Config.GetConfigByKey("ObjectPerRequest")] ??
                    (HttpContext.Current.Items[Config.GetConfigByKey("ObjectPerRequest")] =
                    new Hashtable())) as Hashtable;

            }
        }
    }
}
