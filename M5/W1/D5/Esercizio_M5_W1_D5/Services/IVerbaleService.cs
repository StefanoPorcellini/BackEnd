using Esercizio_M5_W1_D5.Models;

namespace Esercizio_M5_W1_D5.Services
{
    public interface IVerbaleService
    {
        void AddVerbale(Verbale verbale);
        IEnumerable<Verbale> GetAllVerbali();
        IEnumerable<Verbale> GetByAnagraficaId(int anagraficaId);
        Verbale GetById(int id);
        IEnumerable<ViolazioneDettaglio> GetViolazioniOltre10Punti();
        IEnumerable<ViolazioneDettaglio> GetViolazioniOltre400Euro();
    }
}
