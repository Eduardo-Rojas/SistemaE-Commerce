namespace Web.Models;

// Usado tanto por el catalogo (Item 1/6) como por el autocompletado de busqueda (Item 2/7).
public class ProductoCardViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
}
