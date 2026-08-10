using Data.Entities;

namespace Data.Interfaces;

public interface IProductoRepository
{
    IReadOnlyList<Producto> ObtenerTodos();
    IReadOnlyList<Producto> ObtenerPorCategoria(int categoriaId);
    IReadOnlyList<Producto> Buscar(string termino);
}
