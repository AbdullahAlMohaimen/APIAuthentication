using Microsoft.Data.SqlClient;

namespace APIAuthentication
{
	public class TransactionContext
	{
		private string _connectionString;
		private SqlConnection _connection;
		private SqlTransaction _transaction;

		public TransactionContext()
		{
			_connectionString = ConfigurationManager.ConnectionString;
			_connection = new SqlConnection(_connectionString);
		}
		public void Begin()
		{
			_connection.Close();
			_connection.Open();
			_transaction = _connection.BeginTransaction();
		}
		public SqlConnection Connection => _connection;
		public SqlTransaction Transaction => _transaction;
		public void End()
		{
			_transaction.Commit();
			_connection.Close();
		}
		public void Close()
		{
			_connection.Close();
		}
		public void Commit()
		{
			_transaction.Commit();
		}
		public void Rollback()
		{
			_transaction.Rollback();
		}
		public void Dispose()
		{
			_transaction.Dispose();
			_connection.Close();
			_connection.Dispose();
		}
	}
}
