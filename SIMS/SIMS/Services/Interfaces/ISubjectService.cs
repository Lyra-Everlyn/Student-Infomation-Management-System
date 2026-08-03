using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface ISubjectService
    {
        Task CreateSubjectAsync(Subject subject);
        Task UpdateSubjectAsync(Subject subject);
        Task DeleteSubjectAsync(int subjectId);
    }
}