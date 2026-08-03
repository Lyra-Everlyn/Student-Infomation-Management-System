using SIMS.Models.ViewModels;

namespace SIMS.Repositories.Interfaces
{
    public interface IAllCourseDetailsRepository
    {
        Task<ManageCourseViewModel?> GetAllCourseDetailsAsync(int courseId);
    }
}
