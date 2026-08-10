using Data.Entities;

namespace Data.Interfaces;

public interface IDireccionRepository
{
    Direccion Guardar(Direccion direccion);
    IReadOnlyList<Direccion> ObtenerPorUsuario(int usuarioId);
}
