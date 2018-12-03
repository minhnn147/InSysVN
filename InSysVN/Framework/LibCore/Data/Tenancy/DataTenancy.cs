using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using StackExchange.Profiling;
using LibCore.Data.DataAccessProvider;
using log4net;
using Framework.Data;

namespace LibCore.Data.Tenancy
{
    public class DataTenancy
    {
        public Server Server = null;
        public Database MainDb = null;
        public static List<Table> ListTable = new List<Table>();
        public static List<View> ListView = new List<View>();
        private static readonly ILog log = LogManager.GetLogger("QueryTenant");

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTenancy"/> class.
        /// </summary>
        public DataTenancy(string server = null, string userAuthen = null, string passAuthen = null, string dbUse = null)
        {
            var sServer = string.IsNullOrEmpty(server) ? ConfigurationManager.AppSettings["StringServer"] : server;
            var sUser = string.IsNullOrEmpty(userAuthen) ? ConfigurationManager.AppSettings["ConnectionContextLogin"] : userAuthen;
            var sPassword = string.IsNullOrEmpty(passAuthen) ? ConfigurationManager.AppSettings["ConnectionContextPassword"] : passAuthen;
            var selDb = string.IsNullOrEmpty(dbUse) ? ConfigurationManager.AppSettings["DBInteraction"] : dbUse;
            Server = new Server(sServer);
            Server.ConnectionContext.LoginSecure = false;
            Server.ConnectionContext.Login = sUser;
            Server.ConnectionContext.Password = sPassword;
            MainDb = Server.Databases[selDb];
        }

        /// <summary>
        /// Gets all schema db.
        /// </summary>
        /// <returns></returns>
        public SchemaCollection GetAllSchemaDb()
        {
            return MainDb.Schemas;
        }

        public string MakeTenantNew(string schema = "")
        {
            if (!string.IsNullOrEmpty(schema))
            {
                var s = "";
                var tenancy = new DataTenancy();
                List<string> functions = tenancy.GetAllFunction(schema);
                List<string> procedures = tenancy.GetAllProcedures(schema);
                var adoProvider = new ADOProvider(DataHelper.TenantInternal().ToLower());
                using (MiniProfiler.Current.Step("MakeTenantNew"))
                {
                    List<string> tables = tenancy.GetAllTablesInDbSchemaInternal();
                    foreach (var table in tables)
                    {
                        using (MiniProfiler.Current.Step("Create table " + table))
                        {

                            //s +=
                            s =
                                tenancy.GenScriptFromTable(table)
                                       .Replace("[" + DataHelper.TenantInternal().ToLower() + "]", "[" + schema + "]") +
                                "\n";

                            log.Info("-----------------------------Script Table---------------------");
                            log.Info(s);
                            log.Info("-----------------------------End Script Table---------------------");
                            tenancy.ExecuteScript(s);
                            log.Info("-----------------------------Complete Script Table---------------------");

                            //Copy data
                            var querySystemColumn = string.Format("SELECT * FROM sys.columns " +
                                                                  "WHERE object_id = object_id('[{0}].[{1}]')",
                                                                  DataHelper.TenantInternal().ToLower(), table);

                            var datas = adoProvider.Query<SystemColumn>(querySystemColumn);
                            log.Info("-----------------------------query SystemColumn---------------------");
                            log.Info(querySystemColumn);
                            log.Info("-----------------------------End query SystemColumn---------------------");
                            if (datas == null) continue;
                            var isIdentity = false;
                            var fields = datas.ToList();
                            var listFields = new List<string>();
                            foreach (SystemColumn field in fields)
                            {
                                if (field.is_identity == 1)
                                {
                                    isIdentity = true;
                                }
                                listFields.Add(string.Format("[{0}]", field.name));
                            }
                            var sql = string.Format("INSERT INTO [{0}].[{1}]({3}) SELECT * FROM [{2}].[{1}]", schema, table,
                                                    DataHelper.TenantInternal().ToLower(), string.Join(",", listFields));
                            if (isIdentity)
                            {
                                sql = string.Format("SET IDENTITY_INSERT [{0}].[{1}] ON "
                                    + sql
                                    + "SET IDENTITY_INSERT [{0}].[{1}] OFF "
                                    , schema, table);
                            }
                            log.Info("-----------------------------Create table---------------------");
                            log.Info(sql);
                            log.Info("-----------------------------End Create table---------------------");
                            tenancy.ExecuteScript(sql);
                            //+ "<br /><br /><br /><br />"
                        }
                    }

                    //if (procedures.Count > 0)
                    //{
                    //    var sProcedures = string.Join("\n\n", procedures);
                    //    log.Info("-----------------------------Create procedure---------------------");
                    //    log.Info(sProcedures);
                    //    log.Info("-----------------------------End Create procedure---------------------");
                    //    tenancy.ExecuteScript(sProcedures);
                    //}

                    foreach (var procedure in procedures)
                    {
                        var sProcedure = procedure
                            .Replace("SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON", "")
                            .Replace("[tenant].", "[" + schema + "].");
                        log.Info("-----------------------------Create procedure---------------------");
                        log.Info(sProcedure);
                        log.Info("-----------------------------End Create procedure---------------------");
                        tenancy.ExecuteScript(sProcedure);
                    }

                    foreach (var func in functions)
                    {
                        var sFunc = func
                            .Replace("SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON", "")
                            .Replace("[tenant].", "[" + schema + "].");
                        log.Info("-----------------------------Create function---------------------");
                        log.Info(sFunc);
                        log.Info("-----------------------------End Create function---------------------");
                        tenancy.ExecuteScript(sFunc);
                    }

                    
                    return "";
                }
            }
            return "";
        }

