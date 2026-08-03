using SIMS.Models.DTOs;

namespace SIMS.Services.Interfaces
{
    public interface IAdmissionService
    {
        Task UpdateAdmissionApplicationAsync(int applicationId, int userId, string fullName, string phone, DateTime? dob, int departmentId, string status);
        Task DeleteAdmissionApplicationAsync(int applicationId);
    }
}
