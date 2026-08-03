using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetDashboardDataAsync();
        Task<ManageCourseViewModel?> ViewCourseDetailsAsync(int courseId);
    }
}