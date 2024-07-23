using System.Collections.Generic;

namespace Esercizio_Gestione_Albergo.ViewModels
{
    public class ClienteViewModel : Cliente
    {
        public List<PrenotazioneViewModel> Prenotazioni { get; set; }
    }
}
