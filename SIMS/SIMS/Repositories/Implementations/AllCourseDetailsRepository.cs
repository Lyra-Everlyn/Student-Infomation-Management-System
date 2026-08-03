using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories.Implementations
{
    public class AllCourseDetailsRepository : IAllCourseDetailsRepository
    {
        private readonly SimsDbContext _context;

        public AllCourseDetailsRepository(SimsDbContext context)
        {
            _context = context;
        }

        public async Task<ManageCourseViewModel?> GetAllCourseDetailsAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Faculty)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null) return null;

            var assignments = await _context.Assignments
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

            var assignmentIds = assignments.Select(a => a.AssignmentId).ToList();
            var grades = await _context.Grades
                .Where(g => assignmentIds.Contains(g.AssignmentId))
                .ToListAsync();

            return new ManageCourseViewModel
            {
                Course = course,
                Assignments = assignments,
                Enrollments = enrollments,
                Grades = grades
            };
        }
    }
}
