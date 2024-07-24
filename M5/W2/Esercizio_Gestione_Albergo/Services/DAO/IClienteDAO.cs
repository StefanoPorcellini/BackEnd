using System.Collections.Generic;
using System.Threading.Tasks;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IClienteDAO
    {
        Task<IEnumerable<ClienteViewModel>> GetAll();
        Task<ClienteViewModel> GetByCodiceFiscale(string codiceFiscale);
        Task Add(Cliente cliente);
        Task Update(Cliente cliente);
        Task Delete(string codiceFiscale);
    }
}
