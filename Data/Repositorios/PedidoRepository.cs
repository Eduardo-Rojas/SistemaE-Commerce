using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositorios
{
    // Implementa IPedidoRepository (namespace Data.Interfaces), usado por la administracion de pedidos.
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IReadOnlyList<Pedido> ObtenerTodos()
        {
            return _context.Pedidos.OrderBy(p => p.Id).ToList();
        }

        public IReadOnlyList<Pedido> ObtenerPorEstado(EstadoPedido estado)
        {
            return _context.Pedidos
                .Where(p => p.Estado == estado)
                .OrderBy(p => p.Id)
                .ToList();
        }

        public Pedido? ObtenerPorId(int id)
        {
            return _context.Pedidos.FirstOrDefault(p => p.Id == id);
        }

        public void ActualizarEstado(int pedidoId, EstadoPedido nuevoEstado, string? numeroGuia)
        {
            var pedido = ObtenerPorId(pedidoId);
            if (pedido == null)
            {
                return;
            }

            // El controlador ya se encarga de mandar null cuando el estado no es Enviado,
            // asi que aqui se asigna tal cual: cambiar de Enviado a otro estado limpia la guia.
            pedido.Estado = nuevoEstado;
            pedido.NumeroGuia = numeroGuia;
            _context.SaveChanges();
        }
    }
}
