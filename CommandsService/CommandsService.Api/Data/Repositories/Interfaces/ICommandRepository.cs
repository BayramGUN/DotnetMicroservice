using CommandsService.Api.Models;

namespace CommandsService.Api.Data.Repositories.Interfaces;

public interface ICommandRepository
{
    bool SaveChanges();

    IEnumerable<Platform> ReadAllPlatforms();
    void CreatePlatform(Platform platform);
    bool IsPlatformExist(int platformId);
    bool IsExternalPlatformExist(int externalPlatformId);

    IEnumerable<Command> ReadCommandsForPlatform(int platformId);
    void CreateCommand(int platformId, Command command);
    Command GetCommand(int platformId, int commandId);    
    Platform GetPlatform(int platformId);    
}