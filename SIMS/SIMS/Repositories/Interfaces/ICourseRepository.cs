using SIMS.Models.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllWithDetailsAsync();
        Task<IEnumerable<Course>> GetCoursesByFacultyAsync(int facultyId);
    }
}
