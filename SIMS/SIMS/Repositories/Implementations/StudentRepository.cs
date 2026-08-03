using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(SimsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetAllWithDetailsAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .Include(s => s.Department)
                .ToListAsync();
        }
        public async Task<bool> StudentCodeExistsAsync(string studentCode, int excludeStudentId = 0)
        {
            if (string.IsNullOrWhiteSpace(studentCode)) return false;
            return await _dbSet.AnyAsync(s => s.StudentCode.ToLower() == studentCode.ToLower() && s.StudentId != excludeStudentId);
        }
    }
}
