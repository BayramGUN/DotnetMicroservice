using PlatformService.Api.DTOs;

namespace PlatformService.Api.AsyncDataServices.Interfaces;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}