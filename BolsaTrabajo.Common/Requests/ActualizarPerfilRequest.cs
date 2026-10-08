using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.Requests;

public class ActualizarPerfilRequest
{
    [Required]
    public string NombreCompleto { get; set; }
        = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; }
        = string.Empty;
}