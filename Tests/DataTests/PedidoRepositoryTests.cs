using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    // Cubre PedidoRepository, la implementacion de IPedidoRepository
    // usada por la administracion de pedidos.
    public class PedidoRepositoryTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            context.Pedidos.AddRange(
                new Pedido { Id = 1, UsuarioId = 1, Estado = EstadoPedido.Pendiente, Total = 100m },
                new Pedido { Id = 2, UsuarioId = 1, Estado = EstadoPedido.Pagado, Total = 200m },
                new Pedido { Id = 3, UsuarioId = 1, Estado = EstadoPedido.Enviado, Total = 300m, NumeroGuia = "GUIA-001" },
                new Pedido { Id = 4, UsuarioId = 1, Estado = EstadoPedido.Pagado, Total = 400m }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void ObtenerTodos_RetornaTodosLosPedidos()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            Assert.Equal(4, repositorio.ObtenerTodos().Count);
        }

        [Fact]
        public void ObtenerPorEstado_RetornaSoloLosDeEseEstado()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            var resultado = repositorio.ObtenerPorEstado(EstadoPedido.Pagado);

            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal(EstadoPedido.Pagado, p.Estado));
        }

        [Fact]
        public void ObtenerPorId_PedidoExistente_RetornaElPedido()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            Assert.NotNull(repositorio.ObtenerPorId(3));
        }

        [Fact]
        public void ObtenerPorId_PedidoInexistente_RetornaNull()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            Assert.Null(repositorio.ObtenerPorId(999));
        }

        [Fact]
        public void ActualizarEstado_CambiaElEstado()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            repositorio.ActualizarEstado(1, EstadoPedido.Pagado, null);

            Assert.Equal(EstadoPedido.Pagado, repositorio.ObtenerPorId(1)!.Estado);
        }

        [Fact]
        public void ActualizarEstado_AEnviado_GuardaElNumeroDeGuia()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            repositorio.ActualizarEstado(1, EstadoPedido.Enviado, "GUIA-999");

            var pedido = repositorio.ObtenerPorId(1)!;
            Assert.Equal(EstadoPedido.Enviado, pedido.Estado);
            Assert.Equal("GUIA-999", pedido.NumeroGuia);
        }

        [Fact]
        public void ActualizarEstado_ConGuiaNula_LimpiaLaGuiaExistente()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            // El pedido 3 venia como Enviado con GUIA-001.
            repositorio.ActualizarEstado(3, EstadoPedido.Cancelado, null);

            var pedido = repositorio.ObtenerPorId(3)!;
            Assert.Equal(EstadoPedido.Cancelado, pedido.Estado);
            Assert.Null(pedido.NumeroGuia);
        }

        [Fact]
        public void ActualizarEstado_PedidoInexistente_NoLanzaExcepcion()
        {
            using var context = GetContext();
            var repositorio = new PedidoRepository(context);

            repositorio.ActualizarEstado(999, EstadoPedido.Pagado, null);

            Assert.Equal(4, repositorio.ObtenerTodos().Count);
        }
    }
}
