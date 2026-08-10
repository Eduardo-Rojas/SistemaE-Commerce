using Data.Context;
using Data.Entities;
using Data.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositorios
{
    public class CuponRepositorio : ICuponRepositorio
    {
        private readonly ApplicationDbContext _context;

        public CuponRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public Cupon? GetByCodigo(string codigo)
        {
            return _context.Cupones
                .FirstOrDefault(c => c.Codigo.ToUpper() == codigo.ToUpper());
        }

        public bool EstaDisponible(string codigo)
        {
            var cupon = GetByCodigo(codigo);
            return cupon != null && cupon.UsosActuales < cupon.LimiteUsos;
        }

        public void RegistrarUso(string codigo)
        {
            var cupon = GetByCodigo(codigo);
            if (cupon != null)
            {
                cupon.UsosActuales++;
                _context.SaveChanges();
            }
        }
    }
}
