using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; } 
        public DbSet<Admin> Administrador { get; set; }
        public DbSet<Login> Login { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}

/*add-migration nombre*/
/*update-database*/
