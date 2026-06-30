using Data.interfaces; // Tu namespace de interfaces
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace SistemaE_Commerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        // Inyectamos la interfaz, manteniendo las capas separadas
        public AccountController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Llamamos a tu repositorio de forma asíncrona
            var usuarioValido = await _usuarioRepositorio.ObtenerPorCredencialesAsync(model.Correo, model.Password);

            if (usuarioValido != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }
    }
}