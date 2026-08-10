using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.interfaces
{
    public interface ICuponRepositorio
    {
        Cupon? GetByCodigo(string codigo);
        bool EstaDisponible(string codigo);
        void RegistrarUso(string codigo);
    }
}
