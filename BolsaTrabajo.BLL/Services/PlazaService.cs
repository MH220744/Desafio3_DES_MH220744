using AutoMapper;
using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.Entities.Enums;
using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.BLL.Services;

public class PlazaService : IPlazaService
{
    private readonly IPlazaRepository _repository;
    private readonly IMapper _mapper;

    public PlazaService(
        IPlazaRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<PlazaDTO>> ObtenerTodasAsync()
    {
        var plazas =
            await _repository.ObtenerTodasAsync();

        return _mapper.Map<List<PlazaDTO>>(plazas);
    }

    public async Task<List<PlazaDTO>> ObtenerAbiertasAsync()
    {
        var plazas =
            await _repository.ObtenerAbiertasAsync();

        return _mapper.Map<List<PlazaDTO>>(plazas);
    }

    public async Task<PlazaDTO?> ObtenerPorIdAsync(int id)
    {
        var plaza =
            await _repository.ObtenerPorIdAsync(id);

        if (plaza == null)
        {
            return null;
        }

        return _mapper.Map<PlazaDTO>(plaza);
    }

    public async Task<List<PlazaDTO>> ObtenerPorAgenteAsync(
        string idAgente)
    {
        var plazas =
            await _repository.ObtenerPorAgenteAsync(idAgente);

        return _mapper.Map<List<PlazaDTO>>(plazas);
    }

    public async Task<PlazaDTO> CrearAsync(
        CrearPlazaDTO dto,
        string idAgente)
    {
        ValidarFechas(
            dto.FechaPublicacion,
            dto.FechaCierre);

        var plaza =
            _mapper.Map<Plaza>(dto);

        plaza.IdAgente = idAgente;
        plaza.Estado = EstadoPlaza.Abierta;

        await _repository.AgregarAsync(plaza);

        return _mapper.Map<PlazaDTO>(plaza);
    }

    public async Task<PlazaDTO?> ActualizarAsync(
        int id,
        CrearPlazaDTO dto,
        string idAgente)
    {
        ValidarFechas(
            dto.FechaPublicacion,
            dto.FechaCierre);

        var plaza =
            await _repository.ObtenerPorIdAsync(id);

        if (plaza == null)
        {
            return null;
        }

        if (plaza.IdAgente != idAgente)
        {
            throw new UnauthorizedAccessException(
                "No puede modificar una plaza publicada por otro agente.");
        }

        plaza.Titulo = dto.Titulo;
        plaza.Descripcion = dto.Descripcion;
        plaza.FechaPublicacion = dto.FechaPublicacion;
        plaza.FechaCierre = dto.FechaCierre;
        plaza.Institucion = dto.Institucion;

        await _repository.ActualizarAsync(plaza);

        return _mapper.Map<PlazaDTO>(plaza);
    }

    public async Task<bool> EliminarAsync(
        int id,
        string idAgente)
    {
        var plaza =
            await _repository.ObtenerPorIdAsync(id);

        if (plaza == null)
        {
            return false;
        }

        if (plaza.IdAgente != idAgente)
        {
            throw new UnauthorizedAccessException(
                "No puede eliminar una plaza publicada por otro agente.");
        }

        await _repository.EliminarAsync(plaza);

        return true;
    }

    public async Task<PlazaDTO?> CambiarEstadoAsync(
    int id,
    EstadoPlaza nuevoEstado,
    string idAgente)
    {
        var plaza =
            await _repository.ObtenerPorIdAsync(id);

        if (plaza == null)
        {
            return null;
        }

        if (plaza.IdAgente != idAgente)
        {
            throw new UnauthorizedAccessException(
                "No puede cambiar el estado de una plaza publicada por otro agente.");
        }

        plaza.Estado = nuevoEstado;

        await _repository.ActualizarAsync(plaza);

        return _mapper.Map<PlazaDTO>(plaza);
    }

    public async Task<int> ContarAsync()
    {
        return await _repository.ContarAsync();
    }

    private static void ValidarFechas(
        DateTime fechaPublicacion,
        DateTime fechaCierre)
    {
        if (fechaCierre <= fechaPublicacion)
        {
            throw new ArgumentException(
                "La fecha de cierre debe ser posterior a la fecha de publicación.");
        }
    }
}