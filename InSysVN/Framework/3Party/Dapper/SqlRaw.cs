using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;
using System.Dynamic;

namespace Dapper.SQLRaw
{
    public class SqlRaw<T>
    {

        public static string[] keywordSQL = new[] { "like", "index", "order", "select", "where", "as", "insert", "update" };

        public static T Get(IDbConnection cn, string tableName, string nameFieldPrimaryKey, string id, List<string> Columns = null, string _schema = "dbo")
        {
            Columns = Columns ?? new List<string>();
            Columns = Columns.Select(delegate (string name)
            {
                if (keywordSQL.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    name = "[" + name + "]";
                }
                return name;
            }).ToList();
            var selectColumns = Columns.Count != 0 ? string.Join(", ", Columns) : "*";
            T item = default(T);
            if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            var query = string.Format("SELECT {3} FROM [{0}].{1} WHERE {2} = @ID", _schema, tableName, nameFieldPrimaryKey, selectColumns);
            item = cn.Query<T>(query, new { ID = id }).SingleOrDefault();
            //using (IDbConnection _cn = cn)
            //{
            //    _cn.Open();
            //    var query = string.Format("SELECT {3} FROM [{0}].{1} WHERE {2} = @ID", _schema, tableName, nameFieldPrimaryKey, selectColumns);
            //    item = cn.Query<T>(query, new { ID = id }).SingleOrDefault();
            //    _cn.Close();
            //}
            return item;
        }
        public static List<T> GetAll(IDbConnection cn, string tableName, List<string> Columns = null, string _schema = "dbo")
        {
            Columns = Columns ?? new List<string>();
            Columns = Columns.Select(delegate (string name)
            {
                if (keywordSQL.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    name = "[" + name + "]";
                }
                return name;
            }).ToList();
            var selectColumns = Columns.Count != 0 ? string.Join(", ", Columns) : "*";
            List<T> items = new List<T>();
            if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            var query = string.Format(@"
SELECT
    {2}
FROM 
[{0}].{1}", _schema, tableName, selectColumns);
            items = cn.Query<T>(query, new { }).ToList();
            //            using (IDbConnection _cn = cn)
            //            {
            //                _cn.Open();
            //                var query = string.Format(@"
            //SELECT
            //    {3}
            //FROM 
            //[{0}].{1}", _schema, tableName, selectColumns);
            //                items = cn.Query<T>(query, new { }).ToList();
            //                _cn.Close();
            //            }
            return items;
        }
        public static List<T> GetPage(IDbConnection cn, string tableName, string nameFieldPrimaryKey, ref Int64 TotalRow, int page, int pageSize, string where = "", Dictionary<string, object> _param = null, string order = "", List<string> Columns = null, Dictionary<string, string> join = null, string _schema = "dbo")
        {
            _param = _param ?? new Dictionary<string, object>();
            Columns = Columns ?? new List<string>();
            Columns = Columns.Select(delegate (string name)
            {
                if (keywordSQL.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    name = "[" + name + "]";
                }
                return name + "=" + "tableTemp." + name;
            }).ToList();
            var selectColumns = Columns.Count != 0 ? string.Join(", ", Columns) : "*";

            var joinTxt = "";
            if (join != null)
            {
                selectColumns = join["Columns"] as string;
                joinTxt = join["Join"] as string;
            }

            order = string.IsNullOrWhiteSpace(order) ? nameFieldPrimaryKey : order;
            order = order.Trim();
            if (string.IsNullOrEmpty(order))
            {
                order = typeof(T).GetProperties()[0].Name;
            }

            where = string.IsNullOrEmpty(where) ? "where 1=1" : ("where " + where);

            var _return = new List<T>();

            var query = string.Format(@"
                                        select
                                            @TotalRow = count(*)
                                        from(
                                            SELECT 
                                                {5}
                                            FROM {0} tableTemp 
                                            {6} 
                                        )tableTemp
                                        {1}

                                        ;WITH TempResult as (
                                            select
                                                *
                                            from(
                                                SELECT 
                                                    {5}
                                                FROM {0} tableTemp 
                                                {6} 
                                            )tableTemp
                                            {1}
                                        ),
                                        TempCount as (
                                            select
                                                COUNT(*) As 'TotalRow'
                                            from TempResult
                                        )
                                        select
                                            *
                                        from TempResult , TempCount
                                        Order by {2}
                                        OFFSET (({3} - 1)* {4}) ROW FETCH NEXT {4} ROW ONLY
                                        ",
                                        /*0*/"[" + _schema + "]." + tableName,
                                        /*1*/where,
                                        /*2*/order,
                                        /*3*/page,
                                        /*4*/pageSize,
                                        /*5*/selectColumns,
                                        /*6*/joinTxt
                                    );

            var param = new DynamicParameters();
            foreach (var e in _param)
            {
                param.Add(e.Key, e.Value);
            }
            param.Add("@TotalRow", direction: ParameterDirection.Output, dbType: DbType.Int64);
            try
            {
                if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                _return = cn.Query<T>(query, param).ToList();
                TotalRow = param.Get<Int64>("@TotalRow");
                //using (IDbConnection _cn = cn)
                //{
                //    _cn.Open();
                //    _return = _cn.Query<T>(query, param).ToList();
                //    TotalRow = param.Get<Int64>("@TotalRow");
                //    _cn.Close();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _return;
        }
        public static List<T> QueryStringCustom(IDbConnection cn, string tableName, string _query, Dictionary<string, object> _param = null, List<string> Columns = null, Dictionary<string, string> _join = null, string _schema = "dbo")
        {
            Columns = Columns ?? new List<string>();
            Columns = Columns.Select(delegate (string name)
            {
                if (keywordSQL.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    name = "[" + name + "]";
                }
                return name;
            }).ToList();
            var selectColumns = Columns.Count != 0 ? string.Join(", ", Columns) : "*";

            var join = "";
            if (_join != null)
            {
                selectColumns = _join["Columns"] as string;
                join = _join["Join"] as string;
            }

            _param = _param ?? new Dictionary<string, object>();
            var _return = new List<T>();
            var query = string.Format(@"
SELECT
    {2}
FROM
    {0} tableTemp
{3}
{1}", "[" + _schema + "]." + tableName, _query, selectColumns, join);

            var param = new DynamicParameters();
            foreach (var e in _param)
            {
                param.Add(e.Key, e.Value);
            }
            if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            _return = cn.Query<T>(query, param).ToList();
            //using (IDbConnection cn_ = cn)
            //{
            //    cn_.Open();
            //    _return = cn_.Query<T>(query, param).ToList();
            //    cn_.Close();
            //}
            return _return;
        }
        public static bool TruncateTable(IDbConnection cn, string tableName, string _schema = "dbo")
        {
            var query = string.Format("truncate table [{0}].{1}", _schema, tableName);

            var param = new DynamicParameters(); if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            var _return = cn.Query<T>(query, param);
            //using (IDbConnection cn = cn_)
            //{
            //    cn.Open();
            //    var _return = cn.Query<T>(query, param);
            //    cn.Close();
            //}
            return true;
        }
        public static bool DeleteStringCustom(IDbConnection cn, string tableName, string _query, Dictionary<string, object> _param, string _schema = "dbo")
        {
            _param = _param ?? new Dictionary<string, object>();
            var query = string.Format("DELETE [{0}].{1} {2}", _schema, tableName, _query);

            var param = new DynamicParameters();
            foreach (var e in _param)
            {
                param.Add(e.Key, e.Value);
            }
            if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            var _return = cn.Query<T>(query, param);
            //using (IDbConnection cn = cn_)
            //{
            //    cn.Open();
            //    var _return = cn.Query<T>(query, param);
            //    cn.Close();
            //}
            return true;
        }

        public static T Save(IDbConnection cn, string tableName, string nameFieldPrimaryKey, dynamic param, bool keyIdentity = true, List<string> Columns = null, bool IgnoreOrSave = false, int Action = 1 /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/)
        {
            T item = default(T);
            if (cn.State == ConnectionState.Broken || cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            //using (IDbConnection cn = cn_)
            //{
            //    cn.Open();
            T temp = default(T);
            if (Action == 1)
            {
                if (!string.IsNullOrEmpty(nameFieldPrimaryKey))
                {
                    var id = param.GetType().GetProperty(nameFieldPrimaryKey).GetValue(param, null);
                    temp = Get(cn, tableName, nameFieldPrimaryKey, id.ToString());
                }
                if (string.IsNullOrEmpty(nameFieldPrimaryKey) || temp == null)
                {
                    return Save(cn, tableName, nameFieldPrimaryKey, param, keyIdentity, Columns, IgnoreOrSave, 2);
                }
                else
                {
                    return Save(cn, tableName, nameFieldPrimaryKey, param, keyIdentity, Columns, IgnoreOrSave, 3);
                }
            }
            else if (Action == 2)
            {
                var query = GetInsertQuery(tableName, param, nameFieldPrimaryKey, Columns, IgnoreOrSave, keyIdentity);
                //var query = DynamicQuery.GetUpdateQuery(table, param, nameFieldPrimaryKey);
                IEnumerable<T> result = SqlMapper.Query<T>(cn, query, param);
                //item = result.First();
                var inserted = result.First();
                if (!string.IsNullOrEmpty(nameFieldPrimaryKey))
                {
                    var value = inserted.GetType().GetProperty(nameFieldPrimaryKey).GetValue(inserted, null);
                    PropertyInfo property = param.GetType().GetProperty(nameFieldPrimaryKey);
                    property.SetValue(param, value, null);
                }
                item = param;
            }
            else if (Action == 3)
            {
                var query = GetUpdateQuery(tableName, param, nameFieldPrimaryKey, Columns, IgnoreOrSave);
                IEnumerable<T> result = SqlMapper.Query<T>(cn, query, param);
                item = param;
            }
            //}
            return item;
        }

        static string GetInsertQuery(string tableName, dynamic item, string nameFieldPrimaryKey, List<string> Columns = null, bool IgnoreOrSaveColumns = false, bool keyIdentity = true)
        {
            Columns = Columns ?? new List<string>();
            IgnoreOrSaveColumns = Columns.Count == 0 ? false : IgnoreOrSaveColumns;
            PropertyInfo[] props = item.GetType().GetProperties();
            var columns = props.Select(delegate (PropertyInfo p)
            {
                var name = p.Name;
                if (keywordSQL.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    name = "[" + name + "]";
                }
                return new { ColName = name, AttrName = p.Name };
            }).Where(s => s.ColName != nameFieldPrimaryKey || !keyIdentity)
            .Where(delegate (dynamic e)
            {
                var search = Columns.Find(e1 => e1.Equals(e.AttrName));
                return Columns.Count == 0 || (IgnoreOrSaveColumns && search == null) || (!IgnoreOrSaveColumns && search != null);
            }).ToArray();

            return string.Format("INSERT INTO {0} ({1}) OUTPUT inserted.{3} VALUES (@{2})",
                                 tableName,
                                 string.Join(",", columns.Select(e => e.ColName)),
                                 string.Join(",@", columns.Select(e => e.AttrName)),
                                 nameFieldPrimaryKey == "" ? "*" : nameFieldPrimaryKey);
        }
        static string GetUpdateQuery(string tableName, dynamic item, string nameFieldPrimaryKey, List<string> Columns = null, bool IgnoreOrSaveColumns = false)
        {
            Columns = Columns ?? new List<string>();
            IgnoreOrSaveColumns = Columns.Count == 0 ? false : IgnoreOrSaveColumns;

            PropertyInfo[] props = item.GetType().GetProperties();
            var columns = props.Select(delegate (PropertyInfo p)
            {
                var name = p.Name;
                if (keywordSQL.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    name = "[" + name + "]";
                }
                var a = new { ColName = name, AttrName = p.Name };
                return new { ColName = name, AttrName = p.Name };
            }).Where(s => s.ColName != nameFieldPrimaryKey)
            .Where(delegate (dynamic e)
            {
                var search = Columns.Find(e1 => e1.Equals(e.AttrName));
                return Columns.Count == 0 || (IgnoreOrSaveColumns && search == null) || (!IgnoreOrSaveColumns && search != null);
            })
            .ToArray();

            var parameters = columns.Select(obj => obj.ColName + "=@" + obj.AttrName).ToList();

            return string.Format("UPDATE {0} SET {1} WHERE {2} = @{2}", tableName, string.Join(", ", parameters), nameFieldPrimaryKey);
        }
    }
}
