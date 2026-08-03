using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> EmailExistsAsync(string email);
    }
}