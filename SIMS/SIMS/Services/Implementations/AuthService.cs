using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentRepository _studentRepository;

        public AuthService(IRepository<User> userRepository, IFacultyRepository facultyRepository, IStudentRepository studentRepository)
        {
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
            _studentRepository = studentRepository;
        }


        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user == null) { throw new Exception("Account not found."); }
            if (user.Status == "Locked") { throw new Exception("Your account has been locked."); }

            bool isCorrectPasswords = await VerifyPasswordHashAsync(password, user.PasswordHash);
            if (user.Status == "Inactive") { throw new Exception("Your account has been temporarily suspended."); }
            if (!isCorrectPasswords) { throw new Exception("Incorrect password"); }

            if (user.Role == "Faculty")
            {
                var faculties = await _facultyRepository.GetAllWithDetailsAsync();
                bool isAssigned = faculties.Any(f => f.UserId == user.UserId);
                if (!isAssigned)
                {
                    throw new Exception("Your faculty account has not been assigned to a department yet. Please contact the administrator.");
                }
            }
            if (user.Role == "Student")
            {
                var students = await _studentRepository.GetAllWithDetailsAsync();
                bool isAssigned = students.Any(s => s.UserId == user.UserId);
                if (!isAssigned)
                {
                    throw new Exception("Your student account has not been assigned to a program yet. Please contact the administrator.");
                }
            }

            return user;
        }

        public async Task<User> AuthenticateGoogleUserAsync(string email, string fullName)
        {
            var users = await _userRepository.GetAllAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == email);

            if (existingUser == null) { throw new Exception("Account not found."); }
            if (existingUser.Status == "Locked") { throw new Exception("Your account has been locked."); }
            if (existingUser.Status == "Inactive") { throw new Exception("Your account has been temporarily suspended."); }

            return existingUser;
        }

        private async Task<bool> VerifyPasswordHashAsync(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedHash)){  return false; }
            try
            {
                var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(null!, storedHash, inputPassword);
                return await Task.FromResult(
                    result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success
                );
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

    }
}
