using System.Text;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

// Item 5/10: Promociones y Reportes de Ventas.
[ApiController]
[Route("api/admin/[controller]")]
public class AdminPromocionesController : ControllerBase
{
    private readonly IPromocionesRepository _promocionesRepository;

    public AdminPromocionesController(IPromocionesRepository promocionesRepository)
    {
        _promocionesRepository = promocionesRepository;
    }

    // POST /api/admin/AdminPromociones/SubirBanner
    [HttpPost("SubirBanner")]
    public IActionResult SubirBanner(IFormFile? imagen)
    {
        if (imagen is null || imagen.Length == 0)
        {
            return BadRequest("Debe adjuntar una imagen para el banner.");
        }

        var banner = _promocionesRepository.SubirBanner(new Banner
        {
            ImagenUrl = imagen.FileName,
            Activo = true
        });

        return Ok(banner);
    }

    // GET /api/admin/AdminPromociones/ExportarReporteVentas
    [HttpGet("ExportarReporteVentas")]
    public IActionResult ExportarReporteVentas()
    {
        var ventasPagadas = _promocionesRepository.ObtenerVentasPagadas();

        var csv = new StringBuilder();
        csv.AppendLine("Id,Fecha,Total");
        foreach (var pedido in ventasPagadas)
        {
            csv.AppendLine($"{pedido.Id},{pedido.Fecha:yyyy-MM-dd},{pedido.Total}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "reporte-ventas.csv");
    }
}
