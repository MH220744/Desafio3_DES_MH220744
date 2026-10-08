using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.BLL.Mappings;
using BolsaTrabajo.BLL.Services;
using BolsaTrabajo.DAL.Data;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.DAL.Repositories;
using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.BLL.Mappings;
using BolsaTrabajo.BLL.Services;
using BolsaTrabajo.DAL.Data;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BolsaTrabajo.Tests;

public static class Setup
{
    public static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        // LOGGING
        services.AddLogging();

        // BASE DE DATOS EN MEMORIA
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(
                Guid.NewGuid().ToString()));

        // AUTOMAPPER
        services.AddAutoMapper(config =>
        {
            config.AddProfile<MappingProfile>();
        });

        // REPOSITORIOS
        services.AddScoped<
            IPlazaRepository,
            PlazaRepository>();

        services.AddScoped<
            IPostulacionRepository,
            PostulacionRepository>();

        services.AddScoped<
            IHojaDeVidaRepository,
            HojaDeVidaRepository>();

        // SERVICIOS
        services.AddScoped<
            IPlazaService,
            PlazaService>();

        services.AddScoped<
            IPostulacionService,
            PostulacionService>();

        services.AddScoped<
            IHojaDeVidaService,
            HojaDeVidaService>();

        return services.BuildServiceProvider();
    }
}