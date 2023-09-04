using PlatformService.Api.Data.Repository.Interface;
using PlatformService.Api.Models;

namespace PlatformService.Api.Data.Repository.Implementation;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform is null)
            throw new ArgumentNullException($"{nameof(platform)} argument must not be null!");
        _context.Platforms.Add(platform);
    }

    public IEnumerable<Platform> GetAllPlatforms() => 
        _context.Platforms.ToList();


    public Platform GetPlatformById(int id) => 
        _context.Platforms.FirstOrDefault(p => p.Id == id) ?? null!;

    public bool SaveChanges() => 
        (_context.SaveChanges() >= 0);

    public void UpdatePlatform(Platform platform)
    {
        if(platform is null)
            throw new ArgumentNullException($"{nameof(platform)} argument must not be null!");
        _context.Platforms.Update(platform);
    }
    public bool IsExist(int id)
    {
        return _context.Platforms.Any(p => p.Id == id);
    }
}
