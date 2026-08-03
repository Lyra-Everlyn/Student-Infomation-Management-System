using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.DTOs;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class ApplicantService : IApplicantService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<AdmissionApplication> _applicationRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly SimsDbContext _context;

        public ApplicantService(
            IRepository<User> userRepository,
            IRepository<AdmissionApplication> applicationRepository,
            IRepository<Department> departmentRepository,
            SimsDbContext context)
        {
            _userRepository = userRepository;
            _applicationRepository = applicationRepository;
            _departmentRepository = departmentRepository;
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<AdmissionApplication?> GetApplicationByUserIdAsync(int userId)
        {
            var apps = await _context.AdmissionApplications
                .Include(a => a.Department)
                .Include(a => a.User)
                .ToListAsync();

            return apps.FirstOrDefault(a => a.UserId == userId);
        }

        public async Task UpdateProfileAndSubmitAsync(int userId, UpdateProfileRequest request)
        {
            // 1. Update User table
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy tài khoản người dùng.");

            user.FullName = request.FullName.Trim();
            user.PhoneNumber = request.PhoneNumber?.Trim();
            user.Address = request.Address?.Trim();
            user.DateOfBirth = request.DateOfBirth;
            user.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(user);

            // 2. Find User AdmissionApplication record to update
            var applications = await _applicationRepository.GetAllAsync();
            var app = applications.FirstOrDefault(a => a.UserId == userId);

            if (app == null)
            {
                app = new AdmissionApplication
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };
                app.DepartmentId = request.DepartmentId;
                app.Status = "Pending";
                await _applicationRepository.AddAsync(app);
            }
            else
            {
                app.DepartmentId = request.DepartmentId;
                app.Status = "Pending";
                await _applicationRepository.UpdateAsync(app);
            }
        }
    }
}
