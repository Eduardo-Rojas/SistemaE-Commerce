using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class CuponController : Controller
    {
        private readonly ICuponRepositorio _cuponRepositorio;

        public CuponController(ICuponRepositorio cuponRepositorio)
        {
            _cuponRepositorio = cuponRepositorio;
        }

        public IActionResult Index()
        {
            return View(new AplicarCuponViewModel());
        }

        [HttpPost]
        public IActionResult Aplicar(AplicarCuponViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            if (string.IsNullOrWhiteSpace(model.Codigo))
            {
                model.Exito = false;
                model.Mensaje = "Debes ingresar un código de cupón.";
                return View("Index", model);
            }

            var cupon = _cuponRepositorio.GetByCodigo(model.Codigo);

            if (cupon == null)
            {
                model.Exito = false;
                model.Mensaje = "El cupón ingresado no existe.";
            }
            else if (!_cuponRepositorio.EstaDisponible(model.Codigo))
            {
                model.Exito = false;
                model.Mensaje = "Este cupón ya alcanzó su límite de usos.";
            }
            else
            {
                var descuento = Math.Min(cupon.MontoDescuento, model.Subtotal);
                var total = model.Subtotal - descuento;

                _cuponRepositorio.RegistrarUso(model.Codigo);

                model.Exito = true;
                model.MontoDescuento = descuento;
                model.Total = total;
                model.Mensaje = $"Cupón aplicado correctamente. Descuento: ${descuento:N2}";
            }

            return View("Index", model);
        }
    }
}