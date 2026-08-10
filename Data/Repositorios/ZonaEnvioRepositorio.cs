using Data.Context;
using Data.Entities;
using Data.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositorios
{
    public class ZonaEnvioRepositorio : IZonaEnvioRepositorio
    {
        private readonly ApplicationDbContext _context;

        public ZonaEnvioRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ZonaEnvio> GetAll()
        {
            return _context.ZonasEnvio.OrderBy(z => z.Nombre).ToList();
        }

        public ZonaEnvio? GetByNombre(string nombre)
        {
            return _context.ZonasEnvio
                .FirstOrDefault(z => z.Nombre.ToUpper() == nombre.ToUpper());
        }
    }
}
