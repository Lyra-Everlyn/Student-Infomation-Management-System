using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task CreateAssignmentAsync(Assignment assignment);
        Task UpdateAssignmentAsync(Assignment assignment);
        Task DeleteAssignmentAsync(int assignmentId);
    }
}