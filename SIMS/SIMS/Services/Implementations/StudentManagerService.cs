using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class StudentManagerService : IStudentManagerService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IClassRepository _classRepo;

        public StudentManagerService(IStudentRepository studentRepo, IClassRepository classRepo)
        {
            _studentRepo = studentRepo;
            _classRepo = classRepo;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            var existingStudent = await _studentRepo.GetByIdAsync(student.StudentId);
            if (existingStudent == null) throw new Exception("Not found student data.");

            if (existingStudent.StudentCode.ToLower() != student.StudentCode.ToLower())
            {
                bool codeExists = await _studentRepo.StudentCodeExistsAsync(student.StudentCode, student.StudentId);
                if (codeExists)
                {
                    throw new Exception($"Student code '{student.StudentCode}' is already in use by another student.");
                }
            }

            if (student.ClassId.HasValue)
            {
                var assignedClass = await _classRepo.GetByIdAsync(student.ClassId.Value);
                if (assignedClass != null && assignedClass.DepartmentId != student.DepartmentId)
                {
                    throw new Exception("The selected class does not belong to the assigned department. Please select again.");
                }
            }

            existingStudent.StudentCode = student.StudentCode;
            existingStudent.DepartmentId = student.DepartmentId;
            existingStudent.ClassId = student.ClassId;

            await _studentRepo.UpdateAsync(existingStudent);
        }
    }
}