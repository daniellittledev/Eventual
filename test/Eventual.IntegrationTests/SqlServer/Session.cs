using System;
using System.Data;
using System.Data.SqlClient;

namespace Eventual.IntegrationTests.SqlServer
{
    public class Session : IDisposable
    {
        public SqlConnection Connection { get; }
        public SqlTransaction Transaction { get; }

        public Session(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();

            Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Dispose()
        {
            Transaction.Dispose();
            Connection.Dispose();
        }
    }
}