namespace BolsaTrabajo.MVC.Models;

public class UsuarioActualViewModel
{
    public string Id { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
}