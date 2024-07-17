using System.Data.Common;

namespace Esercizio_M5_W1.Services.V1
{
    public abstract class ServiceBase
    {
        protected abstract DbConnection GetConnection();
        protected abstract DbCommand GetCommand(string commandText, DbConnection connection);

    }
}
