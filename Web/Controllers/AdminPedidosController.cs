using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

// Item 4/9: Administración y estados de pedidos.
[ApiController]
[Route("api/admin/[controller]")]
public class AdminPedidosController : ControllerBase
{
    private readonly IPedidoRepository _pedidoRepository;

    public AdminPedidosController(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    // GET /api/admin/AdminPedidos?estado=Pagado
    [HttpGet]
    public IActionResult Index([FromQuery] EstadoPedido? estado)
    {
        var pedidos = estado.HasValue
            ? _pedidoRepository.ObtenerPorEstado(estado.Value)
            : _pedidoRepository.ObtenerTodos();

        return Ok(pedidos);
    }

    // PUT /api/admin/AdminPedidos/ActualizarEstado
    [HttpPut("ActualizarEstado")]
    public IActionResult ActualizarEstado([FromBody] ActualizarEstadoPedidoViewModel model)
    {
        var pedido = _pedidoRepository.ObtenerPorId(model.PedidoId);
        if (pedido is null)
        {
            return NotFound();
        }

        // El número de guía solo tiene sentido cuando el estado nuevo es "Enviado";
        // en cualquier otro caso se ignora aunque venga en el request.
        var numeroGuia = model.NuevoEstado == EstadoPedido.Enviado ? model.NumeroGuia : null;

        _pedidoRepository.ActualizarEstado(model.PedidoId, model.NuevoEstado, numeroGuia);
        return NoContent();
    }
}
