using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<IEnumerable<Class>> GetAllWithDetailsAsync();
        Task<bool> ClassExistsAsync(string className);
    }
}
