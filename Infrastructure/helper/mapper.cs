using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using Application.Dto;
using AutoMapper;
using Infrastructure.Identity;



namespace Application.Mapper;

public class InfrastructureProfile : Profile
{

    public InfrastructureProfile()
    {

        // CreateMap<UserPerfil, ApplicationUser>()
        // .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.authorDto))
        // .ReverseMap();

        CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();

        CreateMap<ApplicationUser, ApplicationUserRolDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}