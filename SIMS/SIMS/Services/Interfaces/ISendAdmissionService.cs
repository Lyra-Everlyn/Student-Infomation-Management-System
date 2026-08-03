using SIMS.Models.DTOs;
namespace SIMS.Services.Interfaces
{
    public interface ISendAdmissionService
    {
        // Create Applicant account
        Task RegisterApplicantAsync(RegisterRequest request);
    }
}
