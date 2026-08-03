using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIMS.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly ISubjectRepository _subjectRepo;
        private readonly IClassRepository _classRepo;
        private readonly IFacultyRepository _facultyRepo;
        private readonly SimsDbContext _context;

        public CourseService(
            ICourseRepository courseRepo,
            ISubjectRepository subjectRepo,
            IClassRepository classRepo,
            IFacultyRepository facultyRepo,
            SimsDbContext context)
        {
            _courseRepo = courseRepo;
            _subjectRepo = subjectRepo;
            _classRepo = classRepo;
            _facultyRepo = facultyRepo;
            _context = context;
        }

        public async Task<object> GetDepartmentFilterDataAsync(int departmentId)
        {
            var subjects = await _subjectRepo.GetAllAsync();
            var classes = await _classRepo.GetAllAsync();
            var faculties = await _facultyRepo.GetAllWithDetailsAsync();

            return new
            {
                subjects = subjects.Where(s => s.DepartmentId == departmentId).Select(s => new { id = s.SubjectId, code = s.SubjectCode, name = s.SubjectName }),
                classes = classes.Where(c => c.DepartmentId == departmentId).Select(c => new { id = c.ClassId, name = c.ClassName }),
                faculties = faculties.Where(f => f.DepartmentId == departmentId).Select(f => new { id = f.FacultyId, name = f.User?.FullName })
            };
        }

        public async Task<(bool Success, string Message)> CreateCourseAsync(Course course, int classId)
        {
            string generatedCode = "CRS-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            course.CourseCode = generatedCode;
            course.CreatedAt = DateTime.Now;
            course.UpdatedAt = DateTime.Now;

            await _courseRepo.AddAsync(course);


            if (classId > 0)
            {
                var studentsInClass = await _context.Students
                    .Where(s => s.ClassId == classId)
                    .ToListAsync();

                foreach (var student in studentsInClass)
                {
                    var enrollment = new Enrollment
                    {
                        CourseId = course.CourseId,
                        StudentId = student.StudentId,
                        EnrollmentDate = DateTime.Now
                    };
                    await _context.Enrollments.AddAsync(enrollment);
                }
                await _context.SaveChangesAsync();

                return (true, $"The course '{course.CourseName}' has been created. {studentsInClass.Count} students have been successfully assigned!");
            }

            return (true, $"The course '{course.CourseName}' has been created with code: {generatedCode}");
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var existingCourse = await _courseRepo.GetByIdAsync(course.CourseId);
            if (existingCourse == null) throw new Exception("Course data not found.");

            existingCourse.CourseName = course.CourseName;
            existingCourse.SubjectId = course.SubjectId;
            existingCourse.FacultyId = course.FacultyId;
            existingCourse.UpdatedAt = DateTime.Now;

            await _courseRepo.UpdateAsync(existingCourse);
        }

        public async Task<(bool Success, string Message)> DeleteCourseAsync(int courseId)
        {
            bool hasAssignments = await _context.Assignments.AnyAsync(a => a.CourseId == courseId);
            if (hasAssignments)
            {
                return (false, "Cannot delete this Course because Assignments have been created within it!");
            }

            var enrollmentsToRemove = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

            int studentCount = enrollmentsToRemove.Count;
            if (studentCount > 0)
            {
                _context.Enrollments.RemoveRange(enrollmentsToRemove);
                await _context.SaveChangesAsync();
            }

            await _courseRepo.DeleteAsync(courseId);
            return (true, $"The course has been deleted successfully! (Automatically removed {studentCount} students from the list).");
        }
    }
}