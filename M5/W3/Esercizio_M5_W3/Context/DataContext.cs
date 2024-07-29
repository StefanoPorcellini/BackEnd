using Esercizio_Pizzeria_In_Forno.Models;
using Microsoft.EntityFrameworkCore;

namespace Esercizio_Pizzeria_In_Forno.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt) { }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductToOrder> ProductToOrders { get; set; }

    }
}
