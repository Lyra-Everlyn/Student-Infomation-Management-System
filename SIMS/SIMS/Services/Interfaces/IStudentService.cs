using SIMS.Models.Entities;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDashboardViewModel?> GetDashboardDataAsync(string email);
        Task<(bool Success, string Message)> EnrollInCourseAsync(string email, string courseCode);
        Task<(bool Success, string Message)> UnenrollCourseAsync(string email, int courseId);

        Task<(Course? Course, IEnumerable<Assignment> Assignments, IEnumerable<Grade> MyGrades)> GetCourseAssignmentsAsync(int courseId, string email);
        Task<(Assignment? Assignment, Grade? Submission, Student? Student)> GetAssignmentDetailAsync(int assignmentId, string email);
        Task<(bool Success, string Message)> SubmitAssignmentAsync(int assignmentId, string email, IFormFile fileUpload);
    }
}
