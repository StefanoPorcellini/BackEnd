using Esercizio_M5_W1.Models;

namespace Esercizio_M5_W1.Services.V1
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAll();
        void Create(Cliente cliente);
        void Update(Cliente cliente);
        void Delete(int Id);
        Cliente GetById(int Id);

    }
}
