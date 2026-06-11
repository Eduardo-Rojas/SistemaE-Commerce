using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Web.Models;

namespace Web.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AccesoController(IUsuarioRepositorio usuarioRepositorio)
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
            // Criterio: campos vacíos no pasan validación del modelo
            if (!ModelState.IsValid)
                return View(model);

            var passwordHash = HashPassword(model.Contrasena);
            var usuario = await _usuarioRepositorio.ObtenerPorCredencialesAsync(model.Correo, passwordHash);

            if (usuario == null)
            {
                // Criterio: credenciales inválidas → mostrar error
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(model);
            }

            // Criterio: credenciales válidas → redirigir a página principal
            return RedirectToAction("Index", "Home");
        }

        // ------------------------------------------------------------------
        // INTEGRACIÓN: Si el proyecto ya tiene un helper de hashing, usar ese
        // y eliminar este método para no duplicar lógica.
        // ------------------------------------------------------------------
        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}