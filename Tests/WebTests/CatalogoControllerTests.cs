using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;

namespace Tests.WebTests;

// Cubre Item 1 y Item 6 del documento de historias de usuario:
// "Visualización del catálogo y categorías".
public class CatalogoControllerTests
{
    private static List<Producto> ProductosDeEjemplo() => new()
    {
        new Producto { Id = 1, Nombre = "Camiseta", Precio = 15.99m, ImagenUrl = "camiseta.jpg", CategoriaId = 1 },
        new Producto { Id = 2, Nombre = "Pantalon", Precio = 29.99m, ImagenUrl = "pantalon.jpg", CategoriaId = 2 },
        new Producto { Id = 3, Nombre = "Gorra", Precio = 9.99m, ImagenUrl = "gorra.jpg", CategoriaId = 1 },
    };

    [Fact]
    public void Index_DeberiaMostrarTodosLosProductosEnUnaCuadricula()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.ObtenerTodos()).Returns(ProductosDeEjemplo());
        var controller = new CatalogoController(repositorioMock.Object);

        var resultado = controller.Index() as ViewResult;
        var modelo = resultado?.Model as List<ProductoCardViewModel>;

        Assert.NotNull(modelo);
        Assert.Equal(3, modelo!.Count);
    }

    [Fact]
    public void Index_CadaTarjetaDebeExponerImagenNombreYPrecio()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.ObtenerTodos()).Returns(ProductosDeEjemplo());
        var controller = new CatalogoController(repositorioMock.Object);

        var resultado = controller.Index() as ViewResult;
        var modelo = resultado?.Model as List<ProductoCardViewModel>;

        Assert.All(modelo!, tarjeta =>
        {
            Assert.False(string.IsNullOrWhiteSpace(tarjeta.Nombre));
            Assert.False(string.IsNullOrWhiteSpace(tarjeta.ImagenUrl));
            Assert.True(tarjeta.Precio > 0);
        });
    }

    [Fact]
    public void PorCategoria_DeberiaCargarUnicamenteLosProductosDeEsaCategoria()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        var productosCategoria1 = ProductosDeEjemplo().Where(p => p.CategoriaId == 1).ToList();
        repositorioMock.Setup(r => r.ObtenerPorCategoria(1)).Returns(productosCategoria1);
        var controller = new CatalogoController(repositorioMock.Object);

        var resultado = controller.PorCategoria(1) as ViewResult;
        var modelo = resultado?.Model as List<ProductoCardViewModel>;

        Assert.NotNull(modelo);
        Assert.Equal(2, modelo!.Count);
        Assert.All(modelo, p => Assert.Contains(p.Nombre, new[] { "Camiseta", "Gorra" }));
        repositorioMock.Verify(r => r.ObtenerPorCategoria(1), Times.Once);
    }

    [Fact]
    public void PorCategoria_SinProductosAsociados_DeberiaRetornarListaVacia()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.ObtenerPorCategoria(It.IsAny<int>())).Returns(new List<Producto>());
        var controller = new CatalogoController(repositorioMock.Object);

        var resultado = controller.PorCategoria(999) as ViewResult;
        var modelo = resultado?.Model as List<ProductoCardViewModel>;

        Assert.NotNull(modelo);
        Assert.Empty(modelo!);
    }
}
