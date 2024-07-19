using Esercizio_M5_W1_D5.Models;

namespace Esercizio_M5_W1_D5.Services
{
    public interface IAnagraficaService
    {
        IEnumerable<Anagrafica> GetAllTrasgressore();
        void AddTrasgressore(Anagrafica trasgressore);
    }
}
