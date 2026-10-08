using AutoMapper;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.BLL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Plaza, PlazaDTO>();
        CreateMap<CrearPlazaDTO, Plaza>();

        CreateMap<Postulacion, PostulacionDTO>();
        CreateMap<CrearPostulacionDTO, Postulacion>();

        CreateMap<HojaDeVida, HojaDeVidaDTO>();
        CreateMap<CrearHojaDeVidaDTO, HojaDeVida>();

        CreateMap<Usuario, UsuarioDTO>();
    }
}