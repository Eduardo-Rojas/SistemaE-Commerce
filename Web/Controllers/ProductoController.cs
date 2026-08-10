using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    
        public class ProductoController : Controller
        {
            private readonly IProductoRepositorio _productoRepositorio;

            public ProductoController(IProductoRepositorio productoRepositorio)
            {
                _productoRepositorio = productoRepositorio;
            }

            public IActionResult Index(
                string? nombre,
                string? categoria,
                decimal? precioMin,
                decimal? precioMax)
            {
                var productos = _productoRepositorio.GetPorFiltros(nombre, categoria, precioMin, precioMax);
                var categorias = _productoRepositorio.GetCategorias();

                var viewModel = new FiltroProductoViewModel
                {
                    Nombre = nombre,
                    Categoria = categoria,
                    PrecioMin = precioMin,
                    PrecioMax = precioMax,
                    Productos = productos,
                    Categorias = categorias
                };

                return View(viewModel);
            }
        }
    }

