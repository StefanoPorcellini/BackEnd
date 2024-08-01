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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura il tipo di colonna per il prezzo dei prodotti
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Configura la relazione molti-a-molti tra User e Role
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));

            // Configura la relazione tra Order e User
            modelBuilder.Entity<User>()
                .HasOne(u => u.Order)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrderId)
                .OnDelete(DeleteBehavior.SetNull); // Imposta l'azione da eseguire quando l'ordine viene eliminato

            // Configura la relazione tra Order e ProductToOrder
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.IdOrder);

            // Configura la relazione tra ProductToOrder e Product
            modelBuilder.Entity<ProductToOrder>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.IdProduct);

            // Configura la relazione tra ProductToOrder e Order
            modelBuilder.Entity<ProductToOrder>()
                .HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.IdOrder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
