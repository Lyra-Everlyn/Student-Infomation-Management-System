using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class FacultyRepository : Repository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Faculty>> GetAllWithDetailsAsync()
        {
            return await _context.Faculties
                .Include(f => f.User)
                .Include(f => f.Department)
                .ToListAsync();
        }
    }
}
