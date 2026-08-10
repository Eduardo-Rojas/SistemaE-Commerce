using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositorios
{
    // Implementa IPromocionesRepository (namespace Data.Interfaces), usado por banners y reporte de ventas.
    public class PromocionesRepository : IPromocionesRepository
    {
        private readonly ApplicationDbContext _context;

        public PromocionesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Banner SubirBanner(Banner banner)
        {
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return banner;
        }

        public IReadOnlyList<Pedido> ObtenerVentasPagadas()
        {
            return _context.Pedidos
                .Where(p => p.Estado == EstadoPedido.Pagado)
                .OrderBy(p => p.Id)
                .ToList();
        }
    }
}
