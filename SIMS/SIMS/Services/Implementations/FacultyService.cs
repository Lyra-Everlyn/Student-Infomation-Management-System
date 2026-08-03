using SIMS.Models.Entities;
using SIMS.Models.ViewModels;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IRepository<Assignment> _assignmentRepo;
        private readonly IRepository<Grade> _gradeRepo;

        public FacultyService(
            IFacultyRepository facultyRepo,
            ICourseRepository courseRepo,
            IEnrollmentRepository enrollmentRepo,
            IRepository<Assignment> assignmentRepo,
            IRepository<Grade> gradeRepo)
        {
            _facultyRepo = facultyRepo;
            _courseRepo = courseRepo;
            _enrollmentRepo = enrollmentRepo;
            _assignmentRepo = assignmentRepo;
            _gradeRepo = gradeRepo;
        }

        public async Task<FacultyDashboardViewModel?> GetDashboardDataAsync(string email)
        {
            var faculties = await _facultyRepo.GetAllWithDetailsAsync();
            var currentFaculty = faculties.FirstOrDefault(f => f.User?.Email == email);

            if (currentFaculty == null) return null;

            return new FacultyDashboardViewModel
            {
                CurrentFaculty = currentFaculty,
                MyCourses = await _courseRepo.GetCoursesByFacultyAsync(currentFaculty.FacultyId)
            };
        }

        public async Task<ManageCourseViewModel?> GetCourseDetailsAsync(int courseId, string email)
        {
            var faculties = await _facultyRepo.GetAllWithDetailsAsync();
            var currentFaculty = faculties.FirstOrDefault(f => f.User?.Email == email);

            if (currentFaculty == null) return null;

            var course = await _courseRepo.GetByIdAsync(courseId);

            if (course == null || course.FacultyId != currentFaculty.FacultyId) return null;

            var enrollments = await _enrollmentRepo.GetEnrollmentsBySectionAsync(courseId);

            var allAssignments = await _assignmentRepo.GetAllAsync();
            var courseAssignments = allAssignments.Where(a => a.CourseId == courseId).ToList();

            var allGrades = await _gradeRepo.GetAllAsync();
            var courseGrades = allGrades.Where(g => courseAssignments.Any(a => a.AssignmentId == g.AssignmentId)).ToList();

            return new ManageCourseViewModel
            {
                Course = course,
                Enrollments = enrollments,
                Assignments = courseAssignments,
                Grades = courseGrades
            };
        }
    }
}
