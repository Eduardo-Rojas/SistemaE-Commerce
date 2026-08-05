using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

// Item 3/8: Registro de dirección de entrega.
[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly IDireccionRepository _direccionRepository;

    public CheckoutController(IDireccionRepository direccionRepository)
    {
        _direccionRepository = direccionRepository;
    }

    // POST /api/Checkout/GuardarDireccion
    [HttpPost("GuardarDireccion")]
    public IActionResult GuardarDireccion([FromBody] DireccionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var direccion = new Direccion
        {
            UsuarioId = model.UsuarioId,
            Calle = model.Calle,
            Numero = model.Numero,
            Ciudad = model.Ciudad,
            CodigoPostal = model.CodigoPostal,
            Referencias = model.Referencias
        };

        var guardada = _direccionRepository.Guardar(direccion);
        return Ok(guardada);
    }

    // GET /api/Checkout/Direcciones/5
    [HttpGet("Direcciones/{usuarioId:int}")]
    public IActionResult Direcciones(int usuarioId)
    {
        var direcciones = _direccionRepository.ObtenerPorUsuario(usuarioId);
        return Ok(direcciones);
    }
}
