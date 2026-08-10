using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;

namespace Tests.WebTests;

// Cubre Item 5 y Item 10 del documento de historias de usuario:
// "Promociones y Reportes de Ventas".
public class AdminPromocionesControllerTests
{
    private static IFormFile CrearArchivoFalso(string nombre = "banner.jpg", long tamano = 1024)
    {
        var archivoMock = new Mock<IFormFile>();
        archivoMock.Setup(f => f.FileName).Returns(nombre);
        archivoMock.Setup(f => f.Length).Returns(tamano);
        return archivoMock.Object;
    }

    [Fact]
    public void SubirBanner_ConImagenValida_DeberiaGuardarlaParaElCarrusel()
    {
        var repositorioMock = new Mock<IPromocionesRepository>();
        repositorioMock.Setup(r => r.SubirBanner(It.IsAny<Banner>())).Returns((Banner b) => b);
        var controller = new AdminPromocionesController(repositorioMock.Object);

        var resultado = controller.SubirBanner(CrearArchivoFalso("promo-verano.jpg"));

        Assert.IsType<OkObjectResult>(resultado);
        repositorioMock.Verify(r => r.SubirBanner(It.Is<Banner>(b => b.ImagenUrl == "promo-verano.jpg" && b.Activo)), Times.Once);
    }

    [Fact]
    public void SubirBanner_SinArchivo_DeberiaRetornarError()
    {
        var repositorioMock = new Mock<IPromocionesRepository>();
        var controller = new AdminPromocionesController(repositorioMock.Object);

        var resultado = controller.SubirBanner(null);

        Assert.IsType<BadRequestObjectResult>(resultado);
        repositorioMock.Verify(r => r.SubirBanner(It.IsAny<Banner>()), Times.Never);
    }

    [Fact]
    public void SubirBanner_ConArchivoVacio_DeberiaRetornarError()
    {
        var repositorioMock = new Mock<IPromocionesRepository>();
        var controller = new AdminPromocionesController(repositorioMock.Object);

        var resultado = controller.SubirBanner(CrearArchivoFalso(tamano: 0));

        Assert.IsType<BadRequestObjectResult>(resultado);
    }

    [Fact]
    public void ExportarReporteVentas_DeberiaIncluirSoloTransaccionesPagadas()
    {
        var ventasPagadas = new List<Pedido>
        {
            new() { Id = 1, Estado = EstadoPedido.Pagado, Total = 150, Fecha = new DateTime(2026, 1, 5) },
            new() { Id = 2, Estado = EstadoPedido.Pagado, Total = 300, Fecha = new DateTime(2026, 1, 6) },
        };
        var repositorioMock = new Mock<IPromocionesRepository>();
        repositorioMock.Setup(r => r.ObtenerVentasPagadas()).Returns(ventasPagadas);
        var controller = new AdminPromocionesController(repositorioMock.Object);

        var resultado = controller.ExportarReporteVentas() as FileContentResult;
        var contenido = System.Text.Encoding.UTF8.GetString(resultado!.FileContents);

        Assert.Contains("150", contenido);
        Assert.Contains("300", contenido);
        repositorioMock.Verify(r => r.ObtenerVentasPagadas(), Times.Once);
    }

    [Fact]
    public void ExportarReporteVentas_DeberiaGenerarUnArchivoDescargable()
    {
        var repositorioMock = new Mock<IPromocionesRepository>();
        repositorioMock.Setup(r => r.ObtenerVentasPagadas()).Returns(new List<Pedido>());
        var controller = new AdminPromocionesController(repositorioMock.Object);

        var resultado = controller.ExportarReporteVentas() as FileContentResult;

        Assert.NotNull(resultado);
        Assert.Equal("text/csv", resultado!.ContentType);
        Assert.Equal("reporte-ventas.csv", resultado.FileDownloadName);
    }
}
