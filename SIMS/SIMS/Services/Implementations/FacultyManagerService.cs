using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class FacultyManagerService : IFacultyManagerService
    {
        private readonly IFacultyRepository _facultyRepo;
        private readonly SimsDbContext _context;

        public FacultyManagerService(IFacultyRepository facultyRepo, SimsDbContext context)
        {
            _facultyRepo = facultyRepo;
            _context = context;
        }

        public async Task CreateFacultyAsync(Faculty faculty)
        {
            bool isAlreadyFaculty = await _context.Faculties.AnyAsync(f => f.UserId == faculty.UserId);
            if (isAlreadyFaculty)
            {
                throw new Exception("This user is already assigned as a faculty member.");
            }

            await _facultyRepo.AddAsync(faculty);
        }

        public async Task UpdateFacultyAsync(Faculty faculty)
        {
            var existingFaculty = await _facultyRepo.GetByIdAsync(faculty.FacultyId);
            if (existingFaculty == null) throw new Exception("Faculty data not found.");

            existingFaculty.DepartmentId = faculty.DepartmentId;
            await _facultyRepo.UpdateAsync(existingFaculty);
        }
    }
}