using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IStudentManagerService
    {
        Task UpdateStudentAsync(Student student);
    }
}