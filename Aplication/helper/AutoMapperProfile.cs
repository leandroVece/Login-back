using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using Application.Dto;
using AutoMapper;
using Domain.Entities;



namespace Application.Mapper;

public class ApplicationProfile : Profile
{

    public ApplicationProfile()
    {

        CreateMap<ApplicationUserDto, IUser>()
        .ReverseMap();
    }
}