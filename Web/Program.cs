
using Data.Context;
using Data.Entities;
using Data.interfaces;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ICuponRepositorio, CuponRepositorio>();
builder.Services.AddScoped<IZonaEnvioRepositorio, ZonaEnvioRepositorio>();

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
    //Poblar la base de datos con productos de ejemplo

    if (!context.Productos.Any())
    {
        context.Productos.AddRange(
            new Producto { Id = 1, Nombre = "Laptop Pro 15", Descripcion = "Alto rendimiento para profesionales", Precio = 45000m, Categoria = "Electrónica", Stock = 10 },
            new Producto { Id = 2, Nombre = "Mouse Inalámbrico", Descripcion = "Ergonómico, receptor 2.4GHz", Precio = 850m, Categoria = "Electrónica", Stock = 50 },
            new Producto { Id = 3, Nombre = "Teclado Mecánico", Descripcion = "RGB, switches Cherry MX", Precio = 3200m, Categoria = "Electrónica", Stock = 25 },
            new Producto { Id = 4, Nombre = "Camiseta Básica", Descripcion = "Algodón 100% premium", Precio = 450m, Categoria = "Ropa", Stock = 100 },
            new Producto { Id = 5, Nombre = "Jeans Slim Fit", Descripcion = "Denim de alta calidad", Precio = 1200m, Categoria = "Ropa", Stock = 60 },
            new Producto { Id = 6, Nombre = "Zapatillas Running", Descripcion = "Para alto rendimiento", Precio = 2800m, Categoria = "Deportes", Stock = 30 },
            new Producto { Id = 7, Nombre = "Pelota de Fútbol", Descripcion = "Balón oficial FIFA", Precio = 1500m, Categoria = "Deportes", Stock = 20 },
            new Producto { Id = 8, Nombre = "Audífonos BT", Descripcion = "Cancelación activa de ruido", Precio = 5500m, Categoria = "Electrónica", Stock = 15 }
        );
        context.SaveChanges();
    }

    if (!context.Cupones.Any())
    {
        context.Cupones.AddRange(
            new Cupon { Id = 1, Codigo = "BIENVENIDO10", MontoDescuento = 500m, LimiteUsos = 100, UsosActuales = 0 },
            new Cupon { Id = 2, Codigo = "VERANO2026", MontoDescuento = 1000m, LimiteUsos = 50, UsosActuales = 0 },
            new Cupon { Id = 3, Codigo = "AGOTADO", MontoDescuento = 300m, LimiteUsos = 1, UsosActuales = 1 } // útil para testear el caso de límite alcanzado
        );
        context.SaveChanges();
    }

    if (!context.ZonasEnvio.Any())
    {
        context.ZonasEnvio.AddRange(
            new ZonaEnvio { Id = 1, Nombre = "Santo Domingo", Costo = 150m },
            new ZonaEnvio { Id = 2, Nombre = "Santiago", Costo = 250m },
            new ZonaEnvio { Id = 3, Nombre = "Punta Cana", Costo = 400m },
            new ZonaEnvio { Id = 4, Nombre = "Interior", Costo = 350m }
        );
        context.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();      // ← para CSS/JS de wwwroot
app.UseRouting();          // ← activa el sistema de rutas MVC
app.UseAuthorization();

app.MapControllerRoute(    // ← esto reemplaza MapControllers()
    name: "default",
    pattern: "{controller=Producto}/{action=Index}/{id?}");

app.Run();
