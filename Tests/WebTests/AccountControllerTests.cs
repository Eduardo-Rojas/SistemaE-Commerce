using Data.Entities;
using Data.interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaE_Commerce.Web.Controllers;
using Web.Models;
using Xunit;

namespace Tests.WebTests
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Login_CredencialesValidas_RedirigeAHomeIndex()
        {
            var mockRepo = new Mock<IUsuarioRepositorio>();
            mockRepo.Setup(r => r.ObtenerPorCredencialesAsync("admin@itla.edu.do", "1234"))
                .ReturnsAsync(new Usuario { Id = 1, Nombre = "Admin", Correo = "admin@itla.edu.do", Password = "1234" });

            var controller = new AccountController(mockRepo.Object);
            var model = new LoginViewModel { Correo = "admin@itla.edu.do", Password = "1234" };

            var resultado = await controller.Login(model);

            var redirect = Assert.IsType<RedirectToActionResult>(resultado);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }

        [Fact]
        public async Task Login_CredencialesInvalidas_RetornaViewConError()
        {
            var mockRepo = new Mock<IUsuarioRepositorio>();
            mockRepo.Setup(r => r.ObtenerPorCredencialesAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Usuario?)null);

            var controller = new AccountController(mockRepo.Object);
            var model = new LoginViewModel { Correo = "admin@itla.edu.do", Password = "wrong" };

            var resultado = await controller.Login(model);

            Assert.IsType<ViewResult>(resultado);
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Login_ModelStateInvalido_NoConsultaElRepositorio()
        {
            var mockRepo = new Mock<IUsuarioRepositorio>();
            var controller = new AccountController(mockRepo.Object);
            controller.ModelState.AddModelError("Correo", "El correo es obligatorio");

            var model = new LoginViewModel { Correo = "", Password = "1234" };

            var resultado = await controller.Login(model);

            Assert.IsType<ViewResult>(resultado);
            mockRepo.Verify(r => r.ObtenerPorCredencialesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}