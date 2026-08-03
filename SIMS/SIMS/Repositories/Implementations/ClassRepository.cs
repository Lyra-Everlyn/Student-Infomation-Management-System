using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Class>> GetAllWithDetailsAsync()
        {
            return await _context.Classes
                .Include(c => c.Department)
                .ToListAsync();
        }
        public async Task<bool> ClassExistsAsync(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) return false;
            return await _dbSet.AnyAsync(c => c.ClassName.ToLower() == className.ToLower());
        }
    }
}