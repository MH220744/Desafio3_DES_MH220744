using BolsaTrabajo.MVC.Models;
using BolsaTrabajo.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using BolsaTrabajo.MVC.Filters;

namespace BolsaTrabajo.MVC.Controllers;

[SessionAuthorize("Agente")]
public class AgenteController : Controller
{
    private readonly ApiClientService _apiClient;

    public AgenteController(
        ApiClientService apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        var client =
            _apiClient.CrearCliente();

        var plazas =
            await client.GetFromJsonAsync<
                List<PlazaViewModel>>(
                    "api/Plazas/mis-plazas");

        return View(
            plazas ?? new List<PlazaViewModel>());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        var modelo =
            new PlazaFormViewModel
            {
                FechaPublicacion =
                    DateTime.Now,

                FechaCierre =
                    DateTime.Now.AddDays(7)
            };

        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        PlazaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PostAsJsonAsync(
                "api/Plazas",
                new
                {
                    titulo =
                        model.Titulo,

                    descripcion =
                        model.Descripcion,

                    fechaPublicacion =
                        model.FechaPublicacion,

                    fechaCierre =
                        model.FechaCierre,

                    institucion =
                        model.Institucion
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

        TempData["Mensaje"] =
            "Plaza creada correctamente.";

        return RedirectToAction(
            "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Editar(
        int id)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.GetAsync(
                $"api/Plazas/{id}");

        if (!respuesta.IsSuccessStatusCode)
        {
            TempData["Error"] =
                "No fue posible obtener la plaza.";

            return RedirectToAction(
                "Index");
        }

        var plaza =
            await respuesta.Content
                .ReadFromJsonAsync<
                    PlazaViewModel>();

        if (plaza == null)
        {
            return NotFound();
        }

        var modelo =
            new PlazaFormViewModel
            {
                IdPlaza =
                    plaza.IdPlaza,

                Titulo =
                    plaza.Titulo,

                Descripcion =
                    plaza.Descripcion,

                FechaPublicacion =
                    plaza.FechaPublicacion,

                FechaCierre =
                    plaza.FechaCierre,

                Institucion =
                    plaza.Institucion,

                Estado =
                    plaza.Estado
            };

        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(
        PlazaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PutAsJsonAsync(
                $"api/Plazas/{model.IdPlaza}",
                new
                {
                    titulo =
                        model.Titulo,

                    descripcion =
                        model.Descripcion,

                    fechaPublicacion =
                        model.FechaPublicacion,

                    fechaCierre =
                        model.FechaCierre,

                    institucion =
                        model.Institucion
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

        TempData["Mensaje"] =
            "Plaza actualizada correctamente.";

        return RedirectToAction(
            "Index");
    }

    [HttpPost]
    public async Task<IActionResult> CambiarEstado(
        int id,
        int estado)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PatchAsJsonAsync(
                $"api/Plazas/{id}/estado",
                new
                {
                    estado
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            TempData["Error"] =
                "No fue posible cambiar el estado.";
        }
        else
        {
            TempData["Mensaje"] =
                "Estado actualizado correctamente.";
        }

        return RedirectToAction(
            "Index");
    }

    public async Task<IActionResult> Postulaciones(
        int id)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.GetAsync(
                $"api/Postulaciones/plaza/{id}");

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

        ViewBag.IdPlaza = id;

        return View(
            postulaciones ??
            new List<PostulacionViewModel>());
    }

    [HttpPost]
    public async Task<IActionResult> CambiarEstadoPostulacion(
        int idPostulacion,
        int idPlaza,
        int estado)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.PatchAsJsonAsync(
                $"api/Postulaciones/{idPostulacion}/estado",
                new
                {
                    estado
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            TempData["Error"] =
                "No fue posible actualizar la postulación.";
        }
        else
        {
            TempData["Mensaje"] =
                "Postulación actualizada correctamente.";
        }

        return RedirectToAction(
            "Postulaciones",
            new
            {
                id = idPlaza
            });
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.DeleteAsync(
                $"api/Plazas/{id}");

        if (!respuesta.IsSuccessStatusCode)
        {
            var error =
                await respuesta.Content.ReadAsStringAsync();

            TempData["Error"] =
                $"No fue posible eliminar la plaza. {error}";
        }
        else
        {
            TempData["Mensaje"] =
                "Plaza eliminada correctamente.";
        }

        return RedirectToAction("Index");
    }
}