namespace WebAPI.Interfaces
{
    public interface IObject2DRepository
    {
        Task<bool> CreateObject2DAsync(Object2D object2D, string environmentId);
        Task<bool> CheckEnvironmentOwnership(string environmentId, string userId);
        Task<List<Object2D>> GetObjectsByEnvironmentIdAsync(string environmentId);
    }
}
