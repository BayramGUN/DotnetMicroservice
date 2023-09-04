using AutoMapper;
using PlatformService.Api.DTOs;
using PlatformService.Api.Models;

namespace PlatformService.Api.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        // Source --> Target
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformUpdateDto, Platform>();
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
    }
}