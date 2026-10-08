using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.BLL.Mappings;
using BolsaTrabajo.BLL.Services;
using BolsaTrabajo.DAL.Data;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.DAL.Repositories;
using BolsaTrabajo.DAL.Seed;
using BolsaTrabajo.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BolsaTrabajo.DAL.Dapper;

var builder = WebApplication.CreateBuilder(args);

// BASE DE DATOS
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// CONTROLADORES
builder.Services.AddControllers();

// SWAGGER / OPENAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AUTOMAPPER
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// REPOSITORIOS

// PLAZAS
builder.Services.AddScoped<
    IPlazaRepository,
    PlazaRepository>();

// POSTULACIONES
builder.Services.AddScoped<
    IPostulacionRepository,
    PostulacionRepository>();

// HOJAS DE VIDA
builder.Services.AddScoped<
    IHojaDeVidaRepository,
    HojaDeVidaRepository>();

// SERVICIOS

// PLAZAS
builder.Services.AddScoped<
    IPlazaService,
    PlazaService>();

// POSTULACIONES
builder.Services.AddScoped<
    IPostulacionService,
    PostulacionService>();

// DAPPER
builder.Services.AddScoped<
    IDapperRepository,
    DapperRepository>();

// HOJAS DE VIDA
builder.Services.AddScoped<
    IHojaDeVidaService,
    HojaDeVidaService>();

// REPORTES
builder.Services.AddScoped<
    IReporteService,
    ReporteService>();

// USUARIOS
builder.Services.AddScoped<
    IUsuarioService,
    UsuarioService>();

// AUTENTICACIÓN Y AUTORIZACIÓN
builder.Services.AddAuthorization();

builder.Services.AddAuthentication()
    .AddCookie(
        IdentityConstants.ApplicationScheme,
        options =>
        {
            // Evita que una API redirija hacia páginas
            // inexistentes de Login/AccessDenied.
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode =
                    StatusCodes.Status403Forbidden;

                return Task.CompletedTask;
            };
        });

// ASP.NET CORE IDENTITY
builder.Services.AddIdentityCore<Usuario>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

// CREAR APLICACIÓN
var app = builder.Build();

// SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// PIPELINE HTTP
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// CONTROLADORES API
app.MapControllers();

// ENDPOINTS AUTOMÁTICOS IDENTITY

// /register
// /login
// /refresh
// /manage/info
app.MapIdentityApi<Usuario>();

// CREAR ROLES Y ADMIN INICIAL
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await DbInitializer
        .SeedRolesAndAdminAsync(services);
}

// EJECUTAR APLICACIÓN
app.Run();