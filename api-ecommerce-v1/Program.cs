using api_ecommerce_v1;
using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Helpers;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Conexion db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer("name=ConnectionStrings:azureDBConnect"));


// Add services to the container.
 
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductBlobConfiguration, ProductBlobConfiguration>();
builder.Services.AddScoped<IInventory, InventoryService>();
builder.Services.AddScoped<ICoupon, CouponService>();
builder.Services.AddScoped<IConfig, ConfigService>();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<IGalery, GaleryService>();
builder.Services.AddScoped<ICart, CartService>();
builder.Services.AddScoped<IAddress, AddressService>();
builder.Services.AddScoped<JwtAuthorizationFilter>();
builder.Services.AddScoped<Jwthelper>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



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

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