        /// <summary>
        /// Gets all procedures.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllProcedures(string schema)
        {
            using (MiniProfiler.Current.Step("GetAllProcedures"))
            {
                var listProcedures = new List<string>();
                var storeCollection = MainDb.StoredProcedures;
                foreach (StoredProcedure sp in storeCollection)
                {
                    if (sp.Schema != DataHelper.TenantInternal() || sp.IsSystemObject) continue;
                    var stringCollection = sp.Script();

                    var aStringCollection = new string[stringCollection.Count];
                    stringCollection.CopyTo(aStringCollection, 0);
                    var sProcedure = string.Join(" ", aStringCollection);
                    sProcedure =
                        sProcedure
                            .Replace("CREATE PROCEDURE [" + DataHelper.TenantInternal().ToLower() + "].",
                                     "CREATE PROCEDURE [" + schema + "].") + "\n";
                    //.Replace("SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON", "");

                    listProcedures.Add(sProcedure);
                }
                return listProcedures;
            }
        }

        /// <summary>
        /// Gets all procedures.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFunction(string schema)
        {
            using (MiniProfiler.Current.Step("GetAllProcedures"))
            {
                var list = new List<string>();
                var userDefinedFunctions = MainDb.UserDefinedFunctions;
                foreach (UserDefinedFunction data in userDefinedFunctions)
                {
                    if (data.Schema != DataHelper.TenantInternal() || data.IsSystemObject) continue;
                    var stringCollection = data.Script();

                    var aStringCollection = new string[stringCollection.Count];
                    stringCollection.CopyTo(aStringCollection, 0);
                    var sData = string.Join(" ", aStringCollection);
                    sData =
                        sData
                            .Replace("CREATE FUNCTION [" + DataHelper.TenantInternal().ToLower() + "].",
                                     "CREATE FUNCTION [" + schema + "].") + "\n";
                    //.Replace("SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON", "");

                    list.Add(sData);
                }
                return list;
            }
        }

        /// <summary>
        /// Gets all tables in database.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTablesInDb(string schema = "dbo")
        {
            var list = new List<string>();
            var tbCollection = MainDb.Tables;
            foreach (Table tb in tbCollection)
            {
                if (tb.Schema == schema && !tb.IsSystemObject)
                {
                    list.Add(tb.Name);
                    ListTable.Add(tb);
                }
            }
            return list;
        }

        public List<string> GetAllTablesInDbSchemaInternal()
        {
            var list = GetAllTablesInDb(DataHelper.TenantInternal());
            return list;
        }

