using Data.Entities;

namespace Web.Models
{
    public class CalculoEnvioViewModel
    {
        public string? ZonaSeleccionada { get; set; }
        public decimal? CostoEnvio { get; set; }
        public IEnumerable<ZonaEnvio> Zonas { get; set; } = Enumerable.Empty<ZonaEnvio>();
        public string? Mensaje { get; set; }
    }
}
