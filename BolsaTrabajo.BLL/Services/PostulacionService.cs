using AutoMapper;
using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.Entities.Enums;
using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.BLL.Services;

public class PostulacionService : IPostulacionService
{
    private readonly IPostulacionRepository _postulacionRepository;
    private readonly IPlazaRepository _plazaRepository;
    private readonly IMapper _mapper;

    public PostulacionService(
        IPostulacionRepository postulacionRepository,
        IPlazaRepository plazaRepository,
        IMapper mapper)
    {
        _postulacionRepository = postulacionRepository;
        _plazaRepository = plazaRepository;
        _mapper = mapper;
    }

    public async Task<List<PostulacionDTO>> ObtenerTodasAsync()
    {
        var postulaciones =
            await _postulacionRepository.ObtenerTodasAsync();

        return _mapper.Map<List<PostulacionDTO>>(
            postulaciones);
    }

    public async Task<PostulacionDTO?> ObtenerPorIdAsync(
        int id)
    {
        var postulacion =
            await _postulacionRepository.ObtenerPorIdAsync(id);

        if (postulacion == null)
        {
            return null;
        }

        return _mapper.Map<PostulacionDTO>(
            postulacion);
    }

    public async Task<List<PostulacionDTO>> ObtenerPorCandidatoAsync(
        string idCandidato)
    {
        var postulaciones =
            await _postulacionRepository
                .ObtenerPorCandidatoAsync(idCandidato);

        return _mapper.Map<List<PostulacionDTO>>(
            postulaciones);
    }

    public async Task<List<PostulacionDTO>> ObtenerPorPlazaAsync(
      int idPlaza,
      string? idAgente,
      bool esAdministrador)
    {
        var plaza =
            await _plazaRepository.ObtenerPorIdAsync(idPlaza);

        if (plaza == null)
        {
            throw new ArgumentException(
                "La plaza no existe.");
        }

        if (!esAdministrador &&
            plaza.IdAgente != idAgente)
        {
            throw new UnauthorizedAccessException(
                "No puede consultar postulaciones de una plaza publicada por otro agente.");
        }

        var postulaciones =
            await _postulacionRepository
                .ObtenerPorPlazaAsync(idPlaza);

        return _mapper.Map<List<PostulacionDTO>>(
            postulaciones);
    }

    public async Task<PostulacionDTO> CrearAsync(
        CrearPostulacionDTO dto,
        string idCandidato)
    {
        var plaza =
            await _plazaRepository.ObtenerPorIdAsync(
                dto.IdPlaza);

        if (plaza == null)
        {
            throw new ArgumentException(
                "La plaza seleccionada no existe.");
        }

        if (plaza.Estado != EstadoPlaza.Abierta)
        {
            throw new InvalidOperationException(
                "Solo puede postularse a plazas abiertas.");
        }

        if (DateTime.Now > plaza.FechaCierre)
        {
            throw new InvalidOperationException(
                "La fecha límite de la plaza ya finalizó.");
        }

        var existe =
            await _postulacionRepository
                .ExistePostulacionAsync(
                    dto.IdPlaza,
                    idCandidato);

        if (existe)
        {
            throw new InvalidOperationException(
                "El candidato ya se postuló a esta plaza.");
        }

        var postulacion = new Postulacion
        {
            IdPlaza = dto.IdPlaza,
            IdCandidato = idCandidato,
            FechaPostulacion = DateTime.Now,
            Estado = EstadoPostulacion.EnRevision
        };

        await _postulacionRepository.AgregarAsync(
            postulacion);

        return _mapper.Map<PostulacionDTO>(
            postulacion);
    }

    public async Task<PostulacionDTO?> CambiarEstadoAsync(
      int id,
      EstadoPostulacion nuevoEstado,
      string? idAgente,
      bool esAdministrador)
    {
        var postulacion =
            await _postulacionRepository.ObtenerPorIdAsync(id);

        if (postulacion == null)
        {
            return null;
        }

        var plaza =
            await _plazaRepository.ObtenerPorIdAsync(
                postulacion.IdPlaza);

        if (plaza == null)
        {
            throw new ArgumentException(
                "La plaza asociada no existe.");
        }

        if (!esAdministrador &&
            plaza.IdAgente != idAgente)
        {
            throw new UnauthorizedAccessException(
                "No puede evaluar postulaciones de una plaza publicada por otro agente.");
        }

        postulacion.Estado = nuevoEstado;

        await _postulacionRepository.ActualizarAsync(
            postulacion);

        return _mapper.Map<PostulacionDTO>(
            postulacion);
    }
}