using Esercizio_M5_W1.Models;

namespace Esercizio_M5_W1.Services.V1
{
    public interface ISpedizioneService
    {
        IEnumerable<Spedizione> GetShipById(int userId);
        void CreaSpedizione(Spedizione spedizione);

    }
}
