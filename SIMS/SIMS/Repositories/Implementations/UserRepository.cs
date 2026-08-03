using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return await _dbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}