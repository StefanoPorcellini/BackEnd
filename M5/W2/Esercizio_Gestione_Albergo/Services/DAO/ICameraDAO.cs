using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public interface ICameraDAO
    {
        public Task<IEnumerable<CameraViewModel>> GetCamere();

    }
}
