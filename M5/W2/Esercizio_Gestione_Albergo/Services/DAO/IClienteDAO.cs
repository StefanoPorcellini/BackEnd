using System.Collections.Generic;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface IClienteDAO
    {
        IEnumerable<ClienteViewModel> GetAll();
        ClienteViewModel GetByCodiceFiscale(string codiceFiscale);
        void Add(Cliente cliente);
        void Update(Cliente cliente);
        void Delete(string codiceFiscale);
    }
}
