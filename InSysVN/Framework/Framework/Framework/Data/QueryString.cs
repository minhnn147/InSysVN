using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data
{

    /// <summary>
    /// Class QueryString
    /// </summary>
    public class QueryString
    {
        /// <summary>
        /// Creates the select query.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>System.String.</returns>
        public static string CreateSelectQuery(string schema, string table, string selector = "*")
        {
            return "SELECT " + selector + " FROM [" + schema + "]." + table;
        }

        /// <summary>
        /// Creates the select query.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="where">The where.</param>
        /// <returns>System.String.</returns>
        public static string CreateSelectQuery(string schema, string table, string selector = "*", string where = null)
        {
            string query = QueryString.CreateSelectQuery(schema, table, selector);
            if (!string.IsNullOrEmpty(where))
            {
                query += " WHERE " + where;
            }
            return query;
        }

        /// <summary>
        /// Creates the select query.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>System.String.</returns>
        public static string CreateSelectQuery(string schema, string table, string selector = "*", string where = null, string orderBy = null)
        {
            string query = QueryString.CreateSelectQuery(schema, table, selector, where);
            if (!string.IsNullOrEmpty(orderBy))
            {
                query += " ORDER BY " + orderBy;
            }
            return query;
        }

        /// <summary>
        /// Creates the select query.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>System.String.</returns>
        public static string CreateSelectQuery(string schema, string table, string selector = "*", string where = null, string orderBy = null, int offset = 0, int limit = 0)
        {
            if (string.IsNullOrEmpty(orderBy) || offset < 0 || limit <= 0)
                return QueryString.CreateSelectQuery(schema, table, selector, where);
            else
            {
                return "SELECT * FROM (SELECT " + selector + ", ROW_NUMBER() OVER (ORDER BY " + orderBy + ") as row WHERE " + where + " FROM " + table + ") WHERE row >= " + offset.ToString() + " AND row <= " + (offset + limit).ToString();
            }
        }
    }
}
