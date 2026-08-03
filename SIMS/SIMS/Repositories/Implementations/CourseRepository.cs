using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        {
            return await _context.Courses
                .Include(cs => cs.Subject)
                .Include(cs => cs.Faculty)
                    .ThenInclude(f => f.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByFacultyAsync(int facultyId)
        {
            return await _dbSet
                .Include(c => c.Subject)
                .Where(c => c.FacultyId == facultyId)
                .ToListAsync();
        }
    }
}
