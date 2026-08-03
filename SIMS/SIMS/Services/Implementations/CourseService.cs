using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Models.ViewModels;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

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

        public async Task<(bool Success, string Message)> CreateCourseAsync(Course newCourse, int classId, List<int> selectedStudentIds)
        {
            newCourse.CreatedAt = DateTime.Now;
            newCourse.UpdatedAt = DateTime.Now;

            await _courseRepo.AddAsync(newCourse);

            if (classId > 0 && selectedStudentIds != null && selectedStudentIds.Any())
            {
                var studentsToEnroll = await _context.Students
                    .Where(s => s.ClassId == classId && selectedStudentIds.Contains(s.StudentId))
                    .ToListAsync();

                if (studentsToEnroll.Any())
                {
                    var enrollments = studentsToEnroll.Select(student => new Enrollment
                    {
                        CourseId = newCourse.CourseId,
                        StudentId = student.StudentId,
                        EnrollmentDate = DateTime.Now
                    });

                    await _context.Enrollments.AddRangeAsync(enrollments);
                    await _context.SaveChangesAsync();
                }

                return (true, $"The course '{newCourse.CourseName}' has been created. {studentsToEnroll.Count} selected students have been successfully assigned!");
            }

            return (true, $"The course '{newCourse.CourseName}' has been created.");
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


        public async Task<IEnumerable<StudentBasicViewModel>> GetStudentsByClassIdAsync(int classId)
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => s.ClassId == classId)
                .Select(s => new StudentBasicViewModel
                {
                    StudentId = s.StudentId,
                    StudentCode = s.StudentCode,
                    FullName = s.User != null ? s.User.FullName : "Unknown"
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> AddStudentToCourseAsync(int courseId, string studentCode)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentCode == studentCode);
            if (student == null)
                return (false, $"Student with code '{studentCode}' not found.");

            var exists = await _context.Enrollments.AnyAsync(e => e.CourseId == courseId && e.StudentId == student.StudentId);
            if (exists)
                return (false, "This student is already enrolled in the course.");

            var existingClassId = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student.ClassId)
                .FirstOrDefaultAsync();

            if (existingClassId != null && existingClassId != 0 && student.ClassId != existingClassId)
            {
                return (false, $"Cannot add student. They belong to a different class than the students currently enrolled in this course.");
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = student.StudentId,
                EnrollmentDate = DateTime.Now
            };

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            return (true, "Student successfully added to the course.");
        }

        public async Task<(bool Success, string Message)> RemoveStudentFromCourseAsync(int courseId, int studentId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (enrollment == null)
                return (false, "Enrollment record not found.");

            var gradesToRemove = await _context.Grades
                .Include(g => g.Assignment)
                .Where(g => g.StudentId == studentId && g.Assignment.CourseId == courseId)
                .ToListAsync();

            if (gradesToRemove.Any())
            {
                _context.Grades.RemoveRange(gradesToRemove);
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return (true, "Student removed from the course successfully.");
        }
    }
}