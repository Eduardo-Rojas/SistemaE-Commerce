using Data.Context;
using Data.Entities;
using Data.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.DataTests
{
    // Cubre PromocionesRepository, la implementacion de IPromocionesRepository
    // usada por los banners y el reporte de ventas.
    public class PromocionesRepositoryTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void SubirBanner_PersisteElBanner()
        {
            using var context = GetContext();
            var repositorio = new PromocionesRepository(context);

            var guardado = repositorio.SubirBanner(new Banner { ImagenUrl = "promo-verano.jpg", Activo = true });

            Assert.True(guardado.Id > 0);
            Assert.Single(context.Banners);
            Assert.Equal("promo-verano.jpg", context.Banners.First().ImagenUrl);
        }

        [Fact]
        public void ObtenerVentasPagadas_RetornaSoloLosPedidosPagados()
        {
            using var context = GetContext();
            context.Pedidos.AddRange(
                new Pedido { Id = 1, Estado = EstadoPedido.Pendiente, Total = 100m },
                new Pedido { Id = 2, Estado = EstadoPedido.Pagado, Total = 200m },
                new Pedido { Id = 3, Estado = EstadoPedido.Enviado, Total = 300m },
                new Pedido { Id = 4, Estado = EstadoPedido.Pagado, Total = 400m },
                new Pedido { Id = 5, Estado = EstadoPedido.Cancelado, Total = 500m }
            );
            context.SaveChanges();
            var repositorio = new PromocionesRepository(context);

            var resultado = repositorio.ObtenerVentasPagadas();

            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal(EstadoPedido.Pagado, p.Estado));
        }

        [Fact]
        public void ObtenerVentasPagadas_SinPedidosPagados_RetornaVacio()
        {
            using var context = GetContext();
            context.Pedidos.Add(new Pedido { Id = 1, Estado = EstadoPedido.Cancelado, Total = 100m });
            context.SaveChanges();
            var repositorio = new PromocionesRepository(context);

            Assert.Empty(repositorio.ObtenerVentasPagadas());
        }
    }
}
