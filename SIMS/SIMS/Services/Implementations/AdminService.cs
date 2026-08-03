using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdmissionApplicationRepository _admissionRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IClassRepository _classRepo;
        private readonly ISubjectRepository _subjectRepo;
        private readonly IUserRepository _userRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IFacultyRepository _facultyRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IAllCourseDetailsRepository _allCourseDetailsRepo;
        private readonly SimsDbContext _context;

        public AdminService(
            IAdmissionApplicationRepository admissionRepo,
            IDepartmentRepository departmentRepo,
            IClassRepository classRepo,
            ISubjectRepository subjectRepo,
            IUserRepository userRepos,
            IStudentRepository studentRepo,
            IFacultyRepository facultyRepo,
            ICourseRepository courseRepo,
            IAllCourseDetailsRepository allCourseDetailsRepo,
            SimsDbContext context)
        {
            _admissionRepo = admissionRepo;
            _departmentRepo = departmentRepo;
            _classRepo = classRepo;
            _subjectRepo = subjectRepo;
            _userRepo = userRepos;
            _studentRepo = studentRepo;
            _facultyRepo = facultyRepo;
            _courseRepo = courseRepo;
            _allCourseDetailsRepo = allCourseDetailsRepo;
            _context = context;
        }

        public async Task<AdminDashboardViewModel> GetDashboardDataAsync()
        {
            return new AdminDashboardViewModel
            {
                Admissions = await _admissionRepo.GetAllWithDetailsAsync(),
                Departments = await _departmentRepo.GetAllAsync(),
                Classes = await _classRepo.GetAllWithDetailsAsync(),
                Subjects = await _subjectRepo.GetAllWithDetailsAsync(),
                Users = await _userRepo.GetAllAsync(),
                Students = await _studentRepo.GetAllWithDetailsAsync(),
                Faculties = await _facultyRepo.GetAllWithDetailsAsync(),
                Courses = await _courseRepo.GetAllWithDetailsAsync()
            };
        }

        public async Task<ManageCourseViewModel?> ViewCourseDetailsAsync(int courseId)
        {
            return await _allCourseDetailsRepo.GetAllCourseDetailsAsync(courseId);
        }
    }
}