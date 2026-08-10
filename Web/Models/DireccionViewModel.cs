using System.ComponentModel.DataAnnotations;

namespace Web.Models;

// Item 3/8: Registro de dirección de entrega.
public class DireccionViewModel
{
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "La calle es obligatoria")]
    public string Calle { get; set; } = string.Empty;

    [Required(ErrorMessage = "El numero es obligatorio")]
    public string Numero { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ciudad es obligatoria")]
    public string Ciudad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El codigo postal es obligatorio")]
    public string CodigoPostal { get; set; } = string.Empty;

    // Las referencias NO son obligatorias según los criterios de aceptación.
    public string? Referencias { get; set; }
}
