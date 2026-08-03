using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<bool> DepartmentExistsAsync(string departmentCode);
    }
}
