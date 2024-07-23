using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IClienteDAO
    {
        public Task<IEnumerable<ClienteViewModel>> GetAllAsync();
        public Task<ClienteViewModel> GetByCodiceFiscaleAsync(string codiceFiscale);
        public Task AddAsync(Cliente cliente);
        public Task UpdateAsync(Cliente cliente);
        public Task DeleteAsync(string codiceFiscale);
    }
}
