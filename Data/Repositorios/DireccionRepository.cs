using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositorios
{
    // Implementa IDireccionRepository (namespace Data.Interfaces), usado por el checkout.
    public class DireccionRepository : IDireccionRepository
    {
        private readonly ApplicationDbContext _context;

        public DireccionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Direccion Guardar(Direccion direccion)
        {
            _context.Direcciones.Add(direccion);
            _context.SaveChanges();
            return direccion;
        }

        public IReadOnlyList<Direccion> ObtenerPorUsuario(int usuarioId)
        {
            return _context.Direcciones
                .Where(d => d.UsuarioId == usuarioId)
                .OrderBy(d => d.Id)
                .ToList();
        }
    }
}
