using Data.Entities;

namespace Data.interfaces
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Retorna el usuario si las credenciales son válidas, null si no.
        /// </summary>
        Task<Usuario?> ObtenerPorCredencialesAsync(string correo, string passwordHash);
    }
}