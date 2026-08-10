using Data.Entities;
using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Web.Models;
using Xunit;

namespace Tests.WebTests
{
    public class EnvioControllerTests
    {
        [Fact]
        public void Calcular_ZonaValida_RetornaCostoCorrecto()
        {
            var mockRepo = new Mock<IZonaEnvioRepositorio>();
            mockRepo.Setup(r => r.GetByNombre("Santo Domingo"))
                .Returns(new ZonaEnvio { Nombre = "Santo Domingo", Costo = 150m });
            mockRepo.Setup(r => r.GetAll()).Returns(new List<ZonaEnvio>());

            var controller = new EnvioController(mockRepo.Object);
            var model = new CalculoEnvioViewModel { ZonaSeleccionada = "Santo Domingo" };

            var resultado = controller.Calcular(model);

            var vm = Assert.IsType<CalculoEnvioViewModel>(((ViewResult)resultado).Model);
            Assert.Equal(150m, vm.CostoEnvio);
        }

        [Fact]
        public void Calcular_ZonaVacia_NoCalculaCosto()
        {
            var mockRepo = new Mock<IZonaEnvioRepositorio>();
            mockRepo.Setup(r => r.GetAll()).Returns(new List<ZonaEnvio>());

            var controller = new EnvioController(mockRepo.Object);
            var model = new CalculoEnvioViewModel { ZonaSeleccionada = "" };

            var resultado = controller.Calcular(model);

            var vm = Assert.IsType<CalculoEnvioViewModel>(((ViewResult)resultado).Model);
            Assert.Null(vm.CostoEnvio);
        }

        [Fact]
        public void Calcular_ZonaInvalida_NoCalculaCosto()
        {
            var mockRepo = new Mock<IZonaEnvioRepositorio>();
            mockRepo.Setup(r => r.GetByNombre("Marte")).Returns((ZonaEnvio?)null);
            mockRepo.Setup(r => r.GetAll()).Returns(new List<ZonaEnvio>());

            var controller = new EnvioController(mockRepo.Object);
            var model = new CalculoEnvioViewModel { ZonaSeleccionada = "Marte" };

            var resultado = controller.Calcular(model);

            var vm = Assert.IsType<CalculoEnvioViewModel>(((ViewResult)resultado).Model);
            Assert.Null(vm.CostoEnvio);
        }
    }
}