using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

// Item 2/7: Búsqueda con autocompletado.
[ApiController]
[Route("api/[controller]")]
public class BusquedaController : ControllerBase
{
    private readonly IProductoRepository _productoRepository;

    public BusquedaController(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    // GET /api/Busqueda/Autocompletar?termino=abc
    [HttpGet("Autocompletar")]
    public IActionResult Autocompletar([FromQuery] string termino)
    {
        termino ??= string.Empty;

        // No se debe activar la búsqueda antes del tercer carácter.
        if (termino.Length < BusquedaResultadoViewModel.MinimoCaracteres)
        {
            return Ok(new BusquedaResultadoViewModel());
        }

        var coincidencias = _productoRepository.Buscar(termino)
            .Take(BusquedaResultadoViewModel.MaximoResultados)
            .Select(p => new ProductoCardViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                ImagenUrl = p.ImagenUrl
            })
            .ToList();

        var resultado = new BusquedaResultadoViewModel
        {
            Productos = coincidencias,
            Mensaje = coincidencias.Count == 0 ? BusquedaResultadoViewModel.MensajeSinResultados : null
        };

        return Ok(resultado);
    }
}
