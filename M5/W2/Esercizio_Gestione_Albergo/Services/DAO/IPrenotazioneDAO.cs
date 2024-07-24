using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IPrenotazioneDAO
    {
        public Task<IEnumerable<PrenotazioneViewModel>> GetAllAsync();
        public Task<PrenotazioneViewModel> GetByIdAsync(int id);
        public Task AddAsync(PrenotazioneViewModel prenotazione);
        public Task UpdateAsync(PrenotazioneViewModel prenotazione);
        public Task DeleteAsync(int id);
    }
}
