using BolsaTrabajo.API.Controllers;
using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace BolsaTrabajo.Tests;

public class PostulacionesControllerTests
{
    [Fact]
    public async Task CrearPostulacion_SinUsuarioAutenticado_RetornaUnauthorized()
    {
        // Arrange
        using var provider =
            Setup.GetServiceProvider();

        using var scope =
            provider.CreateScope();

        var service =
            scope.ServiceProvider
                .GetRequiredService<IPostulacionService>();

        var controller =
            new PostulacionesController(service);

        controller.ControllerContext =
            new ControllerContext
            {
                HttpContext =
                    new DefaultHttpContext
                    {
                        User =
                            new ClaimsPrincipal(
                                new ClaimsIdentity())
                    }
            };

        var dto =
            new CrearPostulacionDTO
            {
                IdPlaza = 1
            };

        // Act
        var resultado =
            await controller.Crear(dto);

        // Assert
        Assert.IsType<UnauthorizedResult>(
            resultado.Result);
    }
}