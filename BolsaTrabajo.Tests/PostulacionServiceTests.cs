using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace BolsaTrabajo.Tests;

public class PostulacionServiceTests
{
    [Fact]
    public async Task Candidato_NoPuedePostularseDosVeces_A_MismaPlaza()
    {
        // Arrange
        using var provider =
            Setup.GetServiceProvider();

        using var scope =
            provider.CreateScope();

        var plazaService =
            scope.ServiceProvider
                .GetRequiredService<IPlazaService>();

        var postulacionService =
            scope.ServiceProvider
                .GetRequiredService<IPostulacionService>();

        var plazaDto = new CrearPlazaDTO
        {
            Titulo = "Analista de Sistemas",
            Descripcion = "Plaza para prueba",
            Institucion = "Ministerio de prueba",
            FechaPublicacion = DateTime.Now,
            FechaCierre = DateTime.Now.AddDays(15)
        };

        var plaza =
            await plazaService.CrearAsync(
                plazaDto,
                "agente-prueba");

        var postulacionDto =
            new CrearPostulacionDTO
            {
                IdPlaza = plaza.IdPlaza
            };

        var idCandidato =
            "candidato-prueba";

        await postulacionService.CrearAsync(
            postulacionDto,
            idCandidato);

        // Act
        var exception =
            await Assert.ThrowsAsync<
                InvalidOperationException>(
                    () =>
                        postulacionService.CrearAsync(
                            postulacionDto,
                            idCandidato));

        // Assert
        Assert.Equal(
            "El candidato ya se postuló a esta plaza.",
            exception.Message);
    }
}