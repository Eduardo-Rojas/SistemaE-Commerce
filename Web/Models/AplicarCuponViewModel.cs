using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AplicarCuponViewModel
    {
        [Required(ErrorMessage = "El subtotal es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
        public decimal Subtotal { get; set; }

        public string? Codigo { get; set; }

        public decimal? MontoDescuento { get; set; }
        public decimal? Total { get; set; }
        public string? Mensaje { get; set; }
        public bool Exito { get; set; }
    }
}