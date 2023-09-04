using System.Net;
using AutoMapper;
using CommandsService.Api.DTOs;
using CommandsService.Api.Models;

namespace CommandsService.Api.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source --> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
    }
}