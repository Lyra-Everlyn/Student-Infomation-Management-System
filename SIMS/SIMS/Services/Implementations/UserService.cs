using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly SimsDbContext _context;

        public UserService(IUserRepository userRepo, SimsDbContext context)
        {
            _userRepo = userRepo;
            _context = context;
        }

        public async Task CreateUserAsync(User user, string rawPassword)
        {
            bool emailExists = await _userRepo.EmailExistsAsync(user.Email);
            if (emailExists) throw new Exception("Email has already been used.");

            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            user.Status = "Active";

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, rawPassword);

            await _userRepo.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepo.GetByIdAsync(user.UserId);
            if (existingUser == null) throw new Exception("User not found.");

            if (existingUser.Email.ToLower() != user.Email.ToLower())
            {
                bool emailExists = await _userRepo.EmailExistsAsync(user.Email);
                if (emailExists) throw new Exception("Email has already been used by another account.");
            }

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Status = user.Status;
            existingUser.UpdatedAt = DateTime.Now;

            await _userRepo.UpdateAsync(existingUser);
        }

        public async Task DeleteUserAsync(int userId)
        {
            bool isAdmin = await _context.Users.AnyAsync(u => u.UserId == userId && u.Role == "Admin");
            bool isStudent = await _context.Students.AnyAsync(s => s.UserId == userId);
            bool isFaculty = await _context.Faculties.AnyAsync(f => f.UserId == userId);
            bool hasApplication = await _context.AdmissionApplications.AnyAsync(a => a.UserId == userId);

            if (isStudent || isFaculty || hasApplication)
            {
                throw new Exception("Cannot delete this account because there is linked data. Please change the status to 'Locked' instead of deleting.");
            }

            if (isAdmin) { throw new Exception("Cannot delete an admin account."); }

            var user = await _userRepo.GetByIdAsync(userId);
            if (user != null)
            {
                await _userRepo.DeleteAsync(userId);
            }
        }
    }
}