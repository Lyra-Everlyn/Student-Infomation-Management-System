using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Faculty")]
    [Route("Faculty/Assignment")]
    public class GradeAssignmentController : Controller
    {
        private readonly IGradeService _gradeService;

        public GradeAssignmentController(IAssignmentService assignmentService, IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost("Grade/Save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeSubmission(int gradeId, decimal score, string feedback, int courseId)
        {
            try
            {
                await _gradeService.GradeSubmissionAsync(gradeId, score, feedback);
                TempData["Success"] = "Grade and feedback saved successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Manage", "Assignment", new { courseId = courseId });
        }
    }
}