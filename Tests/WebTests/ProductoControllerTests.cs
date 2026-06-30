using Data.Entities;
using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;
using Xunit;

namespace Tests.WebTests
{
    public class ProductoControllerTests
    {
        [Fact]
        public void Index_SinFiltros_RetornaTodosLosProductos()
        {
            var mockRepo = new Mock<IProductoRepositorio>();
            mockRepo.Setup(r => r.GetPorFiltros(null, null, null, null))
                .Returns(new List<Producto> { new Producto { Id = 1, Nombre = "Laptop" }, new Producto { Id = 2, Nombre = "Mouse" } });
            mockRepo.Setup(r => r.GetCategorias()).Returns(new List<string> { "Electrónica" });

            var controller = new ProductoController(mockRepo.Object);
            var resultado = controller.Index(null, null, null, null);

            var view = Assert.IsType<ViewResult>(resultado);
            var vm = Assert.IsType<FiltroProductoViewModel>(view.Model);
            Assert.Equal(2, vm.Productos.Count());
        }

        [Fact]
        public void Index_ConFiltroDeNombre_PasaElParametroAlRepositorio()
        {
            var mockRepo = new Mock<IProductoRepositorio>();
            mockRepo.Setup(r => r.GetPorFiltros("laptop", null, null, null))
                .Returns(new List<Producto> { new Producto { Id = 1, Nombre = "Laptop" } });
            mockRepo.Setup(r => r.GetCategorias()).Returns(new List<string>());

            var controller = new ProductoController(mockRepo.Object);
            controller.Index("laptop", null, null, null);

            mockRepo.Verify(r => r.GetPorFiltros("laptop", null, null, null), Times.Once);
        }

        [Fact]
        public void Index_SinResultados_RetornaListaVacia()
        {
            var mockRepo = new Mock<IProductoRepositorio>();
            mockRepo.Setup(r => r.GetPorFiltros("inexistente", null, null, null))
                .Returns(new List<Producto>());
            mockRepo.Setup(r => r.GetCategorias()).Returns(new List<string>());

            var controller = new ProductoController(mockRepo.Object);
            var resultado = controller.Index("inexistente", null, null, null);

            var view = Assert.IsType<ViewResult>(resultado);
            var vm = Assert.IsType<FiltroProductoViewModel>(view.Model);
            Assert.Empty(vm.Productos);
        }
    }
}