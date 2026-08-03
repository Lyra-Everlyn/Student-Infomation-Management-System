using SIMS.Models.Entities;

namespace SIMS.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<AdmissionApplication> Admissions { get; set; } = new List<AdmissionApplication>();
        public IEnumerable<Department> Departments { get; set; } = new List<Department>();
        public IEnumerable<Class> Classes { get; set; } = new List<Class>();
        public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();

        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
        public IEnumerable<Faculty> Faculties { get; set; } = new List<Faculty>();
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
    }
}
