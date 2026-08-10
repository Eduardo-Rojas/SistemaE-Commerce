using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    // Cubre ProductoRepository, la implementacion de IProductoRepository
    // usada por el catalogo y la busqueda con autocompletado.
    public class ProductoRepositoryTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            context.Productos.AddRange(
                new Producto { Id = 1, Nombre = "Camiseta Básica", Precio = 450m, CategoriaId = 2 },
                new Producto { Id = 2, Nombre = "Jeans Slim Fit", Precio = 1200m, CategoriaId = 2 },
                new Producto { Id = 3, Nombre = "Laptop Pro 15", Precio = 45000m, CategoriaId = 1 }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void ObtenerTodos_RetornaTodosLosProductos()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            Assert.Equal(3, repositorio.ObtenerTodos().Count);
        }

        [Fact]
        public void ObtenerPorCategoria_RetornaSoloLosDeEsaCategoria()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            var resultado = repositorio.ObtenerPorCategoria(2);

            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal(2, p.CategoriaId));
        }

        [Fact]
        public void ObtenerPorCategoria_SinCoincidencias_RetornaVacio()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            Assert.Empty(repositorio.ObtenerPorCategoria(999));
        }

        [Fact]
        public void Buscar_CoincidenciaParcial_RetornaElProducto()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            var resultado = repositorio.Buscar("cam");

            Assert.Single(resultado);
            Assert.Equal("Camiseta Básica", resultado[0].Nombre);
        }

        [Fact]
        public void Buscar_NoDistingueMayusculas()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            Assert.Single(repositorio.Buscar("LAPTOP"));
        }

        [Fact]
        public void Buscar_SinCoincidencias_RetornaVacio()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            Assert.Empty(repositorio.Buscar("xyz"));
        }

        [Fact]
        public void Buscar_TerminoVacio_RetornaVacio()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepository(context);

            Assert.Empty(repositorio.Buscar("   "));
        }
    }
}
