using System.Data;
using System.Data.OleDb;

namespace Framework.Data.DataAccessProvider
{
	/// <summary>
	/// The SQLDataAccessLayer contains the data access layer for Oracle data provider. 
	/// This class implements the abstract methods in the DataAccessLayerBaseClass class.
    /// Base: Minhduongvan
    /// date: 2014/10/20
	/// </summary>
    public class OracleDataAccessLayer : DataProviderBaseClass
	{
		// Provide class constructors
		public OracleDataAccessLayer() {}
		public OracleDataAccessLayer(string connectionString) { ConnectionString = connectionString;}

		// DataAccessLayerBaseClass Members
		internal override IDbConnection GetDataProviderConnection()
		{
            return new OleDbConnection();
			//return new OracleConnection();
		}
		internal override IDbCommand GeDataProviderCommand()
		{
            return new OleDbCommand();
			//return new OracleCommand();
		}

		internal override IDbDataAdapter GetDataProviderDataAdapter()
		{
            return new OleDbDataAdapter();
			//return new OracleDataAdapter();
		}
	}
}
