using SIMS.Models.Entities;

namespace SIMS.Models.ViewModels
{
    public class StudentDashboardViewModel
    {
        public Student CurrentStudent { get; set; } = null!;
        public IEnumerable<Enrollment> MyEnrollments { get; set; } = new List<Enrollment>();
    }
}
