using System.Data.Common;

namespace Esercizio_Gestione_Albergo.Services.Sql
{
    //servizio generico per connessione e comandi
    public abstract class ServiceBase
    {
        protected abstract DbConnection GetConnection();
        protected abstract DbCommand GetCommand(string commandText, DbConnection connection);

    }
}
