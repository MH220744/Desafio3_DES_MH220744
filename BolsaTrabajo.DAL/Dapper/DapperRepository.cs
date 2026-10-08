using BolsaTrabajo.Common.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BolsaTrabajo.DAL.Dapper;

public class DapperRepository : IDapperRepository
{
    private readonly IConfiguration _configuration;

    public DapperRepository(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection CrearConexion()
    {
        var connectionString =
            _configuration.GetConnectionString(
                "DefaultConnection");

        return new SqlConnection(connectionString);
    }

    public async Task<ResumenAdministradorDTO>
        ObtenerResumenAdministradorAsync()
    {
        const string sql = """
            SELECT
                (SELECT COUNT(*) FROM AspNetUsers)
                    AS TotalUsuarios,

                (SELECT COUNT(*) FROM Plazas)
                    AS TotalPlazas,

                (SELECT COUNT(*)
                 FROM Plazas
                 WHERE Estado = 1)
                    AS PlazasAbiertas,

                (SELECT COUNT(*)
                 FROM Plazas
                 WHERE Estado = 3)
                    AS PlazasCerradas,

                (SELECT COUNT(*)
                 FROM Postulaciones)
                    AS TotalPostulaciones,

                (SELECT COUNT(*)
                 FROM Postulaciones
                 WHERE Estado = 1)
                    AS PostulacionesEnRevision,

                (SELECT COUNT(*)
                 FROM Postulaciones
                 WHERE Estado = 2)
                    AS PostulacionesAprobadas,

                (SELECT COUNT(*)
                 FROM Postulaciones
                 WHERE Estado = 3)
                    AS PostulacionesRechazadas;
            """;

        using var connection = CrearConexion();

        var resultado =
            await connection
                .QuerySingleAsync<ResumenAdministradorDTO>(
                    sql);

        return resultado;
    }

    public async Task<List<PlazaDTO>>
        ObtenerPlazasDisponiblesAsync()
    {
        const string sql = """
            SELECT
                IdPlaza,
                Titulo,
                Descripcion,
                FechaPublicacion,
                FechaCierre,
                Institucion,
                Estado,
                IdAgente
            FROM Plazas
            WHERE Estado = 1
              AND FechaCierre >= GETDATE()
            ORDER BY FechaPublicacion DESC;
            """;

        using var connection = CrearConexion();

        var plazas =
            await connection.QueryAsync<PlazaDTO>(sql);

        return plazas.ToList();
    }
}