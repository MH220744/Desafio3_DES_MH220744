using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.MVC.Models;

public class PerfilViewModel
{
    [Required]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; }
        = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; }
        = string.Empty;
}