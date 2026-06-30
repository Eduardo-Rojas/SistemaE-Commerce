
using Data.Context;
using Data.Entities;
using Data.interfaces;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// 1. Inyectar EF Core In-Memory usando tu contexto real
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EcommerceDb"));

// 2. Inyectar tu Repositorio (Para que el controlador lo entienda)
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var app = builder.Build();

// 2. Poblar la base de datos al arrancar (Seeding)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Si no hay usuarios, creamos uno por defecto para iniciar sesión
    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new Usuario
        {
            Id = 1,
            Nombre = "Eduardo Rojas",
            Correo = "cliente@tienda.com",
            Password = "12345678"
        });
        context.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
