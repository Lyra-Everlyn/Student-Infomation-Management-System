using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Subject>> GetAllWithDetailsAsync()
        {
            return await _context.Subjects
                .Include(s => s.Department)
                .ToListAsync();
        }
        public async Task<bool> SubjectExistsAsync(string subjectCode)
        {
            if (string.IsNullOrWhiteSpace(subjectCode)) return false;
            return await _dbSet.AnyAsync(s => s.SubjectCode.ToLower() == subjectCode.ToLower());
        }
    }
}
