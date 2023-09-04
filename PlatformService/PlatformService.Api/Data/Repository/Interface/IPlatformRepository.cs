using PlatformService.Api.Models;

namespace PlatformService.Api.Data.Repository.Interface;

public interface IPlatformRepository
{
    bool SaveChanges();
    IEnumerable<Platform> GetAllPlatforms();
    Platform GetPlatformById(int id);
    void CreatePlatform(Platform platform);
    void UpdatePlatform(Platform platform);
    bool IsExist(int id);
}