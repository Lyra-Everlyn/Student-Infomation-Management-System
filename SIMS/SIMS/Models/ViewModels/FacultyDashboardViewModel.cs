using SIMS.Models.Entities;

namespace SIMS.Models.ViewModels
{
    public class FacultyDashboardViewModel
    {
        public Faculty CurrentFaculty { get; set; } = null!;
        public IEnumerable<Course> MyCourses { get; set; } = new List<Course>();
    }
}