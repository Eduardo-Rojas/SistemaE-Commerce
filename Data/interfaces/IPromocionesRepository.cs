using Data.Entities;

namespace Data.Interfaces;

public interface IPromocionesRepository
{
    Banner SubirBanner(Banner banner);
    IReadOnlyList<Pedido> ObtenerVentasPagadas();
}
