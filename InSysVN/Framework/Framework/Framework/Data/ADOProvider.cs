using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Nest;
using StackExchange.Profiling;
using Framework.Configuration;
using Framework.Helper.Attributes;
using Framework.Helper.Cache;
using Framework.Helper.Extensions;
using Framework.Security.Crypt;
using Framework.Caching;
using Dapper;
using DapperExtensions;
using log4net;

namespace Framework.Data
{
    /// <summary>
    /// help to gets connection string in .config file
    /// </summary>
    public class ADOProvider : IDisposable
    {
        private bool _disposed = false;
        //public DataProviderBaseClass Provider;
        private IDbTransaction _transaction;
        private string _schema;
        private CacheHelper _cacheHelper;
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IDbConnection Connection
        {
            get
            {
                if (ConnectionString != null)
                    return new SqlConnection(ConnectionString);

                return new SqlConnection(DataHelper.GetConnectionString());
            }
        }

        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public CacheHelper CacheHelper
        {
            get { return new CacheHelper(_schema); }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        //public ICacheProvider Cache
        public MemcachedProvider Cache
        {
            get { return new MemcachedProvider(_schema); }
        }

        /// <summary>
        /// Tenants the internal.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public ADOProvider(string schema)
        {
            _schema = schema;
        }
        public ADOProvider(string schema, string connection)
        {
            _schema = schema;
            ConnectionString = connection;
        }



        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Could not find the key attributes of the entity or table name Entity;id</exception>
        public virtual T FindById<T>(int id) where T : class
        {
            T item;
            using (var cn = Connection)
            {
                cn.Open();
                item = cn.Get<T>(id);
                cn.Close();
            }
            return item;
        }
        public virtual T FindById<T>(long id) where T : class
        {
            T item;
            using (var cn = Connection)
            {
                cn.Open();
                item = cn.Get<T>(id);
                cn.Close();
            }
            return item;
        }

        public virtual T Insert<T>(T item) where T : class
        {
            //http://www.contentedcoder.com/2012/12/creating-data-repository-using-dapper.html
            using (var cn = Connection)
            {
                cn.Open();
                var id = cn.Insert(item);
                cn.Close();
            }
            return item;
        }

        public virtual bool Update<T>(T item) where T : class
        {
            var data = false;
            using (var cn = Connection)
            {
                cn.Open();
                data = cn.Update<T>(item);
                cn.Close();
            }
            return data;
        }

        public virtual bool Delete<T>(T item) where T : class
        {
            var data = false;
            using (var cn = Connection)
            {
                cn.Open();
                data = cn.Delete<T>(item);
                cn.Close();
            }
            return data;
        }

        public virtual List<T> GetAll<T>(object predicate = null) where T : class
        {
            List<T> data = new List<T>();
            using (var cn = Connection)
            {
                cn.Open();
                var list = cn.GetList<T>(predicate);
                if (list.Any())
                {
                    data = list.ToList();
                }
                cn.Close();
            }
            return data;
        }


        public virtual List<T> GetPage<T>(object predicate = null, IList<ISort> sort = null, int page = 0, int resultsPerPage = 10) where T : class
        {
            //https://github.com/tmsmith/Dapper-Extensions/blob/master/DapperExtensions.Test/IntegrationTests/MySql/CrudFixture.cs
            List<T> data = new List<T>();
            using (var cn = Connection)
            {
                cn.Open();
                if (sort == null) sort = GetSortDefault<T>();
                var list = cn.GetPage<T>(predicate, sort, page, resultsPerPage);
                if (list.Any())
                {
                    data = list.ToList();
                }
                cn.Close();
            }
            return data;
        }

        public virtual int Count<T>(object predicate = null) where T : class
        {
            var data = 0;
            using (var cn = Connection)
            {
                cn.Open();
                data = cn.Count<T>(predicate);
                cn.Close();
            }
            return data;
        }

        private IList<ISort> GetSortDefault<T>()
        {
            var propertyEntity = ObjectExtensions.GetPrimaryKeyFromType(typeof(T));
            if (propertyEntity != null)
            {
                return new List<ISort>{
                    new Sort
                    {
                        PropertyName = propertyEntity.Name,
                        Ascending = true
                    }
                };
            }
            throw new ArgumentException("Could not find the key attributes of the entity or table name Entity", "id");
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query<T>(string sql, object p = null)
        {
            IEnumerable<T> item;
            using (var cn = Connection)
            {
                cn.Open();
                item = p != null ? cn.Query<T>(sql, p) : cn.Query<T>(sql);
                cn.Close();
            }
            return item;
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> Query(string sql, object p = null)
        {
            IEnumerable<dynamic> item;
            using (var cn = Connection)
            {
                cn.Open();
                item = p != null ? cn.Query(sql, p) : cn.Query(sql);
                cn.Close();
            }
            return item;
        }

        public virtual int Execute(string sql, object p = null)
        {
            int item;
            using (var cn = Connection)
            {
                cn.Open();
                item = p != null ? cn.Execute(sql, p) : cn.Execute(sql);
                cn.Close();
            }
            return item;
        }

        /// <summary>
        /// Procedures the specified procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <param name="oParams">The o params.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public IEnumerable<T> Procedure<T>(string procedure, object oParams = null, bool useCache = false)
        {
            //using multi tenant.
            //if (oParams == null) oParams = new { schema = _schema };
            using (MiniProfiler.Current.Step(procedure))
            {
                using (Connection)
                {
                    var listParams = new DynamicParameters();
                    //Dictionary<string, DynamicParameters.ParamInfo> parameters = null;

                    var key = procedure + CacheHelper.GenerateSuffixKeyByParams(oParams);
                    var hashKey = Md5Util.Md5EnCrypt(key);
                    IEnumerable<T> items = null;

                    if (useCache && CacheHelper.UseCache)
                        items = Cache.Get<IEnumerable<T>>(hashKey);

                    //if (useCache && CacheHelper.UseCache)
                    //{
                    //    #region Set data ouput
                    //    var obj = oParams.GetType();
                    //    if (obj.FullName == "Dapper.DynamicParameters")
                    //    {
                    //        listParams = oParams as DynamicParameters;
                    //        if (listParams != null)
                    //        {
                    //            listParams.Cache = Cache.GetInstance();
                    //            listParams.CommandCurrent = procedure;
                    //            listParams.KeyOutParams = key;
                    //            parameters = listParams.parameters;
                    //        }
                    //    }
                    //    #endregion
                    //}

                    if (items == null || items.Count() == 0)
                    {
                        items = Connection.Query<T>(procedure, oParams, commandType: CommandType.StoredProcedure);

                        if (useCache && CacheHelper.UseCache)
                        {
                            //#region Set data ouput

                            //if (parameters != null && parameters.Count > 0)
                            //{
                            //    foreach (var parameter in parameters)
                            //    {
                            //        var oParamInfo = parameter.Value;
                            //        if (oParamInfo.ParameterDirection != ParameterDirection.Output) continue;
                            //        var typeParamOutput = DataHelper.ConvertType(oParamInfo.DbType);
                            //        var nameOutput = parameter.Value.Name;
                            //        var keyOuput = CacheHelper.GenerateKeyCacheOutput(key, nameOutput);
                            //        listParams.KeyOutParams = key;

                            //        var hashKeyOuput = Md5Util.Md5EnCrypt(keyOuput);
                            //        object valueOutput;
                            //        switch (typeParamOutput.Name)
                            //        {
                            //            case "Int32":
                            //                valueOutput = listParams.GetDataOutput<int>(nameOutput);
                            //                if (useCache && CacheHelper.UseCache)
                            //                    Cache.Set(hashKeyOuput, valueOutput);
                            //                break;
                            //            case "String":
                            //                valueOutput = listParams.GetDataOutput<string>(nameOutput);
                            //                if (useCache && CacheHelper.UseCache)
                            //                    Cache.Set(hashKeyOuput, valueOutput);
                            //                break;
                            //        }
                            //    }
                            //}
                            //#endregion
                            if (useCache && CacheHelper.UseCache)
                                Cache.Set(hashKey, items);
                        }
                    }
                    return items;
                }
            }
        }

        /// <summary>
        /// Procedures the query multi.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="oParams">The o params.</param>
        /// <returns></returns>
        public SqlMapper.GridReader ProcedureQueryMulti(string procedure, object oParams = null, bool useCache = true)
        {
            //if (oParams == null) oParams = new { schema = _schema };
            using (MiniProfiler.Current.Step(procedure))
            {
                //var key = procedure + CacheHelper.GenerateSuffixKeyByParams(oParams);
                using (Connection)
                {
                    //SqlMapper.GridReader items = Cache.Get<SqlMapper.GridReader>(key);
                    var items = Connection.QueryMultiple(procedure, oParams, commandType: CommandType.StoredProcedure);
                    //Cache.Set(key, items);
                    return items;
                }
            }
        }

        /// <summary>
        /// Procedures the execute.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="oParams">The o params.</param>
        /// <returns></returns>
        public bool ProcedureExecute(string procedure, object oParams = null)
        {
            try
            {
                using (MiniProfiler.Current.Step(procedure))
                {
                    using (Connection)
                    {
                        Connection.Execute(procedure, oParams, commandType: CommandType.StoredProcedure);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public bool ProcedureExecute(string procedure, int commandTimeout, object oParams = null)
        {
            try
            {
                using (MiniProfiler.Current.Step(procedure))
                {
                    using (Connection)
                    {
                        Connection.Execute(procedure, oParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// Inserts the data using SQL bulk copy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="schema">The schema.</param>
        public void InsertDataUsingSqlBulkCopy<T>(IEnumerable<T> data, string schema = "")
        {
            try
            {
                var propertyTableEntity = GetTableFromType(typeof(T));
                if (propertyTableEntity != null)
                {
                    var schemaAction = _schema;
                    if (!string.IsNullOrEmpty(schema))
                    {
                        schemaAction = schema;
                    }
                    //var table = string.Format("{0}.{1}", _schema, propertyTableEntity.Table);
                    var table = string.Format("{0}.{1}", schemaAction, propertyTableEntity.Table);
                    using (IDbConnection cn = Connection)
                    {
                        var conn = Connection as SqlConnection;
                        if (conn == null) return;
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        using (var bulkCopy = new SqlBulkCopy(conn))
                        {
                            //http://www.sqlteam.com/article/use-sqlbulkcopy-to-quickly-load-data-from-your-client-to-sql-server
                            //DestinationTableName = "Person"
                            bulkCopy.NotifyAfter = 10000;
                            bulkCopy.DestinationTableName = table;
                            bulkCopy.BulkCopyTimeout = 0; // timeout unlimited
                            bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(SqlRowsCopied);
                            var fields = GetAllPropertyField<T>();
                            if (fields.Count > 0)
                            {
                                //var propertyEntity = ObjectExtensions.GetPrimaryKeyFromType(typeof(T));
                                //if (propertyEntity != null)
                                //    fields.Remove(propertyEntity.Name);

                                foreach (KeyValuePair<string, int> pair in fields)
                                {
                                    bulkCopy.ColumnMappings.Add(pair.Key, pair.Key);
                                    //bulkCopy.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                                }
                            }
                            using (var dataReader = new ObjectDataReader<T>(data))
                            {
                                bulkCopy.WriteToServer(dataReader);
                                bulkCopy.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        /// <exception cref="System.SystemException">
        /// </exception>
        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                return;
            }

            try
            {
                _transaction.Rollback();
            }
            catch (SqlException se)
            {
                throw new SystemException(se.Message);
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                Connection.Close();
                _transaction = null;
            }
        }


        #region Function
        /// <summary>
        /// Runs the procedure schema using multi tenant
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public string RunProcedureSchema(string procedure, string schema = null)
        {
            if (string.IsNullOrEmpty(schema))
            {
                if (string.IsNullOrEmpty(_schema))
                    schema = Config.GetConfigByKey("TenantInternal");
                else
                    schema = _schema;
            }
            var nameProcedure = string.Format("[{0}].{1}", schema, procedure);

            return nameProcedure;
        }

        /// <summary>
        /// Gets the type of the table from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The attribute ADOTable was not found;type</exception>
        public ADOTableAttribute GetTableFromType(Type type)
        {
            // Get instance of the attribute.
            var attributeAdo = (ADOTableAttribute)Attribute.GetCustomAttribute(type, typeof(ADOTableAttribute));

            if (attributeAdo == null)
            {
                throw new ArgumentException("The attribute ADOTable was not found", "type");
            }
            return attributeAdo;
        }

        /// <summary>
        /// SQLs the rows copied.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SqlRowsCopiedEventArgs"/> instance containing the event data.</param>
        private void SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Console.WriteLine("-- Copied {0} rows.", e.RowsCopied);
        }

        /// <summary>
        /// Gets all property field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Dictionary<string, int> GetAllPropertyField<T>()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                //.Where(p => !(p.DeclaringType is Base))
                .Where(p => p.CanRead && !p.GetGetMethod().IsVirtual)
                .Where(p =>
                {
                    var attributes = p.GetCustomAttributes(false);
                    return !attributes.Any(a => a is ElasticPropertyAttribute)
                           && !attributes.Any(a => a is KeyAttribute);
                    //&& !attributes.Any(a => a is NotMappedAttribute);
                })
                .Select((p, i) => new
                {
                    Index = i,
                    Property = p
                }).ToDictionary(
                p => p.Property.Name,
                p => p.Index,
                StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ADOProvider"/> class.
        /// </summary>
        ~ADOProvider()
        {
            Dispose(false);
        }

    }
}
