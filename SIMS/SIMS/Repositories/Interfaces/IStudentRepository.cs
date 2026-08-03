using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllWithDetailsAsync();
        Task<bool> StudentCodeExistsAsync(string studentCode, int excludeStudentId = 0);
    }
}
