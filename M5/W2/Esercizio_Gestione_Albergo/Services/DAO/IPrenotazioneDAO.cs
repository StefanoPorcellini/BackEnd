using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IPrenotazioneDAO
    {
        public Task<IEnumerable<Prenotazione>> GetAllAsync();
        public Task<PrenotazioneViewModel> GetByIdAsync(int id);
        public Task AddAsync(Prenotazione prenotazione);
        public Task UpdateAsync(PrenotazioneViewModel prenotazione);
        public Task DeleteAsync(int id);
        public Task<decimal> CalcoloTariffaAsync(int cameraNumero, int dettaglioSoggiornoId, int giorni);
        public Task<bool> IsCameraAvailableAsync(int cameraNumero, DateTime dal, DateTime al);


    }
}
