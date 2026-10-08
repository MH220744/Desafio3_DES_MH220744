using BolsaTrabajo.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BolsaTrabajo.DAL.Seed;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

        string[] roles =
        {
            "Administrador",
            "Agente",
            "Candidato"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@bolsatrabajo.com";
        var adminPassword = "Admin123$";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new Usuario
            {
                UserName = adminEmail,
                Email = adminEmail,
                NombreCompleto = "Administrador General",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Administrador");
            }
        }
    }
}