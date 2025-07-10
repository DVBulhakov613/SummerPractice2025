using Class_Lib.Backend.Services;

namespace Class_Lib.Backend.Database.Interfaces
{
    public interface IRoleService
    {
        public Task CachePermissionsAsync(User employee);
    }
}