using System.Data.Common;

namespace Esercizio_M3_W3_D4.Serices
{
    public abstract class ServiceBase
    {
        protected abstract DbConnection GetConnection();
        protected abstract DbCommand GetCommand(string commandText);
    }
}
