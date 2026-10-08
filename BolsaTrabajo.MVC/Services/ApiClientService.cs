namespace BolsaTrabajo.MVC.Services;

public class ApiClientService
{
    private readonly IHttpClientFactory
        _httpClientFactory;

    private readonly IHttpContextAccessor
        _httpContextAccessor;

    private const string CookieKey =
        "ApiCookie";

    public ApiClientService(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory =
            httpClientFactory;

        _httpContextAccessor =
            httpContextAccessor;
    }

    public HttpClient CrearCliente(
        bool autenticado = true)
    {
        var client =
            _httpClientFactory.CreateClient(
                "BolsaTrabajoAPI");

        if (autenticado)
        {
            var cookie =
                _httpContextAccessor
                    .HttpContext?
                    .Session
                    .GetString(CookieKey);

            if (!string.IsNullOrEmpty(cookie))
            {
                client.DefaultRequestHeaders
                    .Add(
                        "Cookie",
                        cookie);
            }
        }

        return client;
    }

    public void GuardarCookie(
        HttpResponseMessage respuesta)
    {
        if (!respuesta.Headers.TryGetValues(
                "Set-Cookie",
                out var cookies))
        {
            return;
        }

        var valores =
            cookies
                .Select(cookie =>
                    cookie.Split(';')[0])
                .ToList();

        var cookieFinal =
            string.Join(
                "; ",
                valores);

        _httpContextAccessor
            .HttpContext?
            .Session
            .SetString(
                CookieKey,
                cookieFinal);
    }

    public bool EstaAutenticado()
    {
        var cookie =
            _httpContextAccessor
                .HttpContext?
                .Session
                .GetString(CookieKey);

        return !string.IsNullOrEmpty(cookie);
    }

    public void CerrarSesion()
    {
        _httpContextAccessor
            .HttpContext?
            .Session
            .Clear();
    }
}