        /// <summary>
        /// Generate the script from table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public string GenScriptFromTable(string table, string schema = "dbo")
        {
            //var currentTable = MainDb.Tables[table];
            var currentTable = ListTable.Find(t => t.Name == table);
            var existsAndDrop = "";
            var createTable = "";
            if (currentTable != null)
            {
                var options = new ScriptingOptions
                    {
                        //IncludeIfNotExists = true,
                        //ClusteredIndexes = true,
                        Default = true,
                        FullTextIndexes = true,
                        Indexes = true,
                        //ScriptData = true,
                        //ScriptDrops = true,
                        ScriptSchema = true,
                        DriDefaults = true, // Default value for field
                    };
                //Generating IF EXISTS and DROP command for tables
                existsAndDrop = EnumerableString2String(currentTable.EnumScript(options));
                //existsAndDrop = ConvertStringCollection2String(currentTable.Script(options));

                //Generating CREATE TABLE command
                //createTable = ConvertStringCollection2String(currentTable.Script());
                createTable = EnumerableString2String(currentTable.EnumScript(options));
            }

            //return existsAndDrop + createTable;
            //return existsAndDrop;
            return createTable;
        }

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public int ExecuteScript(string script)
        {
            var dataProvider = DataProvider.GetDataProvider(EnumDataProviderType.SqlClient, DataHelper.GetConnectionString());
            return dataProvider.ExecuteNonQuery(script);
        }

        /// <summary>
        /// Converts the string collection to string.
        /// </summary>
        /// <param name="strCollection">The STR collection.</param>
        /// <returns></returns>
        protected string ConvertStringCollection2String(StringCollection strCollection)
        {
            var s = new string[strCollection.Count];
            strCollection.CopyTo(s, 0);
            return string.Join(" ", s);
        }

        /// <summary>
        /// Converts the string collection2 string.
        /// </summary>
        /// <param name="strCollection">The STR collection.</param>
        /// <returns></returns>
        protected string EnumerableString2String(IEnumerable<string> strCollection)
        {
            return string.Join(" ", strCollection.ToArray());
        }

        /// <summary>
        /// Get All View in Schema
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public List<string> GetAllViewInDb(string schema = "dbo")
        {
            var tbCollection = MainDb.Views;
            return (from View view in tbCollection where view.Schema == schema && !view.IsSystemObject select view.Name).ToList();
        }


        public List<DBField> GetAllFieldsInTable(string schema, string tableName, bool isTable)
        {
            var list = new List<DBField>();
            if(isTable)
            {
                var tbCollection = MainDb.Tables;
                foreach (Table tb in tbCollection)
                {
                    if (tb.Schema == schema && !tb.IsSystemObject && tb.Name == tableName)
                    {
                        var columns = tb.Columns;
                        list.AddRange(from Column column in columns 
                                      select new DBField
                                                 {
                                                     Name = column.Name,
                                                     DbType = column.DataType.ToString(),
                                                     Length = column.DataType.MaximumLength,
                                                     Identity = column.Identity,
                                                     Required = !column.Nullable
                                                 });
                        ListTable.Add(tb);
                    }
                }
            }
            else
            {
                var viewCollection = MainDb.Views;
                foreach (View v in viewCollection)
                {
                    if (v.Schema == schema && !v.IsSystemObject && v.Name == tableName)
                    {
                        var columns = v.Columns;
                        list.AddRange(from Column column in columns
                                      select new DBField
                                      {
                                          Name = column.Name,
                                          DbType = column.DataType.ToString(),
                                          Length = column.DataType.MaximumLength,
                                          Identity = column.Identity
                                      });
                        ListView.Add(v);
                    }
                }
            }
           
            return list;
        }
    }

    public class SystemColumn
    {
        public long object_id;
        public string name;
        public int column_id;
        public int system_type_id;
        public int user_type_id;
        public int max_length;
        public int precision;
        public int scale;
        public string collation_name;
        public int is_nullable;
        public int is_ansi_padded;
        public int is_rowguidcol;
        public int is_identity;
        public int is_computed;
        public int is_filestream;
        public int is_relicated;
        public int is_non_sql_subscribed;
        public int is_merge_published;
        public int is_dts_relicated;
        public int is_xml_document;
        public int xml_collection_id;
        public long default_object_id;
        public int rule_object_id;
        public int is_sparse;
        public int is_column_set;
    }

    public class DBField
    {
        public string Name { get; set; }
        public string DbType { get; set; }
        public int Length { get; set; }
        public bool Identity { get; set; }
        public bool Required { get; set; }
    }
}
