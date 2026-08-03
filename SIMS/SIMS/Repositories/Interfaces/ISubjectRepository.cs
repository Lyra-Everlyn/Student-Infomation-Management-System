using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<IEnumerable<Subject>> GetAllWithDetailsAsync();
        Task<bool> SubjectExistsAsync(string subjectCode);
    }
}
