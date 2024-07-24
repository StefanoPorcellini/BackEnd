using Esercizio_Gestione_Albergo.Models;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IDettaglioSoggiornoDAO
    {
        IEnumerable<DettagliSoggiorno> GetAll();

    }
}
