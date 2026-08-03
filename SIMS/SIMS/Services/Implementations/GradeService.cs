using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class GradeService : IGradeService
    {
        private readonly IRepository<Grade> _gradeRepo;

        public GradeService(IRepository<Grade> gradeRepo)
        {
            _gradeRepo = gradeRepo;
        }

        public async Task GradeSubmissionAsync(int gradeId, decimal score, string feedback)
        {
            var gradeRecord = await _gradeRepo.GetByIdAsync(gradeId);
            if (gradeRecord != null)
            {
                gradeRecord.Score = score;
                gradeRecord.Feedback = feedback;
                gradeRecord.GradedAt = DateTime.Now;
                await _gradeRepo.UpdateAsync(gradeRecord);
            }
            else
            {
                throw new Exception("Student submission not found.");
            }
        }
    }
}