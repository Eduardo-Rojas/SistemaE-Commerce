using Data.Entities;

namespace Web.Models;

// Item 4/9: Administración y estados de pedidos.
public class ActualizarEstadoPedidoViewModel
{
    public int PedidoId { get; set; }
    public EstadoPedido NuevoEstado { get; set; }

    // Opcional: solo se usa cuando NuevoEstado == Enviado.
    public string? NumeroGuia { get; set; }
}
