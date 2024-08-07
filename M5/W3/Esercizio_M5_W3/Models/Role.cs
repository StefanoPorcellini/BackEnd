﻿using Esercizio_Pizzeria_In_Forno.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esercizio_Pizzeria_In_Forno.Models
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public required string Name { get; set; }
        public virtual List<User> Users { get; set; } = [];
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();


    }
}
