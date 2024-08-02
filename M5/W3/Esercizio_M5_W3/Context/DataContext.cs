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
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura il tipo di colonna per il prezzo dei prodotti
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Configura la chiave primaria composta per UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Configura la relazione molti-a-molti tra User e Role attraverso UserRole
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            // Configura la relazione tra Order e User
            modelBuilder.Entity<User>()
                .HasOne(u => u.Order)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

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

            base.OnModelCreating(modelBuilder);

            modelBuilder.SeedRoles();
        }
    }
}
