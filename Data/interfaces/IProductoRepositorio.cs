using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.interfaces
{
    public interface IProductoRepositorio
    {
        IEnumerable<Producto> GetAll();
        IEnumerable<Producto> GetPorFiltros(string? nombre, string? categoria, decimal? precioMin, decimal? precioMax);
        IEnumerable<string> GetCategorias();
    }
}
