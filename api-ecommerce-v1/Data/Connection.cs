using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1
{
    public class Conection
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen();
            }

            string connectionString = builder.Configuration.GetConnectionString("azureDBConnect")!;

            try
            {
                using var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options);
                context.Database.EnsureCreated();
                Console.WriteLine("Conexión exitosa a la base de datos.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al conectar a la base de datos: " + e.Message);
            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
