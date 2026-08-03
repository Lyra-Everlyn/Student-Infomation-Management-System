using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(Subject subject)
        {
            try
            {
                await _subjectService.CreateSubjectAsync(subject);
                TempData["SuccessMessage"] = "Subject added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding subject: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSubject(Subject subject)
        {
            try
            {
                await _subjectService.UpdateSubjectAsync(subject);
                TempData["SuccessMessage"] = "Subject updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating subject: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await _subjectService.DeleteSubjectAsync(id);
                TempData["SuccessMessage"] = "Subject deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting subject: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}