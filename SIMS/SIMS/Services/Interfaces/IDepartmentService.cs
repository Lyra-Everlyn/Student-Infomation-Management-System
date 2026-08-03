using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task CreateDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int departmentId);
    }
}
