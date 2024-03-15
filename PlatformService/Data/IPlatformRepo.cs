using WebApplication1.Models;

namespace WebApplication1.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int platformId);
        void CreatePlatform(Platform platform);
    }
}
