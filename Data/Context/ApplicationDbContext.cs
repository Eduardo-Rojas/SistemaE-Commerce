using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            // tabla de usuarios
            public DbSet<Usuario> Usuarios { get; set; }

            public DbSet<Producto> Productos { get; set; }

            public DbSet<Cupon> Cupones { get; set; }
            public DbSet<ZonaEnvio> ZonasEnvio { get; set; }

            // Modulo de catalogo, pedidos y promociones
            public DbSet<Categoria> Categorias { get; set; }
            public DbSet<Pedido> Pedidos { get; set; }
            public DbSet<Direccion> Direcciones { get; set; }
            public DbSet<Banner> Banners { get; set; }




    }
}