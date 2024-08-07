﻿using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Esercizio_Gestione_Albergo.Services.Sql
{
    //Servizio SqlService per la gestione della connectionString, comandi e connessione al DB
    public class SqlServerServiceBase : ServiceBase
    {
        private readonly string _connectionString;

        public SqlServerServiceBase(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("AlbergoDB");
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
