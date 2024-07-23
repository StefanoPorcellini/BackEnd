using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IPrenotazioneDAO
    {
        public Task<IEnumerable<Prenotazione>> GetAllAsync();
        public Task<Prenotazione> GetByIdAsync(int id);
        public Task AddAsync(Prenotazione prenotazione);
        public Task UpdateAsync(Prenotazione prenotazione);
        public Task DeleteAsync(int id);
    }
}
