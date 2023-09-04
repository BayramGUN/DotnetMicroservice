using CommandsService.Api.Data.Repositories.Interfaces;
using CommandsService.Api.Models;

namespace CommandsService.Api.Data.Repositories.Implementations;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if(command is null)
            throw new ArgumentNullException(nameof(command));
        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform is null)
            throw new ArgumentNullException(nameof(platform));
        _context.Platforms.Add(platform);
    }

    public Command GetCommand(int platformId, int commandId) =>
        _context.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault()!;
    public Platform GetPlatform(int platformId) =>
        _context.Platforms.Where(c => c.Id == platformId).FirstOrDefault()!;

    public bool IsExternalPlatformExist(int externalPlatformId) =>
        _context.Platforms.Any(p => p.ExternalId == externalPlatformId);

    public bool IsPlatformExist(int platformId) =>
        _context.Platforms.Any(p => p.Id == platformId);

    public IEnumerable<Platform> ReadAllPlatforms() =>
        _context.Platforms.ToList();

    public IEnumerable<Command> ReadCommandsForPlatform(int platformId) =>
        _context.Commands.Where(c => c.PlatformId == platformId)
                         .OrderBy(c => c.Platform.Name);
                    

    public bool SaveChanges() =>
        (_context.SaveChanges() >= 0);
}