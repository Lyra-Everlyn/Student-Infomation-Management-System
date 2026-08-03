using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("Student")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> StudentDashboard()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var viewModel = await _studentService.GetDashboardDataAsync(userEmail!);

            if (viewModel == null) return RedirectToAction("Login", "Auth");
            return View(viewModel);
        }

        //[HttpPost("Course/Join")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> JoinCourse(string sectionCode)
        //{
        //    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        //    var result = await _studentService.EnrollInCourseAsync(userEmail!, sectionCode);

        //    if (result.Success) TempData["Success"] = result.Message;
        //    else TempData["Error"] = result.Message;

        //    return RedirectToAction(nameof(StudentDashboard));
        //}

        //[HttpGet("Course/Leave/{courseId}")]
        //public async Task<IActionResult> LeaveCourse(int courseId)
        //{
        //    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        //    var result = await _studentService.UnenrollCourseAsync(userEmail!, courseId);

        //    if (result.Success) TempData["Success"] = result.Message;
        //    else TempData["Error"] = result.Message;

        //    return RedirectToAction(nameof(StudentDashboard));
        //}
    }
}