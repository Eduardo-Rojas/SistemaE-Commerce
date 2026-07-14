using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    public class ProductoRepositorioTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            context.Productos.AddRange(
                new Producto { Id = 1, Nombre = "Laptop Pro", Descripcion = "Alto rendimiento", Precio = 45000m, Categoria = "Electrónica", Stock = 10 },
                new Producto { Id = 2, Nombre = "Mouse", Descripcion = "Ergonómico", Precio = 850m, Categoria = "Electrónica", Stock = 50 },
                new Producto { Id = 3, Nombre = "Camiseta", Descripcion = "Algodón", Precio = 450m, Categoria = "Ropa", Stock = 100 }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_RetornaTodosLosProductos()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepositorio(context);

            Assert.Equal(3, repositorio.GetAll().Count());
        }

        [Fact]
        public void GetPorFiltros_FiltraPorNombre()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepositorio(context);

            var resultado = repositorio.GetPorFiltros("laptop", null, null, null);

            Assert.Single(resultado);
            Assert.Equal("Laptop Pro", resultado.First().Nombre);
        }

        [Fact]
        public void GetPorFiltros_FiltraPorCategoria()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepositorio(context);

            var resultado = repositorio.GetPorFiltros(null, "Ropa", null, null);

            Assert.Single(resultado);
            Assert.Equal("Camiseta", resultado.First().Nombre);
        }

        [Fact]
        public void GetPorFiltros_FiltraPorRangoDePrecio()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepositorio(context);

            var resultado = repositorio.GetPorFiltros(null, null, 500m, 1000m);

            Assert.Single(resultado);
            Assert.Equal("Mouse", resultado.First().Nombre);
        }

        [Fact]
        public void GetPorFiltros_SinCoincidencias_RetornaVacio()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepositorio(context);

            Assert.Empty(repositorio.GetPorFiltros("inexistente", null, null, null));
        }

        [Fact]
        public void GetCategorias_RetornaCategoriasUnicas()
        {
            using var context = GetContext();
            var repositorio = new ProductoRepositorio(context);

            var resultado = repositorio.GetCategorias();

            Assert.Equal(2, resultado.Count());
        }
    }
}