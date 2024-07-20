using System;
using System.ComponentModel.DataAnnotations;

namespace Esercizio_M5_W1_D5.Models
{
    public class Verbale
    {
        public int IDVerbale { get; set; }

        [Required(ErrorMessage = "La data della violazione è obbligatoria")]
        public DateTime DataViolazione { get; set; }

        [Required(ErrorMessage = "L'indirizzo della violazione è obbligatorio")]
        public string IndirizzoViolazione { get; set; }

        [Required(ErrorMessage = "Il nominativo dell'agente è obbligatorio")]
        public string NominativoAgente { get; set; }

        [Required(ErrorMessage = "La data di trascrizione del verbale è obbligatoria")]
        public DateTime DataTrascrizioneVerbale { get; set; }

        [Required(ErrorMessage = "Il trasgressore è obbligatorio")]
        public int Anagrafica_FK { get; set; }

        [Required(ErrorMessage = "La violazione è obbligatoria")]
        public int Violazione_FK { get; set; }

    }
}
