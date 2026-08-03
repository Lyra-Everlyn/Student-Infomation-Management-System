using SIMS.Models.Entities;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<FacultyDashboardViewModel?> GetDashboardDataAsync(string email);
        Task<ManageCourseViewModel?> GetCourseDetailsAsync(int courseId, string email);
    }
}