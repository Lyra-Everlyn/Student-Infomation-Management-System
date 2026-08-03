using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(User user, string rawPassword);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}