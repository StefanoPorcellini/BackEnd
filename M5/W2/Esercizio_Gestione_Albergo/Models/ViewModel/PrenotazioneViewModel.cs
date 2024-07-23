using Esercizio_Gestione_Albergo.Models;
using System;
using System.Collections.Generic;

namespace Esercizio_Gestione_Albergo.ViewModels
{
    public class PrenotazioneViewModel : Prenotazione
    {
        public ClienteViewModel Cliente { get; set; }
        public CameraViewModel Camera { get; set; }
        public DettagliSoggiorno DettagliSoggiorno { get; set; }
        public List<ServizioAggiuntivo> ServiziAggiuntivi { get; set; }
    }
}
