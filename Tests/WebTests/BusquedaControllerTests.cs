using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;

namespace Tests.WebTests;

// Cubre Item 2 y Item 7 del documento de historias de usuario:
// "Búsqueda con autocompletado".
public class BusquedaControllerTests
{
    private static List<Producto> ProductosCoincidentes(int cantidad) =>
        Enumerable.Range(1, cantidad)
            .Select(i => new Producto { Id = i, Nombre = $"Producto {i}", Precio = i * 10m, ImagenUrl = $"img{i}.jpg" })
            .ToList();

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("ab")]
    public void Autocompletar_ConMenosDeTresCaracteres_NoDeberiaBuscar(string termino)
    {
        var repositorioMock = new Mock<IProductoRepository>();
        var controller = new BusquedaController(repositorioMock.Object);

        controller.Autocompletar(termino);

        repositorioMock.Verify(r => r.Buscar(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Autocompletar_ConTresCaracteres_DeberiaActivarLaBusqueda()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.Buscar("abc")).Returns(ProductosCoincidentes(2));
        var controller = new BusquedaController(repositorioMock.Object);

        controller.Autocompletar("abc");

        repositorioMock.Verify(r => r.Buscar("abc"), Times.Once);
    }

    [Fact]
    public void Autocompletar_DeberiaLimitarseAMaximoCincoResultados()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.Buscar("cam")).Returns(ProductosCoincidentes(8));
        var controller = new BusquedaController(repositorioMock.Object);

        var resultado = controller.Autocompletar("cam") as OkObjectResult;
        var modelo = resultado?.Value as BusquedaResultadoViewModel;

        Assert.NotNull(modelo);
        Assert.Equal(5, modelo!.Productos.Count);
    }

    [Fact]
    public void Autocompletar_ResultadosDebenIncluirImagenNombreYPrecio()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.Buscar("cam")).Returns(ProductosCoincidentes(1));
        var controller = new BusquedaController(repositorioMock.Object);

        var resultado = controller.Autocompletar("cam") as OkObjectResult;
        var modelo = resultado?.Value as BusquedaResultadoViewModel;
        var producto = modelo!.Productos.Single();

        Assert.False(string.IsNullOrWhiteSpace(producto.ImagenUrl));
        Assert.False(string.IsNullOrWhiteSpace(producto.Nombre));
        Assert.True(producto.Precio > 0);
    }

    [Fact]
    public void Autocompletar_SinCoincidencias_DeberiaMostrarMensajeDeNoEncontrados()
    {
        var repositorioMock = new Mock<IProductoRepository>();
        repositorioMock.Setup(r => r.Buscar("xyz")).Returns(new List<Producto>());
        var controller = new BusquedaController(repositorioMock.Object);

        var resultado = controller.Autocompletar("xyz") as OkObjectResult;
        var modelo = resultado?.Value as BusquedaResultadoViewModel;

        Assert.Empty(modelo!.Productos);
        Assert.Equal("No se encontraron productos", modelo.Mensaje);
    }
}
