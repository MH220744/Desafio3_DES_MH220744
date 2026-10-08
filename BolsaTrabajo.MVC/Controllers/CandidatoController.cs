using BolsaTrabajo.MVC.Filters;
using BolsaTrabajo.MVC.Models;
using BolsaTrabajo.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BolsaTrabajo.MVC.Controllers;

[SessionAuthorize("Candidato")]
public class CandidatoController : Controller
{
    private readonly ApiClientService _apiClient;
    private readonly IWebHostEnvironment _environment;

    public CandidatoController(
        ApiClientService apiClient,
        IWebHostEnvironment environment)
    {
        _apiClient = apiClient;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var client =
            _apiClient.CrearCliente();

        var plazas =
            await client.GetFromJsonAsync<
                List<PlazaViewModel>>(
                    "api/Reportes/plazas-disponibles");

        return View(
            plazas ?? new List<PlazaViewModel>());
    }

    [HttpPost]
    public async Task<IActionResult> Postularse(
        int idPlaza)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PostAsJsonAsync(
                "api/Postulaciones",
                new
                {
                    idPlaza
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            var error =
                await respuesta.Content
                    .ReadAsStringAsync();

            TempData["Error"] = error;

            return RedirectToAction(
                "Index");
        }

        TempData["Mensaje"] =
            "Postulación realizada correctamente.";

        return RedirectToAction(
            "MisPostulaciones");
    }

    public async Task<IActionResult> MisPostulaciones()
    {
        var client =
            _apiClient.CrearCliente();

        var postulaciones =
            await client.GetFromJsonAsync<
                List<PostulacionViewModel>>(
                    "api/Postulaciones/mis-postulaciones");

        return View(
            postulaciones ??
            new List<PostulacionViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> HojaDeVida()
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.GetAsync(
                "api/HojasDeVida/mi-cv");

        if (respuesta.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return View(
                new HojaDeVidaViewModel());
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            TempData["Error"] =
                "No fue posible obtener la hoja de vida.";

            return RedirectToAction(
                "Index");
        }

        var modelo =
            await respuesta.Content
                .ReadFromJsonAsync<
                    HojaDeVidaViewModel>();

        return View(
            modelo ??
            new HojaDeVidaViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> HojaDeVida(
        HojaDeVidaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // SUBIR PDF
        if (model.ArchivoPDF != null &&
            model.ArchivoPDF.Length > 0)
        {
            var extension =
                Path.GetExtension(
                    model.ArchivoPDF.FileName)
                    .ToLowerInvariant();

            if (extension != ".pdf")
            {
                ModelState.AddModelError(
                    "ArchivoPDF",
                    "Solamente se permiten archivos PDF.");

                return View(model);
            }

            // MAXIMO 5 MB
            if (model.ArchivoPDF.Length >
                5 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    "ArchivoPDF",
                    "El archivo no puede superar los 5 MB.");

                return View(model);
            }

            var webRoot =
     _environment.WebRootPath;

            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot");
            }

            var carpeta =
                Path.Combine(
                    webRoot,
                    "cv");

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(
                    carpeta);
            }

            var nombreArchivo =
                $"{Guid.NewGuid()}.pdf";

            var rutaFisica =
                Path.Combine(
                    carpeta,
                    nombreArchivo);

            using (var stream =
                new FileStream(
                    rutaFisica,
                    FileMode.Create))
            {
                await model.ArchivoPDF
                    .CopyToAsync(stream);
            }

            model.ArchivoCV =
                $"/cv/{nombreArchivo}";
        }

        var client =
            _apiClient.CrearCliente();

        var datosApi = new
        {
            formacionAcademica =
                model.FormacionAcademica,

            experienciaLaboral =
                model.ExperienciaLaboral,

            competencias =
                model.Competencias,

            archivoCV =
                model.ArchivoCV
        };

        HttpResponseMessage respuesta;

        if (model.IdCV == 0)
        {
            respuesta =
                await client.PostAsJsonAsync(
                    "api/HojasDeVida",
                    datosApi);
        }
        else
        {
            respuesta =
                await client.PutAsJsonAsync(
                    "api/HojasDeVida",
                    datosApi);
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            var error =
                await respuesta.Content
                    .ReadAsStringAsync();

            TempData["Error"] =
                $"No fue posible guardar la hoja de vida. {error}";

            return View(model);
        }

        TempData["Mensaje"] =
            "Hoja de vida guardada correctamente.";

        return RedirectToAction(
            "HojaDeVida");
    }

    [HttpGet]
    public async Task<IActionResult> Perfil()
    {
        var client =
            _apiClient.CrearCliente();

        var usuario =
            await client.GetFromJsonAsync<
                UsuarioActualViewModel>(
                    "api/Auth/usuario-actual");

        if (usuario == null)
        {
            return RedirectToAction(
                "Login",
                "Cuenta");
        }

        var modelo =
            new PerfilViewModel
            {
                NombreCompleto =
                    usuario.NombreCompleto,

                Email =
                    usuario.Email
            };

        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Perfil(
    PerfilViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PutAsJsonAsync(
                "api/Auth/perfil",
                new
                {
                    nombreCompleto =
                        model.NombreCompleto,

                    email =
                        model.Email
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            var error =
                await respuesta.Content
                    .ReadAsStringAsync();

            ModelState.AddModelError(
                string.Empty,
                error);

            return View(model);
        }

        HttpContext.Session.SetString(
            "NombreUsuario",
            model.NombreCompleto);

        TempData["Mensaje"] =
            "Perfil actualizado correctamente.";

        return RedirectToAction(
            "Perfil");
    }
}