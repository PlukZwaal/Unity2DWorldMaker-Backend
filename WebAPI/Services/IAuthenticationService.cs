namespace WebAPI.Services
{
    public interface IAuthenticationService
    {
        string GetCurrentAuthenticatedUserId();
    }
}
