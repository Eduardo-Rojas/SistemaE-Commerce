using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class EnvioController : Controller
    {
        private readonly IZonaEnvioRepositorio _zonaEnvioRepositorio;

        public EnvioController(IZonaEnvioRepositorio zonaEnvioRepositorio)
        {
            _zonaEnvioRepositorio = zonaEnvioRepositorio;
        }

        public IActionResult Index()
        {
            var viewModel = new CalculoEnvioViewModel
            {
                Zonas = _zonaEnvioRepositorio.GetAll()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Calcular(CalculoEnvioViewModel model)
        {
            model.Zonas = _zonaEnvioRepositorio.GetAll();

            if (string.IsNullOrWhiteSpace(model.ZonaSeleccionada))
            {
                model.Mensaje = "Debes seleccionar una zona de envío.";
                return View("Index", model);
            }

            var zona = _zonaEnvioRepositorio.GetByNombre(model.ZonaSeleccionada);

            if (zona == null)
            {
                model.Mensaje = "La zona seleccionada no es válida.";
                return View("Index", model);
            }

            model.CostoEnvio = zona.Costo;
            model.Mensaje = $"Costo de envío para {zona.Nombre}: ${zona.Costo:N2}";

            return View("Index", model);
        }
    }
}