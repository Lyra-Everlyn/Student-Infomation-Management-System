using SIMS.Models.DTOs;
using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<AdmissionApplication?> GetApplicationByUserIdAsync(int userId);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task UpdateProfileAndSubmitAsync(int userId, UpdateProfileRequest request);
    }
}
