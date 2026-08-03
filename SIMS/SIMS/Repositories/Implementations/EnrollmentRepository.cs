using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsBySectionAsync(int courseId)
        {
            return await _dbSet
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }
    }
}
