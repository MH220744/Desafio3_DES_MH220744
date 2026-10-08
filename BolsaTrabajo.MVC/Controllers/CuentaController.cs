using BolsaTrabajo.MVC.Models;
using BolsaTrabajo.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BolsaTrabajo.MVC.Controllers;

public class CuentaController : Controller
{
    private readonly ApiClientService _apiClient;

    public CuentaController(
        ApiClientService apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client =
            _apiClient.CrearCliente(
                autenticado: false);

        var respuesta =
            await client.PostAsJsonAsync(
                "login?useCookies=true&useSessionCookies=true",
                new
                {
                    email = model.Email,
                    password = model.Password
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "Correo o contraseña incorrectos.");

            return View(model);
        }

        // GUARDAR COOKIE DE LA API
        _apiClient.GuardarCookie(
            respuesta);

        // CREAR CLIENTE NUEVO YA AUTENTICADO
        client =
            _apiClient.CrearCliente();

        var usuario =
            await client.GetFromJsonAsync<
                UsuarioActualViewModel>(
                    "api/Auth/usuario-actual");

        if (usuario == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "No fue posible obtener la información del usuario.");

            return View(model);
        }

        // GUARDAR DATOS DEL USUARIO EN SESION
        HttpContext.Session.SetString(
            "NombreUsuario",
            usuario.NombreCompleto);

        var rol =
            usuario.Roles.FirstOrDefault()
            ?? string.Empty;

        HttpContext.Session.SetString(
            "RolUsuario",
            rol);

        TempData["Mensaje"] =
            "Inicio de sesión correcto.";

        // REDIRECCION SEGUN ROL
        if (usuario.Roles.Contains(
            "Administrador"))
        {
            return RedirectToAction(
                "Index",
                "Administrador");
        }

        if (usuario.Roles.Contains(
            "Agente"))
        {
            return RedirectToAction(
                "Index",
                "Agente");
        }

        if (usuario.Roles.Contains(
            "Candidato"))
        {
            return RedirectToAction(
                "Index",
                "Candidato");
        }

        return RedirectToAction(
            "Index",
            "Home");
    }

    [HttpGet]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registro(
        RegistroViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client =
            _apiClient.CrearCliente(
                autenticado: false);

        var respuesta =
            await client.PostAsJsonAsync(
                "api/Auth/registrar-candidato",
                new
                {
                    nombreCompleto =
                        model.NombreCompleto,

                    email =
                        model.Email,

                    password =
                        model.Password
                });

        if (!respuesta.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "No fue posible registrar el usuario.");

            return View(model);
        }

        TempData["Mensaje"] =
            "Registro exitoso. Ahora puede iniciar sesión.";

        return RedirectToAction(
            "Login");
    }

    [HttpGet]
    public async Task<IActionResult> UsuarioActual()
    {
        var client =
            _apiClient.CrearCliente();

        var respuesta =
            await client.GetAsync(
                "api/Auth/usuario-actual");

        if (!respuesta.IsSuccessStatusCode)
        {
            return Content(
                $"No autenticado. Código: {(int)respuesta.StatusCode}");
        }

        var contenido =
            await respuesta.Content.ReadAsStringAsync();

        return Content(
            contenido,
            "application/json");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var client =
            _apiClient.CrearCliente();

        try
        {
            await client.PostAsync(
                "api/Auth/logout",
                null);
        }
        catch
        {
            // Aunque falle la API,
            // eliminamos la sesion local.
        }

        _apiClient.CerrarSesion();

        TempData["Mensaje"] =
            "Sesión cerrada correctamente.";

        return RedirectToAction(
            "Index",
            "Home");
    }
}