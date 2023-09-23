using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Coupon> Cupon { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Galery> Galery { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.product)  // Aquí deberías usar la propiedad de navegación "Product"
                .WithOne(p => p.inventory)
                .HasForeignKey<Product>(p => p.inventoryId);

            /* modelBuilder.Entity<Product>()
             .HasOne(p => p.galerys)  // Producto tiene una relación uno a uno con Galery
             .WithOne(g => g.product)  // Galery tiene una relación uno a uno con Product
             .HasForeignKey<Galery>(g => g.productId);  // Indica que Galery es el lado dependiente*/
        }
    }
}


/*add-migration nombre*/
/*update-database*/
/*remove-migration*/
