using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.Requests;

public class RegistroUsuarioRequest
{
    [Required]
    [MaxLength(150)]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}