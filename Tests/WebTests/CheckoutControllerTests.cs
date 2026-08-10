using System.ComponentModel.DataAnnotations;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;

namespace Tests.WebTests;

// Cubre Item 3 y Item 8 del documento de historias de usuario:
// "Registro de dirección de entrega".
public class CheckoutControllerTests
{
    private static DireccionViewModel DireccionValida() => new()
    {
        UsuarioId = 1,
        Calle = "Av. Siempre Viva",
        Numero = "742",
        Ciudad = "Santo Domingo",
        CodigoPostal = "10101",
        Referencias = "Casa azul"
    };

    // Valida las DataAnnotations del ViewModel igual que lo haria el ModelState de ASP.NET Core.
    private static List<ValidationResult> Validar(DireccionViewModel model)
    {
        var contexto = new ValidationContext(model);
        var resultados = new List<ValidationResult>();
        Validator.TryValidateObject(model, contexto, resultados, validateAllProperties: true);
        return resultados;
    }

    [Fact]
    public void GuardarDireccion_ConTodosLosCamposValidos_DeberiaGuardarExitosamente()
    {
        var repositorioMock = new Mock<IDireccionRepository>();
        repositorioMock.Setup(r => r.Guardar(It.IsAny<Direccion>())).Returns((Direccion d) => d);
        var controller = new CheckoutController(repositorioMock.Object);

        var resultado = controller.GuardarDireccion(DireccionValida());

        Assert.IsType<OkObjectResult>(resultado);
        repositorioMock.Verify(r => r.Guardar(It.IsAny<Direccion>()), Times.Once);
    }

    [Theory]
    [InlineData(nameof(DireccionViewModel.Calle))]
    [InlineData(nameof(DireccionViewModel.Numero))]
    [InlineData(nameof(DireccionViewModel.Ciudad))]
    [InlineData(nameof(DireccionViewModel.CodigoPostal))]
    public void GuardarDireccion_SinCampoObligatorio_DeberiaSerInvalido(string campoVacio)
    {
        var model = DireccionValida();
        typeof(DireccionViewModel).GetProperty(campoVacio)!.SetValue(model, string.Empty);

        var errores = Validar(model);

        Assert.Contains(errores, e => e.MemberNames.Contains(campoVacio));
    }

    [Fact]
    public void GuardarDireccion_SinReferencias_DeberiaSerValida()
    {
        var model = DireccionValida();
        model.Referencias = null;

        var errores = Validar(model);

        Assert.Empty(errores);
    }

    [Fact]
    public void GuardarDireccion_DeberiaVincularseAlPerfilDelUsuario()
    {
        var repositorioMock = new Mock<IDireccionRepository>();
        Direccion? capturada = null;
        repositorioMock.Setup(r => r.Guardar(It.IsAny<Direccion>()))
            .Callback<Direccion>(d => capturada = d)
            .Returns((Direccion d) => d);
        var controller = new CheckoutController(repositorioMock.Object);

        controller.GuardarDireccion(DireccionValida());

        Assert.NotNull(capturada);
        Assert.Equal(1, capturada!.UsuarioId);
    }

    [Fact]
    public void ObtenerDirecciones_DeberiaPermitirSeleccionarUnaDireccionGuardada()
    {
        var direccionesGuardadas = new List<Direccion> { new() { Id = 5, UsuarioId = 1, Calle = "Calle 1" } };
        var repositorioMock = new Mock<IDireccionRepository>();
        repositorioMock.Setup(r => r.ObtenerPorUsuario(1)).Returns(direccionesGuardadas);
        var controller = new CheckoutController(repositorioMock.Object);

        var resultado = controller.Direcciones(1) as OkObjectResult;
        var modelo = resultado?.Value as IReadOnlyList<Direccion>;

        Assert.Single(modelo!);
        Assert.Equal(5, modelo![0].Id);
    }
}
