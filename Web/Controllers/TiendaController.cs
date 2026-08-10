using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    // Pantallas MVC que consumen las APIs del modulo de catalogo (BusquedaController,
    // CheckoutController, AdminPedidosController y AdminPromocionesController).
    // Viven aparte porque esos controladores son [ApiController] y devuelven JSON:
    // aqui solo se sirven las vistas, el trabajo real lo hacen sus endpoints via fetch.
    public class TiendaController : Controller
    {
        public IActionResult Buscar()
        {
            return View();
        }

        public IActionResult Direccion()
        {
            return View();
        }

        public IActionResult Pedidos()
        {
            return View();
        }

        public IActionResult Promociones()
        {
            return View();
        }
    }
}
