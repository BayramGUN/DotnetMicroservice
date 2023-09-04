using PlatformService.Api.DTOs;

namespace PlatformService.Api.SyncDataServices.Http.Interface;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platformReadDto);
    
}