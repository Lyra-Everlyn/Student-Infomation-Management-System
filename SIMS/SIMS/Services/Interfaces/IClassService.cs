using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IClassService
    {
        Task CreateClassAsync(Class cls);
        Task UpdateClassAsync(Class cls);
        Task DeleteClassAsync(int classId);
    }
}
