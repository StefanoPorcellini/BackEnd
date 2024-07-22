using System.Data.Common;

namespace Esercizio_M5_W1_D5.Services
{
    //servizio generico per connessione e comandi
    public abstract class ServiceBase
    {
        protected abstract DbConnection GetConnection();
        protected abstract DbCommand GetCommand(string commandText, DbConnection connection);

    }
}
