namespace SIMS.Services.Interfaces
{
    public interface IGradeService
    {
        Task GradeSubmissionAsync(int gradeId, decimal score, string feedback);
    }
}