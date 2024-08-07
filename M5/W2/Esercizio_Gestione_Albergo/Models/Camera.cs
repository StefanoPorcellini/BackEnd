﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class Camera
    {
        [Key]
        public int Numero { get; set; }

        [Required]
        [StringLength(255)]
        public string Desc { get; set; }
        public bool Disponibile { get; set; } = true;

        [ForeignKey("Tipologia")]
        public int TipologiaId { get; set; }

        public double Coefficiente { get; set; }

    }
}
