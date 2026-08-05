using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;

namespace Tests.WebTests;

// Cubre Item 4 y Item 9 del documento de historias de usuario:
// "Administración y estados de Pedidos".
public class AdminPedidosControllerTests
{
    private static List<Pedido> PedidosDeEjemplo() => new()
    {
        new Pedido { Id = 1, Estado = EstadoPedido.Pendiente, Total = 100 },
        new Pedido { Id = 2, Estado = EstadoPedido.Pagado, Total = 200 },
        new Pedido { Id = 3, Estado = EstadoPedido.Enviado, Total = 300, NumeroGuia = "GUIA-001" },
        new Pedido { Id = 4, Estado = EstadoPedido.Cancelado, Total = 50 },
    };

    [Fact]
    public void Index_SinFiltro_DeberiaRetornarTodosLosPedidos()
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        repositorioMock.Setup(r => r.ObtenerTodos()).Returns(PedidosDeEjemplo());
        var controller = new AdminPedidosController(repositorioMock.Object);

        var resultado = controller.Index(null) as OkObjectResult;
        var modelo = resultado?.Value as IReadOnlyList<Pedido>;

        Assert.Equal(4, modelo!.Count);
    }

    [Theory]
    [InlineData(EstadoPedido.Pendiente)]
    [InlineData(EstadoPedido.Pagado)]
    [InlineData(EstadoPedido.Enviado)]
    [InlineData(EstadoPedido.Cancelado)]
    public void Index_ConFiltroDeEstado_DeberiaRetornarSoloEsePedidos(EstadoPedido estado)
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        var esperado = PedidosDeEjemplo().Where(p => p.Estado == estado).ToList();
        repositorioMock.Setup(r => r.ObtenerPorEstado(estado)).Returns(esperado);
        var controller = new AdminPedidosController(repositorioMock.Object);

        var resultado = controller.Index(estado) as OkObjectResult;
        var modelo = resultado?.Value as IReadOnlyList<Pedido>;

        Assert.All(modelo!, p => Assert.Equal(estado, p.Estado));
        repositorioMock.Verify(r => r.ObtenerPorEstado(estado), Times.Once);
    }

    [Fact]
    public void ActualizarEstado_DeberiaCambiarElEstadoManualmente()
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        repositorioMock.Setup(r => r.ObtenerPorId(1)).Returns(PedidosDeEjemplo()[0]);
        var controller = new AdminPedidosController(repositorioMock.Object);
        var request = new ActualizarEstadoPedidoViewModel { PedidoId = 1, NuevoEstado = EstadoPedido.Pagado };

        var resultado = controller.ActualizarEstado(request);

        Assert.IsType<NoContentResult>(resultado);
        repositorioMock.Verify(r => r.ActualizarEstado(1, EstadoPedido.Pagado, null), Times.Once);
    }

    [Fact]
    public void ActualizarEstado_AEnviado_DeberiaAceptarNumeroDeGuiaOpcional()
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        repositorioMock.Setup(r => r.ObtenerPorId(1)).Returns(PedidosDeEjemplo()[0]);
        var controller = new AdminPedidosController(repositorioMock.Object);
        var request = new ActualizarEstadoPedidoViewModel
        {
            PedidoId = 1,
            NuevoEstado = EstadoPedido.Enviado,
            NumeroGuia = "GUIA-999"
        };

        controller.ActualizarEstado(request);

        repositorioMock.Verify(r => r.ActualizarEstado(1, EstadoPedido.Enviado, "GUIA-999"), Times.Once);
    }

    [Fact]
    public void ActualizarEstado_AEnviado_SinNumeroDeGuia_DeberiaSerValido()
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        repositorioMock.Setup(r => r.ObtenerPorId(1)).Returns(PedidosDeEjemplo()[0]);
        var controller = new AdminPedidosController(repositorioMock.Object);
        var request = new ActualizarEstadoPedidoViewModel { PedidoId = 1, NuevoEstado = EstadoPedido.Enviado };

        var resultado = controller.ActualizarEstado(request);

        Assert.IsType<NoContentResult>(resultado);
        repositorioMock.Verify(r => r.ActualizarEstado(1, EstadoPedido.Enviado, null), Times.Once);
    }

    [Fact]
    public void ActualizarEstado_ConNumeroDeGuiaEnOtroEstado_DeberiaIgnorarlo()
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        repositorioMock.Setup(r => r.ObtenerPorId(1)).Returns(PedidosDeEjemplo()[0]);
        var controller = new AdminPedidosController(repositorioMock.Object);
        var request = new ActualizarEstadoPedidoViewModel
        {
            PedidoId = 1,
            NuevoEstado = EstadoPedido.Cancelado,
            NumeroGuia = "NO-DEBERIA-USARSE"
        };

        controller.ActualizarEstado(request);

        repositorioMock.Verify(r => r.ActualizarEstado(1, EstadoPedido.Cancelado, null), Times.Once);
    }

    [Fact]
    public void ActualizarEstado_ConPedidoInexistente_DeberiaRetornarNotFound()
    {
        var repositorioMock = new Mock<IPedidoRepository>();
        repositorioMock.Setup(r => r.ObtenerPorId(It.IsAny<int>())).Returns((Pedido?)null);
        var controller = new AdminPedidosController(repositorioMock.Object);
        var request = new ActualizarEstadoPedidoViewModel { PedidoId = 999, NuevoEstado = EstadoPedido.Pagado };

        var resultado = controller.ActualizarEstado(request);

        Assert.IsType<NotFoundResult>(resultado);
        repositorioMock.Verify(r => r.ActualizarEstado(It.IsAny<int>(), It.IsAny<EstadoPedido>(), It.IsAny<string?>()), Times.Never);
    }
}
