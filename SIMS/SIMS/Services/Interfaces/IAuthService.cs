using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User> AuthenticateGoogleUserAsync(string email, string fullName);
    }
}
