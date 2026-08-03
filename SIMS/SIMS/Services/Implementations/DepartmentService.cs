using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly SimsDbContext _context;

        public DepartmentService(IDepartmentRepository departmentRepo, SimsDbContext context)
        {
            _departmentRepo = departmentRepo;
            _context = context;
        }

        public async Task CreateDepartmentAsync(Department department)
        {
            bool exists = await _departmentRepo.DepartmentExistsAsync(department.DepartmentCode);
            if (exists)
            {
                throw new Exception($"The Department code '{department.DepartmentCode}' already exists in the system.");
            }
            await _departmentRepo.AddAsync(department);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            var existingDept = await _departmentRepo.GetByIdAsync(department.DepartmentId);
            if (existingDept == null) throw new Exception("Not found department data.");

            if (existingDept.DepartmentCode.ToLower() != department.DepartmentCode.ToLower())
            {
                bool exists = await _departmentRepo.DepartmentExistsAsync(department.DepartmentCode);
                if (exists)
                {
                    throw new Exception($"The Department code '{department.DepartmentCode}' already exists in the system.");
                }
            }

            existingDept.DepartmentCode = department.DepartmentCode;
            existingDept.DepartmentName = department.DepartmentName;

            await _departmentRepo.UpdateAsync(existingDept);
        }

        public async Task DeleteDepartmentAsync(int departmentId)
        {
            bool hasStudents = await _context.Students.AnyAsync(s => s.DepartmentId == departmentId);
            bool hasFaculties = await _context.Faculties.AnyAsync(f => f.DepartmentId == departmentId);
            bool hasApplications = await _context.AdmissionApplications.AnyAsync(a => a.DepartmentId == departmentId);
            bool hasClasses = await _context.Classes.AnyAsync(c => c.DepartmentId == departmentId);
            bool hasSubjects = await _context.Subjects.AnyAsync(s => s.DepartmentId == departmentId);

            if (hasStudents || hasFaculties || hasApplications || hasClasses || hasSubjects)
            {
                throw new Exception("This department cannot be deleted (it may contain linked data).");
            }

            var dept = await _departmentRepo.GetByIdAsync(departmentId);
            if (dept != null)
            {
                await _departmentRepo.DeleteAsync(departmentId);
            }
        }
    }
}