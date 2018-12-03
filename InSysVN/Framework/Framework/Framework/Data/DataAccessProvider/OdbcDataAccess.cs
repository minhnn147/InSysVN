using System.Data;
using System.Data.Odbc;

namespace Framework.Data.DataAccessProvider
{
	/// <summary>
	/// The SQLDataAccessLayer contains the data access layer for Odbc data provider. 
	/// This class implements the abstract methods in the DataAccessLayerBaseClass class.
    /// Base: Minhduongvan
    /// date: 2014/10/20
	/// </summary>
    public class OdbcDataAccessLayer : DataProviderBaseClass
	{
		// Provide class constructors
		public OdbcDataAccessLayer() {}
		public OdbcDataAccessLayer(string connectionString) { this.ConnectionString = connectionString;}

		// DataAccessLayerBaseClass Members
		internal override IDbConnection GetDataProviderConnection()
		{
			return new OdbcConnection();
		}
		internal override IDbCommand GeDataProviderCommand()
		{
			return new OdbcCommand();
		}

		internal override IDbDataAdapter GetDataProviderDataAdapter()
		{
			return new OdbcDataAdapter();
		}
	}
}
