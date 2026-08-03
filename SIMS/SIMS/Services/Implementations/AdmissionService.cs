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
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionApplicationRepository _admissionRepo;
        private readonly IUserRepository _userRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly SimsDbContext _context;

        public AdmissionService(
            IAdmissionApplicationRepository admissionRepo,
            IUserRepository userRepo,
            IDepartmentRepository departmentRepo,
            SimsDbContext context)
        {
            _admissionRepo = admissionRepo;
            _userRepo = userRepo;
            _departmentRepo = departmentRepo;
            _context = context;
        }

        public async Task UpdateAdmissionApplicationAsync(int applicationId, int userId, string fullName, string phone, DateTime? dob, int departmentId, string status)
        {
            var application = await _admissionRepo.GetByIdAsync(applicationId);
            var user = await _userRepo.GetByIdAsync(userId);

            if (application == null || user == null) throw new Exception("Not found admission application or user");


            user.FullName = fullName;
            user.PhoneNumber = phone;
            user.DateOfBirth = dob;
            user.UpdatedAt = DateTime.Now;
            await _userRepo.UpdateAsync(user);

            application.DepartmentId = departmentId;
            application.Status = status;
            await _admissionRepo.UpdateAsync(application);

            if (status == "Approved")
            {
                await HandleApprovalLogicAsync(user, departmentId);
            }
        }

        public async Task DeleteAdmissionApplicationAsync(int applicationId)
        {
            await _admissionRepo.DeleteAsync(applicationId);
        }

        private async Task HandleApprovalLogicAsync(User user, int departmentId)
        {
            bool studentExists = await _context.Students.AnyAsync(s => s.UserId == user.UserId);
            if (studentExists) return;


            user.Role = "Student";
            await _userRepo.UpdateAsync(user);


            string randomDigits, studentCode;
            do
            {
                randomDigits = new Random().Next(1000, 9999).ToString();
                studentCode = $"BH{DateTime.Now.Year}{randomDigits}";
            } while (await _context.Students.AnyAsync(s => s.StudentCode == studentCode));


            var department = await _departmentRepo.GetByIdAsync(departmentId);
            string deptPrefix = department?.DepartmentCode ?? ("DEP" + departmentId);
            string yearStr = DateTime.Now.Year.ToString();

            var existingClasses = await _context.Classes
                .Include(c => c.Students)
                .Where(c => c.DepartmentId == departmentId && c.ClassName.StartsWith($"{deptPrefix}{yearStr}"))
                .OrderBy(c => c.ClassName)
                .ToListAsync();

            var targetClass = existingClasses.FirstOrDefault(c => c.Students.Count < 30);

            if (targetClass == null)
            {
                targetClass = new Class
                {
                    ClassName = $"{deptPrefix}{yearStr}{(existingClasses.Count + 1):D2}",
                    DepartmentId = departmentId
                };
                await _context.Classes.AddAsync(targetClass);
                await _context.SaveChangesAsync();
            }


            var newStudent = new Student
            {
                UserId = user.UserId,
                StudentCode = studentCode,
                DepartmentId = departmentId,
                ClassId = targetClass.ClassId
            };

            await _context.Students.AddAsync(newStudent);
            await _context.SaveChangesAsync();
        }
    }
}