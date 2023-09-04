using System.Text.Json;
using AutoMapper;
using CommandsService.Api.Data.Repositories.Interfaces;
using CommandsService.Api.DTOs;
using CommandsService.Api.EvenProcessing.Interfaces;
using CommandsService.Api.Models;

namespace CommandsService.Api.EvenProcessing.Implementations;
public class EvenProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public EvenProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch(eventType)
        {
            case EventType.PlatformPublished:
                addPlatform(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine($"--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch(eventType!.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published Event Detected!");
                return EventType.PlatformPublished;
            default:   
                Console.WriteLine("--> Could not determine the event type!");
                return EventType.Undetermined;
        }
    }
    private void addPlatform(string platformPublishedMessage)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);
                if(!repository.IsExternalPlatformExist(platform.ExternalId))
                {
                    repository.CreatePlatform(platform);
                    repository.SaveChanges();
                    Console.WriteLine("--> Platform added successfully!");
                }
                else
                {
                    Console.WriteLine("--> Platform already exist!");

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"--> Could not add Platform to DB: {exception.Message}");
            }
        }
    }
}
enum EventType
{
    PlatformPublished,
    Undetermined
}