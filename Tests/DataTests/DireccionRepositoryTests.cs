using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    // Cubre DireccionRepository, la implementacion de IDireccionRepository
    // usada por el registro de direccion de entrega.
    public class DireccionRepositoryTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        private static Direccion DireccionValida(int usuarioId = 1) => new()
        {
            UsuarioId = usuarioId,
            Calle = "Av. Independencia",
            Numero = "55",
            Ciudad = "Santo Domingo",
            CodigoPostal = "10101"
        };

        [Fact]
        public void Guardar_PersisteLaDireccionYLeAsignaId()
        {
            using var context = GetContext();
            var repositorio = new DireccionRepository(context);

            var guardada = repositorio.Guardar(DireccionValida());

            Assert.True(guardada.Id > 0);
            Assert.Single(repositorio.ObtenerPorUsuario(1));
        }

        [Fact]
        public void Guardar_SinReferencias_PersisteIgual()
        {
            using var context = GetContext();
            var repositorio = new DireccionRepository(context);

            var guardada = repositorio.Guardar(DireccionValida());

            Assert.Null(guardada.Referencias);
        }

        [Fact]
        public void ObtenerPorUsuario_FiltraPorElUsuarioIndicado()
        {
            using var context = GetContext();
            var repositorio = new DireccionRepository(context);
            repositorio.Guardar(DireccionValida(usuarioId: 1));
            repositorio.Guardar(DireccionValida(usuarioId: 1));
            repositorio.Guardar(DireccionValida(usuarioId: 2));

            var resultado = repositorio.ObtenerPorUsuario(1);

            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, d => Assert.Equal(1, d.UsuarioId));
        }

        [Fact]
        public void ObtenerPorUsuario_SinDirecciones_RetornaVacio()
        {
            using var context = GetContext();
            var repositorio = new DireccionRepository(context);

            Assert.Empty(repositorio.ObtenerPorUsuario(99));
        }
    }
}
