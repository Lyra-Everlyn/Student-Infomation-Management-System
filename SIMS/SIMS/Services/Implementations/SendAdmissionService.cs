using Microsoft.AspNetCore.Identity;
using SIMS.Models.DTOs;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class SendAdmissionService : ISendAdmissionService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<AdmissionApplication> _applicationRepository;

        public SendAdmissionService(
            IRepository<User> userRepository,
            IRepository<AdmissionApplication> applicationRepository)
        {
            _userRepository = userRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task RegisterApplicantAsync(RegisterRequest request)
        {
            var allUsers = await _userRepository.GetAllAsync();
            if (allUsers.Any(u => u.Email.ToLower() == request.Email.Trim().ToLower()))
            {
                throw new Exception("This email has been used in system");
            }

            // 1. Create a new user, role = Applicant
            var newUser = new User
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                Role = "Applicant",
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var passwordHasher = new PasswordHasher<User>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

            await _userRepository.AddAsync(newUser);

            // 2. Create AdmissionApplication for new User, Status = Incomplete
            var application = new AdmissionApplication
            {
                UserId = newUser.UserId,
                Status = "Incomplete",
                CreatedAt = DateTime.Now
                // DepartmentId, DateOfBirth,... null
            };

            await _applicationRepository.AddAsync(application);
        }
    }
}
