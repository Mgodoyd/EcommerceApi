using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1
{
    public class ApplicationDbContext : DbContext
    {
        /*
         * Aquí se tiene una DbSet por cada Entidad esto para poder mapear cada una como tabla a la base de datos.
         */
        public DbSet<User> User { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Coupon> Cupon { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Galery> Galery { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<NSale> NSales { get; set; }
        public DbSet<Contact> Contact { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /*
         *    Aquí se tiene el mapeo de las relaciones entre las entidades.
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.product)  
                .WithOne(p => p.inventory)
                .HasForeignKey<Product>(p => p.inventoryId);

            modelBuilder.Entity<Sales>()
               .HasMany(s => s.nsale)
               .WithOne(ns => ns.sales)
               .HasForeignKey(ns => ns.SalesId);
        }
    }
}


/*add-migration nombre*/
/*update-database*/
/*remove-migration*/
