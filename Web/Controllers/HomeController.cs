using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        // Pantalla de verificación: lista lo que se debe probar en producción
        public IActionResult Index()
        {
            return View();
        }

        // Ruta que espera app.UseExceptionHandler("/Home/Error") fuera de Development
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
