var builder = WebApplication.CreateBuilder(args);

// CONTROLADORES Y VISTAS
builder.Services.AddControllersWithViews();

// SESION
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ACCESO AL CONTEXTO HTTP
builder.Services.AddHttpContextAccessor();

// HTTPCLIENT PARA CONSUMIR LA API
builder.Services
    .AddHttpClient(
        "BolsaTrabajoAPI",
        client =>
        {
            client.BaseAddress =
                new Uri("https://localhost:7072/");
        })
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            UseCookies = false
        });

// SERVICIOS MVC
builder.Services.AddScoped<
    BolsaTrabajo.MVC.Services.ApiClientService>();

// CREAR APLICACION
var app = builder.Build();

// MANEJO DE ERRORES
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// PIPELINE HTTP
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// SESION
app.UseSession();

app.UseAuthorization();

// RUTA MVC
app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Home}/{action=Index}/{id?}");

// EJECUTAR APLICACION
app.Run();