using BolsaTrabajo.Common.DTOs;

namespace BolsaTrabajo.BLL.Interfaces;

public interface IUsuarioService
{
    Task<List<UsuarioConRolesDTO>> ObtenerTodosAsync();

    Task<UsuarioConRolesDTO?> ObtenerPorIdAsync(
        string idUsuario);

    Task<bool> CambiarRolAsync(
        string idUsuario,
        string nuevoRol);
}