// ***********************************************************************
// Assembly         : LIB
// Author           : minhdv
// Created          : 08-07-2017
//
// Last Modified By : minhdv
// Last Modified On : 08-07-2017
// ***********************************************************************
// <copyright file="IBaseServices.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    /// <summary>
    /// Interface IBaseServices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    public interface IBaseServices<T, TKey>
    {
        #region SQL Raw;
        T Raw_Get(string Id, List<string> Columns = null);
        T Raw_Get(int Id, List<string> Columns = null);
        List<T> Raw_GetAll(List<string> Columns = null);

        /// <summary>
        /// Get page
        /// </summary>
        /// <param name="Join">new Dictionary&lt;string, string&gt;() { { &quot;Columns&quot; , &quot;Column1, Column2..&quot; }, { &quot;Join&quot;, &quot;inner join Table on 1=1&quot;} };</param>
        /// <returns>T.</returns>
        List<T> Raw_GetPage(ref Int64 TotalRow, int page, int pageSize, string where = "", string order = "", Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> Join = null);
        List<T> Raw_QueryStringCustom(string query, Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> join = null);
        IEnumerable<T1> Raw_Query<T1>(string query, Dictionary<string, object> param = null);
        bool Raw_TruncateTable();
        bool Raw_DeleteStringCustom(string query, Dictionary<string, object> param);
        bool Raw_Delete(int Id);
        bool Raw_Delete(string Ids);
        bool Raw_SaveAll(List<T> datas, List<string> Columns, bool keyIdentity = true, bool IgnoreOrSave = false);
        bool Raw_InsertAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false);
        bool Raw_UpdateAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false);
        T Raw_SaveAuto(List<string> Columns, dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/);
        T Raw_Insert(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null);
        T Raw_Update(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null);
        #endregion=====================

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> GetAll(IFieldPredicate predicate = null);

        /// <summary>
        /// Gets the paging.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRow">The total row.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> GetPaging(int pageIndex, int pageSize, ref int totalRow);

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        int Count<T>(IFieldPredicate predicate = null) where T : class;

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        T GetById(int id);
        T GetById(long id);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TKey.</returns>
        T Insert(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Update(T entity);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //bool Delete(TKey id);
        bool Delete(T entity);
    }
}
