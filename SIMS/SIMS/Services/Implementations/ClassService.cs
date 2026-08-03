using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepo;
        private readonly SimsDbContext _context;


        public ClassService(IClassRepository classRepo, SimsDbContext context)
        {
            _classRepo = classRepo;
            _context = context;
        }

        public async Task CreateClassAsync(Class cls)
        {
            bool exists = await _classRepo.ClassExistsAsync(cls.ClassName);
            if (exists)
            {
                throw new Exception($"The class name '{cls.ClassName}' already exists in the system.");
            }

            await _classRepo.AddAsync(cls);
        }

        public async Task UpdateClassAsync(Class cls)
        {
            var existingClass = await _classRepo.GetByIdAsync(cls.ClassId);
            if (existingClass == null) throw new Exception("Class data not found.");

            if (existingClass.ClassName.ToLower() != cls.ClassName.ToLower())
            {
                bool exists = await _classRepo.ClassExistsAsync(cls.ClassName);
                if (exists)
                {
                    throw new Exception($"The class name '{cls.ClassName}' already exists in the system.");
                }
            }

            existingClass.ClassName = cls.ClassName;
            existingClass.DepartmentId = cls.DepartmentId;

            await _classRepo.UpdateAsync(existingClass);
        }

        public async Task DeleteClassAsync(int classId)
        {
            bool hasStudents = await _context.Students.AnyAsync(s => s.ClassId == classId);
            if (hasStudents)
            {
                throw new Exception("This class cannot be deleted because there are students enrolled in it.");
            }

            var cls = await _classRepo.GetByIdAsync(classId);
            if (cls != null)
            {
                await _classRepo.DeleteAsync(classId);
            }
        }
    }
}