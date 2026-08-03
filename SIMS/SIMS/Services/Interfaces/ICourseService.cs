using SIMS.Models.Entities;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface ICourseService
    {
        Task<object> GetDepartmentFilterDataAsync(int departmentId);
        Task<(bool Success, string Message)> CreateCourseAsync(Course newCourse, int classId, List<int> selectedStudentIds);
        Task UpdateCourseAsync(Course course);
        Task<(bool Success, string Message)> DeleteCourseAsync(int courseId);


        Task<IEnumerable<StudentBasicViewModel>> GetStudentsByClassIdAsync(int classId);
        Task<(bool Success, string Message)> AddStudentToCourseAsync(int courseId, string studentCode);
        Task<(bool Success, string Message)> RemoveStudentFromCourseAsync(int courseId, int studentId);

    }
}
