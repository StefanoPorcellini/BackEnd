using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Esercizio_M5_W1_D5.Services
{
    //Servizio SqlService per la gestione della connectionString, comandi e connessione al DB
    public class SqlServerServiceBase : ServiceBase
    {
        private readonly string _connectionString;

        public SqlServerServiceBase(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("PMDB");
        }

        protected override DbCommand GetCommand(string commandText, DbConnection connection)
        {
            return new SqlCommand(commandText, (SqlConnection)connection);
        }

        protected override DbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
