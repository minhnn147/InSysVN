using Framework.EF;
using Framework.Helper.Cache;
using Framework.Caching;
using System.Collections.Generic;
using Framework.Data;
using System.Data;
using System;
using System.Linq;

namespace Framework
{
    public class BaseIpl<T>
    {
        public T unitOfWork;
        protected ICacheProvider cache;
        protected CacheHelper cacheHelper;
        protected string _schema;
        protected string _connection;
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseIpl{T}" /> class.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public BaseIpl(string schema = "dbo")
        {
            _schema = schema;
            cache = new MemcachedProvider(schema);
            cacheHelper = new CacheHelper(schema);
            if (!string.IsNullOrEmpty(_connection))
                unitOfWork = (T)SingletonIpl.GetInstance<T>(schema, _connection);
            else
                unitOfWork = (T)SingletonIpl.GetInstance<T>(schema);
            //unitOfWork.CacheHelper = cacheHelper;
        }
    }
}
