using Data.Entities;
using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;
using Xunit;

namespace Tests.WebTests
{
    public class CuponControllerTests
    {
        [Fact]
        public void Aplicar_CuponValido_CalculaDescuentoYTotal()
        {
            var mockRepo = new Mock<ICuponRepositorio>();
            mockRepo.Setup(r => r.GetByCodigo("BIENVENIDO10"))
                .Returns(new Cupon { Codigo = "BIENVENIDO10", MontoDescuento = 500m, LimiteUsos = 100, UsosActuales = 0 });
            mockRepo.Setup(r => r.EstaDisponible("BIENVENIDO10")).Returns(true);

            var controller = new CuponController(mockRepo.Object);
            var model = new AplicarCuponViewModel { Subtotal = 2000m, Codigo = "BIENVENIDO10" };

            var resultado = controller.Aplicar(model);

            var view = Assert.IsType<ViewResult>(resultado);
            var vm = Assert.IsType<AplicarCuponViewModel>(view.Model);
            Assert.True(vm.Exito);
            Assert.Equal(500m, vm.MontoDescuento);
            Assert.Equal(1500m, vm.Total);
            mockRepo.Verify(r => r.RegistrarUso("BIENVENIDO10"), Times.Once);
        }

        [Fact]
        public void Aplicar_DescuentoMayorQueSubtotal_TotalNoQuedaNegativo()
        {
            var mockRepo = new Mock<ICuponRepositorio>();
            mockRepo.Setup(r => r.GetByCodigo("BIENVENIDO10"))
                .Returns(new Cupon { Codigo = "BIENVENIDO10", MontoDescuento = 500m, LimiteUsos = 100, UsosActuales = 0 });
            mockRepo.Setup(r => r.EstaDisponible("BIENVENIDO10")).Returns(true);

            var controller = new CuponController(mockRepo.Object);
            var model = new AplicarCuponViewModel { Subtotal = 300m, Codigo = "BIENVENIDO10" };

            var resultado = controller.Aplicar(model);

            var vm = Assert.IsType<AplicarCuponViewModel>(((ViewResult)resultado).Model);
            Assert.Equal(0m, vm.Total);
        }

        [Fact]
        public void Aplicar_CodigoVacio_RetornaError()
        {
            var mockRepo = new Mock<ICuponRepositorio>();
            var controller = new CuponController(mockRepo.Object);
            var model = new AplicarCuponViewModel { Subtotal = 1000m, Codigo = "" };

            var resultado = controller.Aplicar(model);

            var vm = Assert.IsType<AplicarCuponViewModel>(((ViewResult)resultado).Model);
            Assert.False(vm.Exito);
        }

        [Fact]
        public void Aplicar_CuponAgotado_RetornaErrorYNoIncrementaUso()
        {
            var mockRepo = new Mock<ICuponRepositorio>();
            mockRepo.Setup(r => r.GetByCodigo("AGOTADO"))
                .Returns(new Cupon { Codigo = "AGOTADO", MontoDescuento = 300m, LimiteUsos = 1, UsosActuales = 1 });
            mockRepo.Setup(r => r.EstaDisponible("AGOTADO")).Returns(false);

            var controller = new CuponController(mockRepo.Object);
            var model = new AplicarCuponViewModel { Subtotal = 1000m, Codigo = "AGOTADO" };

            var resultado = controller.Aplicar(model);

            var vm = Assert.IsType<AplicarCuponViewModel>(((ViewResult)resultado).Model);
            Assert.False(vm.Exito);
            mockRepo.Verify(r => r.RegistrarUso(It.IsAny<string>()), Times.Never);
        }
    }
}