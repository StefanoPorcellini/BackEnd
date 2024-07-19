using Esercizio_M5_W1_D5.Models;

namespace Esercizio_M5_W1_D5.Services
{
    public interface IViolazioneService
    {
        IEnumerable<Violazione> GetAllViolazioni();
        void AddViolazione(Violazione violazione);
    }
}
