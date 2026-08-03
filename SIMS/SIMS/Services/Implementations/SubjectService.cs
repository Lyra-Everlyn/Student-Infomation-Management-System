using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepo;
        private readonly SimsDbContext _context;

        public SubjectService(ISubjectRepository subjectRepo, SimsDbContext context)
        {
            _subjectRepo = subjectRepo;
            _context = context;
        }

        public async Task CreateSubjectAsync(Subject subject)
        {
            bool exists = await _subjectRepo.SubjectExistsAsync(subject.SubjectCode);
            if (exists)
            {
                throw new Exception($"The subject code '{subject.SubjectCode}' already exists in the system.");
            }

            await _subjectRepo.AddAsync(subject);
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            var existingSubject = await _subjectRepo.GetByIdAsync(subject.SubjectId);
            if (existingSubject == null) throw new Exception("The subject data was not found.");

            if (existingSubject.SubjectCode.ToLower() != subject.SubjectCode.ToLower())
            {
                bool exists = await _subjectRepo.SubjectExistsAsync(subject.SubjectCode);
                if (exists)
                {
                    throw new Exception($"The subject code '{subject.SubjectCode}' already exists in the system.");
                }
            }

            existingSubject.SubjectName = subject.SubjectName;
            existingSubject.SubjectCode = subject.SubjectCode;
            existingSubject.Credits = subject.Credits;
            existingSubject.DepartmentId = subject.DepartmentId;

            await _subjectRepo.UpdateAsync(existingSubject);
        }

        public async Task DeleteSubjectAsync(int subjectId)
        {
            bool isUsedInCourses = await _context.Courses.AnyAsync(c => c.SubjectId == subjectId);
            if (isUsedInCourses)
            {
                throw new Exception("The subject cannot be deleted because there are courses using it.");
            }

            var subject = await _subjectRepo.GetByIdAsync(subjectId);
            if (subject != null)
            {
                await _subjectRepo.DeleteAsync(subjectId);
            }
        }
    }
}