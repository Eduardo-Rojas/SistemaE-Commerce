using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositorios
{
    // Implementa IProductoRepository (namespace Data.Interfaces), usado por el catalogo y la busqueda.
    // No confundir con ProductoRepositorio, que implementa IProductoRepositorio (namespace Data.interfaces).
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IReadOnlyList<Producto> ObtenerTodos()
        {
            return _context.Productos.ToList();
        }

        public IReadOnlyList<Producto> ObtenerPorCategoria(int categoriaId)
        {
            return _context.Productos
                .Where(p => p.CategoriaId == categoriaId)
                .ToList();
        }

        public IReadOnlyList<Producto> Buscar(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                return new List<Producto>();
            }

            return _context.Productos
                .Where(p => p.Nombre.ToLower().Contains(termino.ToLower()))
                .ToList();
        }
    }
}
