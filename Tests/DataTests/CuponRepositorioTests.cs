using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    public class CuponRepositorioTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            context.Cupones.AddRange(
                new Cupon { Id = 1, Codigo = "BIENVENIDO10", MontoDescuento = 500m, LimiteUsos = 100, UsosActuales = 0 },
                new Cupon { Id = 2, Codigo = "AGOTADO", MontoDescuento = 300m, LimiteUsos = 1, UsosActuales = 1 }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetByCodigo_CodigoExistente_RetornaCupon()
        {
            using var context = GetContext();
            var repositorio = new CuponRepositorio(context);

            var resultado = repositorio.GetByCodigo("BIENVENIDO10");

            Assert.NotNull(resultado);
            Assert.Equal(500m, resultado!.MontoDescuento);
        }

        [Fact]
        public void GetByCodigo_CodigoInexistente_RetornaNull()
        {
            using var context = GetContext();
            var repositorio = new CuponRepositorio(context);

            Assert.Null(repositorio.GetByCodigo("NOEXISTE"));
        }

        [Fact]
        public void EstaDisponible_CuponConUsosDisponibles_RetornaTrue()
        {
            using var context = GetContext();
            var repositorio = new CuponRepositorio(context);

            Assert.True(repositorio.EstaDisponible("BIENVENIDO10"));
        }

        [Fact]
        public void EstaDisponible_CuponAgotado_RetornaFalse()
        {
            using var context = GetContext();
            var repositorio = new CuponRepositorio(context);

            Assert.False(repositorio.EstaDisponible("AGOTADO"));
        }

        [Fact]
        public void RegistrarUso_IncrementaUsosActuales()
        {
            using var context = GetContext();
            var repositorio = new CuponRepositorio(context);

            repositorio.RegistrarUso("BIENVENIDO10");

            Assert.Equal(1, repositorio.GetByCodigo("BIENVENIDO10")!.UsosActuales);
        }
    }
}