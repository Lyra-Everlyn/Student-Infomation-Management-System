using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetEnrollmentsBySectionAsync(int sectionId);
    }
}
