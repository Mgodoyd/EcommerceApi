using api_ecommerce_v1;
using api_ecommerce_v1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Conexion db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer("name=ConnectionStrings:azureDBConnect"));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IClienteService, ClienteService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
