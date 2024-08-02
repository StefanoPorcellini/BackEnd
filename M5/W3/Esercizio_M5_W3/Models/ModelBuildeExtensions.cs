using Esercizio_Pizzeria_In_Forno.Models;
using Microsoft.EntityFrameworkCore;

namespace Esercizio_Pizzeria_In_Forno.Context
{
    public static class ModelBuilderExtensions
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );
        }
    }
}
