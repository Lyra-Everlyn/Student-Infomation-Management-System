using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<Assignment> _assignmentRepo;
        private readonly IRepository<Grade> _gradeRepo;

        public AssignmentService(
            IRepository<Assignment> assignmentRepo,
            IRepository<Grade> gradeRepo)
        {
            _assignmentRepo = assignmentRepo;
            _gradeRepo = gradeRepo;
        }

        public async Task CreateAssignmentAsync(Assignment assignment)
        {
            await _assignmentRepo.AddAsync(assignment);
        }

        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            var existing = await _assignmentRepo.GetByIdAsync(assignment.AssignmentId);
            if (existing != null)
            {
                existing.AssignmentName = assignment.AssignmentName;
                existing.Description = assignment.Description;
                existing.StartTime = assignment.StartTime;
                existing.EndTime = assignment.EndTime;
                await _assignmentRepo.UpdateAsync(existing);
            }
        }

        public async Task DeleteAssignmentAsync(int assignmentId)
        {
            await _assignmentRepo.DeleteAsync(assignmentId);
        }
    }
}