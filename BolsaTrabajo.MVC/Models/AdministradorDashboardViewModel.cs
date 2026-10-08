namespace BolsaTrabajo.MVC.Models;

public class AdministradorDashboardViewModel
{
    public ResumenAdministradorViewModel Resumen { get; set; }
        = new();

    public List<UsuarioConRolesViewModel> Usuarios { get; set; }
        = new();
}