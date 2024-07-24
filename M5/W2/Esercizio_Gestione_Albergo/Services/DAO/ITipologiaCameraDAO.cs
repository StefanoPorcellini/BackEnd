using Esercizio_Gestione_Albergo.Models;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface ITipologiaCameraDAO
    {
        IEnumerable<TipologiaCamera> GetAll();

    }
}
