using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IAdmissionApplicationRepository : IRepository<AdmissionApplication>
    {
        // Get all Admission Application data include User, register Department
        Task<IEnumerable<AdmissionApplication>> GetAllWithDetailsAsync();
    }
}
