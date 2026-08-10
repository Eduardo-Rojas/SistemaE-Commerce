using Data.Context;
using Data.Entities;
using Data.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositorios
{
   
        public class ProductoRepositorio : IProductoRepositorio
        {
            private readonly ApplicationDbContext _context;

            public ProductoRepositorio(ApplicationDbContext context)
            {
                _context = context;
            }

            public IEnumerable<Producto> GetAll()
            {
                return _context.Productos.ToList();
            }

            public IEnumerable<Producto> GetPorFiltros(
                string? nombre,
                string? categoria,
                decimal? precioMin,
                decimal? precioMax)
            {
                var query = _context.Productos.AsQueryable();

                if (!string.IsNullOrWhiteSpace(nombre))
                    query = query.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));

                if (!string.IsNullOrWhiteSpace(categoria))
                    query = query.Where(p => p.Categoria == categoria);

                if (precioMin.HasValue)
                    query = query.Where(p => p.Precio >= precioMin.Value);

                if (precioMax.HasValue)
                    query = query.Where(p => p.Precio <= precioMax.Value);

                return query.ToList();
            }

            public IEnumerable<string> GetCategorias()
            {
                return _context.Productos
                    .Select(p => p.Categoria)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
            }
        }
    }

