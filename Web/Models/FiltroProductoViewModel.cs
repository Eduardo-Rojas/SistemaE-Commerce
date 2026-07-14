using Data.Entities;

namespace Web.Models
{
    public class FiltroProductoViewModel
    {
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public IEnumerable<Producto> Productos { get; set; } = Enumerable.Empty<Producto>();
        public IEnumerable<string> Categorias { get; set; } = Enumerable.Empty<string>();
    }
}
