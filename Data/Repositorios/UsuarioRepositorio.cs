using Data.Context;
using Data.Entities;
using Data.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorCredencialesAsync(string correo, string password)
        {
            // Se busca usando texto plano, simple y directo
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Password == password);
        }
    }
}