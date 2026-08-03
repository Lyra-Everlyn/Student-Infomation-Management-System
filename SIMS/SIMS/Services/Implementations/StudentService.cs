using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SIMS.Helpers;
using SIMS.Models.Entities;
using SIMS.Models.ViewModels;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IRepository<Enrollment> _enrollmentRepo;
        private readonly IRepository<Assignment> _assignmentRepo;
        private readonly IRepository<Grade> _gradeRepo;
        private readonly Cloudinary _cloudinary;

        public StudentService(
            IStudentRepository studentRepo,
            ICourseRepository courseRepo,
            IRepository<Enrollment> enrollmentRepo,
            IRepository<Assignment> assignmentRepo,
            IRepository<Grade> gradeRepo,
            Cloudinary cloudinary)
        {
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
            _enrollmentRepo = enrollmentRepo;
            _assignmentRepo = assignmentRepo;
            _gradeRepo = gradeRepo;
            _cloudinary = cloudinary;
        }

        public async Task<StudentDashboardViewModel?> GetDashboardDataAsync(string email)
        {
            var students = await _studentRepo.GetAllWithDetailsAsync();
            var currentStudent = students.FirstOrDefault(s => s.User?.Email == email);
            if (currentStudent == null) return null;

            var enrollments = (await _enrollmentRepo.GetAllAsync())
                              .Where(e => e.StudentId == currentStudent.StudentId)
                              .ToList();

            var allCourses = await _courseRepo.GetAllWithDetailsAsync();
            foreach (var e in enrollments)
            {
                e.Course = allCourses.FirstOrDefault(c => c.CourseId == e.CourseId)!;
            }

            return new StudentDashboardViewModel
            {
                CurrentStudent = currentStudent,
                MyEnrollments = enrollments
            };
        }

        //public async Task<(bool Success, string Message)> EnrollInCourseAsync(string email, string courseCode)
        //{
        //    var students = await _studentRepo.GetAllWithDetailsAsync();
        //    var currentStudent = students.FirstOrDefault(s => s.User?.Email == email);
        //    if (currentStudent == null) return (false, "Not found .");

        //    var allCourses = await _courseRepo.GetAllAsync();
        //    var targetCourse = allCourses.FirstOrDefault(c => c.CourseCode == courseCode.Trim());
        //    if (targetCourse == null) return (false, "Invalid enrollment code! Course not found.");

        //    var allEnrollments = await _enrollmentRepo.GetAllAsync();
        //    if (allEnrollments.Any(e => e.StudentId == currentStudent.StudentId && e.CourseId == targetCourse.CourseId))
        //    {
        //        return (false, "You have already enrolled in this course.");
        //    }

        //    var newEnrollment = new Enrollment
        //    {
        //        StudentId = currentStudent.StudentId,
        //        CourseId = targetCourse.CourseId,
        //        EnrollmentDate = DateTime.Now
        //    };

        //    await _enrollmentRepo.AddAsync(newEnrollment);
        //    return (true, $"Successfully enrolled in: {targetCourse.CourseName}!");
        //}

        //public async Task<(bool Success, string Message)> UnenrollCourseAsync(string email, int courseId)
        //{
        //    var students = await _studentRepo.GetAllWithDetailsAsync();
        //    var currentStudent = students.FirstOrDefault(s => s.User?.Email == email);
        //    if (currentStudent == null) return (false, "Not found .");

        //    // Logic block leave mandatory course
        //    var targetCourse = await _courseRepo.GetByIdAsync(courseId);
        //    if (targetCourse != null && currentStudent.Class != null)
        //    {
        //        if (targetCourse.CourseName.Contains(currentStudent.Class.ClassName))
        //        {
        //            return (false, "Cannot unenroll from this course.");
        //        }
        //    }

        //    var allEnrollments = await _enrollmentRepo.GetAllAsync();
        //    var enrollment = allEnrollments.FirstOrDefault(e => e.StudentId == currentStudent.StudentId && e.CourseId == courseId);
        //    if (enrollment == null) return (false, "Not found enrollment data.");

        //    await _enrollmentRepo.DeleteAsync(enrollment.EnrollmentId);
        //    return (true, "Successfully unenrolled from the course.");
        //}

        public async Task<(Course? Course, IEnumerable<Assignment> Assignments, IEnumerable<Grade> MyGrades)> GetCourseAssignmentsAsync(int courseId, string email)
        {
            var students = await _studentRepo.GetAllWithDetailsAsync();
            var currentStudent = students.FirstOrDefault(s => s.User?.Email == email);

            var course = await _courseRepo.GetByIdAsync(courseId);
            var assignments = (await _assignmentRepo.GetAllAsync()).Where(a => a.CourseId == courseId).ToList();
            var myGrades = (await _gradeRepo.GetAllAsync()).Where(g => g.StudentId == currentStudent?.StudentId).ToList();

            return (course, assignments, myGrades);
        }

        public async Task<(Assignment? Assignment, Grade? Submission, Student? Student)> GetAssignmentDetailAsync(int assignmentId, string email)
        {
            var students = await _studentRepo.GetAllWithDetailsAsync();
            var currentStudent = students.FirstOrDefault(s => s.User?.Email == email);
            var assignment = await _assignmentRepo.GetByIdAsync(assignmentId);
            var submission = (await _gradeRepo.GetAllAsync())
                             .FirstOrDefault(g => g.AssignmentId == assignmentId && g.StudentId == currentStudent?.StudentId);

            return (assignment, submission, currentStudent);
        }

        public async Task<(bool Success, string Message)> SubmitAssignmentAsync(int assignmentId, string email, IFormFile fileUpload)
        {
            var students = await _studentRepo.GetAllWithDetailsAsync();
            var currentStudent = students.FirstOrDefault(s => s.User?.Email == email);
            if (currentStudent == null) return (false, "Not found .");

            if (fileUpload == null || fileUpload.Length == 0) return (false, "Please select a valid file to submit.");

            // 1. Upload new file to Cloudinary
            string? fileUrl = await CloudinaryHelper.UploadFileAsync(fileUpload, _cloudinary);
            if (string.IsNullOrEmpty(fileUrl)) return (false, "Upload failed.");

            // 2. Update data
            var existingGrade = (await _gradeRepo.GetAllAsync())
                                .FirstOrDefault(g => g.AssignmentId == assignmentId && g.StudentId == currentStudent.StudentId);

            if (existingGrade != null)
            {
                if (!string.IsNullOrEmpty(existingGrade.FileUrl))
                {
                    await CloudinaryHelper.DeleteFileAsync(existingGrade.FileUrl, _cloudinary);
                }

                existingGrade.FileUrl = fileUrl;
                existingGrade.SubmitDate = DateTime.Now;
                await _gradeRepo.UpdateAsync(existingGrade);
            }
            else
            {
                var newGrade = new Grade
                {
                    AssignmentId = assignmentId,
                    StudentId = currentStudent.StudentId,
                    FileUrl = fileUrl,
                    SubmitDate = DateTime.Now
                };
                await _gradeRepo.AddAsync(newGrade);
            }

            return (true, "Successfully submitted assignment!");
        }
    }
}