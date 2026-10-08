using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.MVC.Models;

public class RegistroViewModel
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}