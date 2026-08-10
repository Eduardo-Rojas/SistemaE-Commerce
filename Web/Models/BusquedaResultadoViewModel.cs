namespace Web.Models;

// Item 2/6: Búsqueda con autocompletado.
public class BusquedaResultadoViewModel
{
    public const int MaximoResultados = 5;
    public const int MinimoCaracteres = 3;
    public const string MensajeSinResultados = "No se encontraron productos";

    public List<ProductoCardViewModel> Productos { get; set; } = new();
    public string? Mensaje { get; set; }
}
