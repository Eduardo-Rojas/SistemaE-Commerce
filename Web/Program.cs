
using Data.Context;
using Data.Entities;
using Data.interfaces;
using Data.Interfaces;
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

// Modulo de catalogo, busqueda, checkout, pedidos y promociones
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IDireccionRepository, DireccionRepository>();
builder.Services.AddScoped<IPromocionesRepository, PromocionesRepository>();

// 1. Inyectar EF Core In-Memory usando tu contexto real
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EcommerceDb"));

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
            new Producto { Id = 1, Nombre = "Laptop Pro 15", Descripcion = "Alto rendimiento para profesionales", Precio = 45000m, Categoria = "Electrónica", CategoriaId = 1, Stock = 10, ImagenUrl = "https://placehold.co/400x300?text=Laptop+Pro+15" },
            new Producto { Id = 2, Nombre = "Mouse Inalámbrico", Descripcion = "Ergonómico, receptor 2.4GHz", Precio = 850m, Categoria = "Electrónica", CategoriaId = 1, Stock = 50, ImagenUrl = "https://placehold.co/400x300?text=Mouse" },
            new Producto { Id = 3, Nombre = "Teclado Mecánico", Descripcion = "RGB, switches Cherry MX", Precio = 3200m, Categoria = "Electrónica", CategoriaId = 1, Stock = 25, ImagenUrl = "https://placehold.co/400x300?text=Teclado" },
            new Producto { Id = 4, Nombre = "Camiseta Básica", Descripcion = "Algodón 100% premium", Precio = 450m, Categoria = "Ropa", CategoriaId = 2, Stock = 100, ImagenUrl = "https://placehold.co/400x300?text=Camiseta" },
            new Producto { Id = 5, Nombre = "Jeans Slim Fit", Descripcion = "Denim de alta calidad", Precio = 1200m, Categoria = "Ropa", CategoriaId = 2, Stock = 60, ImagenUrl = "https://placehold.co/400x300?text=Jeans" },
            new Producto { Id = 6, Nombre = "Zapatillas Running", Descripcion = "Para alto rendimiento", Precio = 2800m, Categoria = "Deportes", CategoriaId = 3, Stock = 30, ImagenUrl = "https://placehold.co/400x300?text=Zapatillas" },
            new Producto { Id = 7, Nombre = "Pelota de Fútbol", Descripcion = "Balón oficial FIFA", Precio = 1500m, Categoria = "Deportes", CategoriaId = 3, Stock = 20, ImagenUrl = "https://placehold.co/400x300?text=Pelota" },
            new Producto { Id = 8, Nombre = "Audífonos BT", Descripcion = "Cancelación activa de ruido", Precio = 5500m, Categoria = "Electrónica", CategoriaId = 1, Stock = 15, ImagenUrl = "https://placehold.co/400x300?text=Audifonos" }
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

    // Datos del modulo de catalogo, pedidos y promociones.
    // Los Id de categoria coinciden con el CategoriaId sembrado en los productos de arriba.
    if (!context.Categorias.Any())
    {
        context.Categorias.AddRange(
            new Categoria { Id = 1, Nombre = "Electrónica" },
            new Categoria { Id = 2, Nombre = "Ropa" },
            new Categoria { Id = 3, Nombre = "Deportes" }
        );
        context.SaveChanges();
    }

    if (!context.Pedidos.Any())
    {
        context.Pedidos.AddRange(
            new Pedido { Id = 1, UsuarioId = 1, Estado = EstadoPedido.Pendiente, Total = 1500m, Fecha = new DateTime(2026, 7, 1) },
            new Pedido { Id = 2, UsuarioId = 1, Estado = EstadoPedido.Pagado, Total = 45000m, Fecha = new DateTime(2026, 7, 3) },
            new Pedido { Id = 3, UsuarioId = 1, Estado = EstadoPedido.Enviado, Total = 3200m, Fecha = new DateTime(2026, 7, 5), NumeroGuia = "GUIA-001" },
            new Pedido { Id = 4, UsuarioId = 1, Estado = EstadoPedido.Pagado, Total = 850m, Fecha = new DateTime(2026, 7, 8) },
            new Pedido { Id = 5, UsuarioId = 1, Estado = EstadoPedido.Cancelado, Total = 450m, Fecha = new DateTime(2026, 7, 10) },
            new Pedido { Id = 6, UsuarioId = 1, Estado = EstadoPedido.Pagado, Total = 5500m, Fecha = new DateTime(2026, 7, 12) }
        );
        context.SaveChanges();
    }

    if (!context.Direcciones.Any())
    {
        context.Direcciones.AddRange(
            new Direccion { Id = 1, UsuarioId = 1, Calle = "Av. Winston Churchill", Numero = "1099", Ciudad = "Santo Domingo", CodigoPostal = "10148", Referencias = "Torre Acrópolis, piso 12" },
            new Direccion { Id = 2, UsuarioId = 1, Calle = "Calle El Sol", Numero = "45", Ciudad = "Santiago", CodigoPostal = "51000" }
        );
        context.SaveChanges();
    }

    if (!context.Banners.Any())
    {
        context.Banners.Add(new Banner { Id = 1, ImagenUrl = "promo-inicial.jpg", Activo = true });
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

app.MapControllerRoute(    // ← rutas MVC (controlador/accion) para las vistas
    name: "default",
    pattern: "{controller=Producto}/{action=Index}/{id?}");

app.MapControllers();      // ← rutas por atributo ([ApiController]) del modulo de catalogo

app.Run();
