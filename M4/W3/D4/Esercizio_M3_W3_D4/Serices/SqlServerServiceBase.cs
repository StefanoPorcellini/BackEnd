using System.Data.Common;
using System.Data.SqlClient;

namespace Esercizio_M3_W3_D4.Serices
{
    public class SqlServerServiceBase : ServiceBase
    {
        private SqlConnection _connection;

        public SqlServerServiceBase(IConfiguration config)
        {
            _connection = new SqlConnection(config.GetConnectionString("Esercizio_M3_W3_D4"));
        }

        protected override DbCommand GetCommand(string commandText)
        {
            return new SqlCommand(commandText, _connection);
        }

        protected override DbConnection GetConnection()
        {
            return _connection;
        }
    }
}
