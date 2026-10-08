using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.MVC.Models;

public class HojaDeVidaViewModel
{
    public int IdCV { get; set; }

    public string IdUsuario { get; set; } = string.Empty;

    [Required]
    public string FormacionAcademica { get; set; } = string.Empty;

    [Required]
    public string ExperienciaLaboral { get; set; } = string.Empty;

    [Required]
    public string Competencias { get; set; } = string.Empty;

    public string? ArchivoCV { get; set; }

    public IFormFile? ArchivoPDF { get; set; }
}