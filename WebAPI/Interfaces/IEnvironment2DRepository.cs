namespace WebAPI.Interfaces
{
    public interface IEnvironment2DRepository
    {
        Task<bool> CreateEnvironment2DAsync(Environment2D environment, string userId);
        Task<IEnumerable<Environment2D>> GetEnvironment2DsByUserIdAsync(string userId);
        Task<bool> DeleteEnvironment2DAsync(string id, string userId);
    }
}
