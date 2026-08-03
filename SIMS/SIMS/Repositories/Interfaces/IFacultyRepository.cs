using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IFacultyRepository : IRepository<Faculty>
    {
        Task<IEnumerable<Faculty>> GetAllWithDetailsAsync();
    }
}
