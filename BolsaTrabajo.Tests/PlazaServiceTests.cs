using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BolsaTrabajo.Tests;

public class PlazaServiceTests
{
    [Fact]
    public async Task CrearPlazaValida_AumentaConteoDeRegistros()
    {
        // Arrange
        using var provider =
            Setup.GetServiceProvider();

        using var scope =
            provider.CreateScope();

        var service =
            scope.ServiceProvider
                .GetRequiredService<IPlazaService>();

        var repository =
            scope.ServiceProvider
                .GetRequiredService<IPlazaRepository>();

        var conteoInicial =
            await repository.ContarAsync();

        var dto = new CrearPlazaDTO
        {
            Titulo = "Desarrollador .NET",
            Descripcion = "Plaza de prueba",
            Institucion = "Institución de prueba",
            FechaPublicacion = DateTime.Now,
            FechaCierre = DateTime.Now.AddDays(10)
        };

        var idAgente = "agente-prueba";

        // Act
        await service.CrearAsync(
            dto,
            idAgente);

        var conteoFinal =
            await repository.ContarAsync();

        // Assert
        Assert.Equal(
            conteoInicial + 1,
            conteoFinal);
    }

    [Fact]
    public async Task CrearPlaza_FechaCierreAnterior_LanzaError()
    {
        // Arrange
        using var provider =
            Setup.GetServiceProvider();

        using var scope =
            provider.CreateScope();

        var service =
            scope.ServiceProvider
                .GetRequiredService<IPlazaService>();

        var dto = new CrearPlazaDTO
        {
            Titulo = "Plaza inválida",
            Descripcion = "Prueba de fechas",
            Institucion = "Institución prueba",

            FechaPublicacion =
                new DateTime(2026, 10, 20),

            FechaCierre =
                new DateTime(2026, 10, 10)
        };

        // Act
        var exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CrearAsync(
                    dto,
                    "agente-prueba"));

        // Assert
        Assert.Equal(
            "La fecha de cierre debe ser posterior a la fecha de publicación.",
            exception.Message);
    }
}