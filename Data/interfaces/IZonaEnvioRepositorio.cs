using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.interfaces
{
    public interface IZonaEnvioRepositorio
    {
        IEnumerable<ZonaEnvio> GetAll();
        ZonaEnvio? GetByNombre(string nombre);
    }
}
