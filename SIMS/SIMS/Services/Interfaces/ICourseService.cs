using SIMS.Models.Entities;

namespace SIMS.Services.Interfaces
{
    public interface ICourseService
    {
        Task<object> GetDepartmentFilterDataAsync(int departmentId);
        Task<(bool Success, string Message)> CreateCourseAsync(Course course, int classId);
        Task UpdateCourseAsync(Course course);
        Task<(bool Success, string Message)> DeleteCourseAsync(int courseId);
    }
}
