using Data.Entities;

namespace Data.Interfaces;

public interface IPedidoRepository
{
    IReadOnlyList<Pedido> ObtenerTodos();
    IReadOnlyList<Pedido> ObtenerPorEstado(EstadoPedido estado);
    Pedido? ObtenerPorId(int id);
    void ActualizarEstado(int pedidoId, EstadoPedido nuevoEstado, string? numeroGuia);
}
