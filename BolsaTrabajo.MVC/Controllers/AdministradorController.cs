using BolsaTrabajo.MVC.Filters;
using BolsaTrabajo.MVC.Models;
using BolsaTrabajo.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BolsaTrabajo.MVC.Controllers;

[SessionAuthorize("Administrador")]
public class AdministradorController : Controller
{
    private readonly ApiClientService _apiClient;

    public AdministradorController(
        ApiClientService apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        var client =
            _apiClient.CrearCliente();

        var modelo =
            new AdministradorDashboardViewModel();

        // RESUMEN GENERAL
        var respuestaResumen =
            await client.GetAsync(
                "api/Reportes/resumen-administrador");

        if (respuestaResumen.IsSuccessStatusCode)
        {
            var resumen =
                await respuestaResumen.Content
                    .ReadFromJsonAsync<
                        ResumenAdministradorViewModel>();

            if (resumen != null)
            {
                modelo.Resumen = resumen;
            }
        }

        // USUARIOS
        var respuestaUsuarios =
            await client.GetAsync(
                "api/Usuarios");

        if (respuestaUsuarios.IsSuccessStatusCode)
        {
            var usuarios =
                await respuestaUsuarios.Content
                    .ReadFromJsonAsync<
                        List<UsuarioConRolesViewModel>>();

            if (usuarios != null)
            {
                modelo.Usuarios = usuarios;
            }
        }

        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> CambiarRol(
        string idUsuario,
        string rol)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PatchAsJsonAsync(
                $"api/Usuarios/{idUsuario}/rol",
                new
                {
                    rol
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            var error =
                await respuesta.Content
                    .ReadAsStringAsync();

            TempData["Error"] =
                $"No fue posible cambiar el rol. {error}";
        }
        else
        {
            TempData["Mensaje"] =
                "Rol actualizado correctamente.";
        }

        return RedirectToAction(
            "Index");
    }

    public async Task<IActionResult> Postulaciones()
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.GetAsync(
                "api/Postulaciones");

        if (!respuesta.IsSuccessStatusCode)
        {
            TempData["Error"] =
                "No fue posible obtener las postulaciones.";

            return RedirectToAction(
                "Index");
        }

        var postulaciones =
            await respuesta.Content
                .ReadFromJsonAsync<
                    List<PostulacionViewModel>>();

        return View(
            postulaciones ??
            new List<PostulacionViewModel>());
    }
}