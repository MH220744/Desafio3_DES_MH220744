using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BolsaTrabajo.BLL.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsuarioService(
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<UsuarioConRolesDTO>>
        ObtenerTodosAsync()
    {
        var usuarios =
            await _userManager.Users.ToListAsync();

        var resultado =
            new List<UsuarioConRolesDTO>();

        foreach (var usuario in usuarios)
        {
            var roles =
                await _userManager.GetRolesAsync(usuario);

            resultado.Add(new UsuarioConRolesDTO
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email ?? string.Empty,
                Roles = roles.ToList()
            });
        }

        return resultado;
    }

    public async Task<UsuarioConRolesDTO?>
        ObtenerPorIdAsync(string idUsuario)
    {
        var usuario =
            await _userManager.FindByIdAsync(idUsuario);

        if (usuario == null)
        {
            return null;
        }

        var roles =
            await _userManager.GetRolesAsync(usuario);

        return new UsuarioConRolesDTO
        {
            Id = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email = usuario.Email ?? string.Empty,
            Roles = roles.ToList()
        };
    }

    public async Task<bool> CambiarRolAsync(
        string idUsuario,
        string nuevoRol)
    {
        var usuario =
            await _userManager.FindByIdAsync(idUsuario);

        if (usuario == null)
        {
            return false;
        }

        if (!await _roleManager.RoleExistsAsync(nuevoRol))
        {
            throw new ArgumentException(
                "El rol indicado no existe.");
        }

        var rolesActuales =
            await _userManager.GetRolesAsync(usuario);

        if (rolesActuales.Any())
        {
            await _userManager.RemoveFromRolesAsync(
                usuario,
                rolesActuales);
        }

        var resultado =
            await _userManager.AddToRoleAsync(
                usuario,
                nuevoRol);

        if (!resultado.Succeeded)
        {
            throw new InvalidOperationException(
                "No fue posible cambiar el rol del usuario.");
        }

        return true;
    }
}