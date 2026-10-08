using AutoMapper;
using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.BLL.Services;

public class HojaDeVidaService : IHojaDeVidaService
{
    private readonly IHojaDeVidaRepository _repository;
    private readonly IMapper _mapper;

    public HojaDeVidaService(
        IHojaDeVidaRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<HojaDeVidaDTO?> ObtenerPorUsuarioAsync(
        string idUsuario)
    {
        var hoja =
            await _repository.ObtenerPorUsuarioAsync(
                idUsuario);

        if (hoja == null)
        {
            return null;
        }

        return _mapper.Map<HojaDeVidaDTO>(
            hoja);
    }

    public async Task<HojaDeVidaDTO> CrearAsync(
        CrearHojaDeVidaDTO dto,
        string idUsuario)
    {
        var existente =
            await _repository.ObtenerPorUsuarioAsync(
                idUsuario);

        if (existente != null)
        {
            throw new InvalidOperationException(
                "El usuario ya posee una hoja de vida.");
        }

        var hoja =
            _mapper.Map<HojaDeVida>(
                dto);

        hoja.IdUsuario =
            idUsuario;

        await _repository.AgregarAsync(
            hoja);

        return _mapper.Map<HojaDeVidaDTO>(
            hoja);
    }

    public async Task<HojaDeVidaDTO?> ActualizarAsync(
        CrearHojaDeVidaDTO dto,
        string idUsuario)
    {
        var hojaDeVida =
            await _repository.ObtenerPorUsuarioAsync(
                idUsuario);

        if (hojaDeVida == null)
        {
            return null;
        }

        hojaDeVida.FormacionAcademica =
            dto.FormacionAcademica;

        hojaDeVida.ExperienciaLaboral =
            dto.ExperienciaLaboral;

        hojaDeVida.Competencias =
            dto.Competencias;

        hojaDeVida.ArchivoCV =
            dto.ArchivoCV;

        await _repository.ActualizarAsync(
            hojaDeVida);

        return _mapper.Map<HojaDeVidaDTO>(
            hojaDeVida);
    }
}