using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    public class UsuarioRepositorioTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task ObtenerPorCredencialesAsync_CredencialesValidas_RetornaUsuario()
        {
            using var context = GetContext();
            context.Usuarios.Add(new Usuario { Id = 1, Nombre = "Admin", Correo = "admin@itla.edu.do", Password = "1234" });
            await context.SaveChangesAsync();

            var repositorio = new UsuarioRepositorio(context);
            var resultado = await repositorio.ObtenerPorCredencialesAsync("admin@itla.edu.do", "1234");

            Assert.NotNull(resultado);
            Assert.Equal("Admin", resultado!.Nombre);
        }

        [Fact]
        public async Task ObtenerPorCredencialesAsync_PasswordIncorrecto_RetornaNull()
        {
            using var context = GetContext();
            context.Usuarios.Add(new Usuario { Id = 1, Nombre = "Admin", Correo = "admin@itla.edu.do", Password = "1234" });
            await context.SaveChangesAsync();

            var repositorio = new UsuarioRepositorio(context);
            var resultado = await repositorio.ObtenerPorCredencialesAsync("admin@itla.edu.do", "wrong");

            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObtenerPorCredencialesAsync_CorreoInexistente_RetornaNull()
        {
            using var context = GetContext();
            var repositorio = new UsuarioRepositorio(context);

            var resultado = await repositorio.ObtenerPorCredencialesAsync("noexiste@itla.edu.do", "1234");

            Assert.Null(resultado);
        }
    }
}