using System.Collections.Generic;
using Framework.Data;
using System.Data;
using System;
using System.Linq;
using Framework;
using Dapper;
using DapperExtensions;
using Dapper.SQLRaw;
using log4net;

namespace LIB
{
    /// <summary>
    /// Class BaseService.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <seealso cref="Framework.BaseIpl{Framework.Data.ADOProvider}" />
    /// <seealso cref="LIB.IBaseServices{T, TKey}" />
    public class BaseService<T, TKey> : BaseIpl<ADOProvider>, IBaseServices<T, TKey> where T : class
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //#region SQL Raw
        public T Raw_Get(string Id, List<string> Columns = null)
        {
            var cn = unitOfWork.Connection;
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.Get(cn, TableName, PrimaryKey, Id, Columns);
        }
        public T Raw_Get(int Id, List<string> Columns = null)
        {
            return Raw_Get(Id.ToString(), Columns);
        }
        public List<T> Raw_GetAll(List<string> Columns = null)
        {
            var cn = unitOfWork.Connection;
            return SqlRaw<T>.GetAll(cn, TableName);
        }
        public List<T> Raw_GetPage(ref Int64 TotalRow, int page, int pageSize, string where = "", string order = "", Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> Join = null)
        {
            var cn = unitOfWork.Connection;
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.GetPage(cn, TableName, PrimaryKey, ref TotalRow, page, pageSize, where, param, order, Columns, Join);
        }
        public List<T> Raw_QueryStringCustom(string query, Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> join = null)
        {
            var cn = unitOfWork.Connection;
            return SqlRaw<T>.QueryStringCustom(cn, TableName, query, param, Columns, join);
        }
        public IEnumerable<T1> Raw_Query<T1>(string query, Dictionary<string, object> param = null)
        {
            var cn = unitOfWork.Connection;
            param = param ?? new Dictionary<string, object>();
            var _param = new DynamicParameters();
            foreach (var e in param)
            {
                _param.Add(e.Key, e.Value);
            }
            return cn.Query<T1>(query, param: _param);
        }
        public bool Raw_TruncateTable()
        {
            var cn = unitOfWork.Connection;
            return SqlRaw<T>.TruncateTable(cn, TableName);
        }
        public bool Raw_DeleteStringCustom(string query, Dictionary<string, object> param)
        {
            var cn = unitOfWork.Connection;
            return SqlRaw<T>.DeleteStringCustom(cn, TableName, query, param);
        }
        public bool Raw_Delete(string Ids)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            return Raw_DeleteStringCustom(" where " + PrimaryKey + " in (" + Ids + ")", null);
        }
        public bool Raw_Delete(int Id)
        {
            return Raw_Delete(Id.ToString());
        }

        public bool Raw_SaveAll(List<T> datas, List<string> Columns, bool keyIdentity = true, bool IgnoreOrSave = false)
        {
            var cn = unitOfWork.Connection;
            var PrimaryKey = getPrimaryKeyColumn();
            if (!keyIdentity)
            {
                Columns.Add(PrimaryKey);
            }
            return SqlRaw<T>.SaveAll(cn, TableName, PrimaryKey, datas, Columns: Columns, IgnoreOrSave: IgnoreOrSave, Action: 1, keyIdentity: keyIdentity).Count > 0;
        }
        public bool Raw_InsertAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false)
        {
            datas = datas ?? new List<T>();
            if (datas.Count <= 0)
            {
                return false;
            }
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(datas[0], Framework.Data.Attributes.CrudFieldType.Create);
            if (!keyIdentity)
            {
                allColumns.Add(PrimaryKey);
            }
            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = unitOfWork.Connection;
            return SqlRaw<T>.SaveAll(cn, TableName, PrimaryKey, datas, Columns: allColumns, IgnoreOrSave: false, Action: 2, keyIdentity: keyIdentity).Count > 0;
        }
        public bool Raw_UpdateAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false)
        {
            datas = datas ?? new List<T>();
            if (datas.Count <= 0)
            {
                return false;
            }
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(datas[0], Framework.Data.Attributes.CrudFieldType.Update);

            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = unitOfWork.Connection;
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.SaveAll(cn, TableName, PrimaryKey, datas, Columns: allColumns, IgnoreOrSave: false, Action: 3, keyIdentity: keyIdentity).Count > 0;
        }

        public T Raw_SaveAuto(List<string> Columns, dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/)
        {
            var cn = unitOfWork.Connection;
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, keyIdentity, Columns, IgnoreOrSave, 1);

        }
        public T Raw_Insert(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(param, Framework.Data.Attributes.CrudFieldType.Update);
            if (!keyIdentity)
            {
                allColumns.Add(PrimaryKey);
            }
            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = unitOfWork.Connection;
            return SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, keyIdentity, allColumns, IgnoreOrSave: false, Action: 2);
        }
        public T Raw_Update(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(param, Framework.Data.Attributes.CrudFieldType.Update);

            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = unitOfWork.Connection;
            return SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, keyIdentity, allColumns, IgnoreOrSave: false, Action: 3);
        }

        IEnumerable<string> Raw_GetColumns(List<string> all, List<string> column, bool ignore = false)
        {
            var get_ = all.Where(e =>
            {
                return column.Count == 0 || (ignore && !column.Contains(e)) || (!ignore && column.Contains(e));
            });
            return get_;
        }
        //#endregion=====================
        /// <summary>
        /// The _table name
        /// </summary>
        private readonly string _tableName;
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T, TKey}" /> class.
        /// </summary>
        public BaseService()
        {
            _tableName = GetTableName();
        }

        #region base command
        /// <summary>
        /// Get All data
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> GetAll(IFieldPredicate predicate = null)
        {
            //https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib
            return unitOfWork.GetAll<T>(predicate);
        }

        /// <summary>
        /// Get All data by paging
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRow">The total row.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public virtual IEnumerable<T> GetPaging(int pageIndex, int pageSize, ref int totalRow)
        {
            var data = unitOfWork.GetPage<T>(null, null, pageIndex, pageSize);
            if (data.Count > 0) totalRow = data.Count;
            return data;
        }

        public virtual int Count<T>(IFieldPredicate predicate = null) where T : class
        {
            var data = unitOfWork.Count<T>(predicate);
            return data;
        }


        public T GetById(int id)
        {
            return unitOfWork.FindById<T>(id);
        }
        public T GetById(long id)
        {
            return unitOfWork.FindById<T>(id);
        }

        /// <summary>
        /// Insert new record to table
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TKey.</returns>
        public T Insert(T entity)
        {
            try
            {
                unitOfWork.Insert(entity);
                return entity;
            }
            catch (Exception ex)
            {
                //log and return
                return default(T);
            }
        }
        /// <summary>
        /// Update a record
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Update(T entity)
        {
            return unitOfWork.Update(entity);
        }
        /// <summary>
        /// Delete a record
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Delete(T entity)
        {
            return unitOfWork.Delete<T>(entity);
        }

        #endregion

        #region sql command
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException">TableAttribute not found</exception>
        private string GetTableName()
        {
            //var attr = typeof(T).CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "TableAttribute");
            //if (attr == null)
            //    throw new ArgumentNullException("TableAttribute not found");
            ////TODO: Get TableAttribute name
            //return (string)attr.ConstructorArguments[0].Value;
            var temp = typeof(T).CustomAttributes.Where(c => c.AttributeType.Name == "TableAttribute").FirstOrDefault();
            if (temp != null)
            {
                return temp.ConstructorArguments[0].Value.ToString();
            }
            return "";
        }
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetInsertCommand()
        {
            return $"sp_{TableName}_Insert";
        }
        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetUpdateCommand()
        {
            return $"sp_{TableName}_Update";
        }
        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetDeleteCommand()
        {
            return $"sp_{TableName}_Delete";
        }
        /// <summary>
        /// Gets all command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetAllCommand()
        {
            return $"sp_{TableName}_GetAll";
        }
        /// <summary>
        /// Gets the by paging command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetByPagingCommand()
        {
            return $"sp_{TableName}_GetPaging";
        }
        /// <summary>
        /// Gets the by identifier command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetByIdCommand()
        {
            return $"sp_{TableName}_GetById";
        }
        #endregion

        #region Parameters command
        /// <summary>
        /// Inserts the command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DynamicParameters.</returns>
        private DynamicParameters InsertCommand(T entity)
        {
            var allParams = DataHelper.GetSQLParametersFromPublicProperties(entity, Framework.Data.Attributes.CrudFieldType.Create);
            allParams.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            return allParams;
        }
        /// <summary>
        /// Updates the parameters.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DynamicParameters.</returns>
        private DynamicParameters UpdateParameters(T entity)
        {
            var allParams = DataHelper.GetSQLParametersFromPublicProperties(entity, Framework.Data.Attributes.CrudFieldType.Update);
            return allParams;
        }

        private string getPrimaryKeyColumn()
        {
            return getPrimaryKeyColumns().FirstOrDefault();
        }
        private IEnumerable<string> getPrimaryKeyColumns()
        {
            return typeof(T).GetProperties().Where(e =>
            {
                return Attribute.IsDefined(e, typeof(System.ComponentModel.DataAnnotations.KeyAttribute));
            }).Select(e => e.Name);
        }



        #endregion
    }
}
