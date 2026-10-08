using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BolsaTrabajo.MVC.Filters;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string[] _roles;

    public SessionAuthorizeAttribute(
        params string[] roles)
    {
        _roles = roles;
    }

    public override void OnActionExecuting(
        ActionExecutingContext context)
    {
        var session =
            context.HttpContext.Session;

        var apiCookie =
            session.GetString("ApiCookie");

        var rol =
            session.GetString("RolUsuario");

        // NO HAY SESION
        if (string.IsNullOrEmpty(apiCookie))
        {
            context.Result =
                new RedirectToActionResult(
                    "Login",
                    "Cuenta",
                    null);

            return;
        }

        // VERIFICAR ROL
        if (_roles.Length > 0 &&
            !_roles.Contains(rol))
        {
            context.Result =
                new RedirectToActionResult(
                    "Index",
                    "Home",
                    null);
        }
    }
}