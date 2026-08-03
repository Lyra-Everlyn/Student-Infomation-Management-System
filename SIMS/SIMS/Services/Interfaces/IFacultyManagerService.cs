using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IFacultyManagerService
    {
        Task CreateFacultyAsync(Faculty faculty);
        Task UpdateFacultyAsync(Faculty faculty);
    }
}