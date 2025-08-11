using AutoMapper;
using ClinicaAPI.DTO;
using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.DTO.Auth;
using ClinicaAPI.DTO.Pedido;
using ClinicaAPI.DTO.Utente;
using ClinicaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DAL.Helper
{
    public class MappingProfiles : Profile
    {





        public MappingProfiles()
        {
            // Auth Mappings
            CreateMap<RegisterDto, Utilizador>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
            src.Email));
            CreateMap<Utilizador, AuthResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src =>
            src.Id))
            .ForMember(dest => dest.NomeCompleto, opt => opt.MapFrom(src =>
            src.NomeCompleto));
            CreateMap<Utilizador, UserDto>()
            .ForMember(dest => dest.Telemovel, opt => opt.MapFrom(src =>
            src.PhoneNumber));
            // Pedido Mappings
            CreateMap<CreatePedidoAnonimoDto, Pedido>();
            CreateMap<CreatePedidoRegistadoDto, Pedido>();
            CreateMap< Pedido, CreatePedidoRegistadoDto>();
            CreateMap<Pedido, CreatePedidoAnonimoDto>();

            CreateMap<Pedido, PedidoDto>()
            .ForMember(dest => dest.Utilizador, opt => opt.MapFrom(src =>
            src.Utilizador)); // Mapeia o Utilizador para UserDto
                              // PedidoAtoClinico Mappings
            CreateMap<CreatePedidoAtoClinicoDto, PedidoAtoClinico>();
            CreateMap<PedidoAtoClinico, PedidoAtoClinicoDto>();
            // Utente Mappings
            CreateMap<Utilizador, UtenteDto>()
            .ForMember(dest => dest.Telemovel, opt => opt.MapFrom(src =>
            src.PhoneNumber));
            CreateMap<UpdateUtenteDto, Utilizador>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
            src.Telemovel));
            // Apoio Mappings
            CreateMap<AtoClinico, AtoClinicoDto>();
            CreateMap<CreateAtoClinicoDto, AtoClinico>();
            CreateMap<UpdateAtoClinicoDto, AtoClinico>();
            CreateMap<Profissional, ProfissionalDto>();
            CreateMap<CreateProfissionalDto, Profissional>();
            CreateMap<UpdateProfissionalDto, Profissional>();
            CreateMap<SubsistemaSaude, SubsistemaSaudeDto>();
            CreateMap<CreateSubsistemaSaudeDto, SubsistemaSaude>();
            CreateMap<UpdateSubsistemaSaudeDto, SubsistemaSaude>();
        }
    }
}
