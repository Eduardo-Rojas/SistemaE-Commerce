using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

// Item 1/6: Visualización del catálogo y categorías.
public class CatalogoController : Controller
{
    private readonly IProductoRepository _productoRepository;

    public CatalogoController(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    // GET /Catalogo
    public IActionResult Index()
    {
        var productos = _productoRepository.ObtenerTodos()
            .Select(p => new ProductoCardViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                ImagenUrl = p.ImagenUrl
            })
            .ToList();

        return View(productos);
    }

    // GET /Catalogo/PorCategoria/5
    public IActionResult PorCategoria(int categoriaId)
    {
        var productos = _productoRepository.ObtenerPorCategoria(categoriaId)
            .Select(p => new ProductoCardViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                ImagenUrl = p.ImagenUrl
            })
            .ToList();

        return View("Index", productos);
    }
}
