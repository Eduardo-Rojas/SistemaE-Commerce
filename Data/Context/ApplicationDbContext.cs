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


    }
}