using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdmissionController : Controller
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionController(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdmission(int ApplicationId, int UserId, string FullName, string PhoneNumber, DateTime? DateOfBirth, int DepartmentId, string Status)
        {
            try
            {
                await _admissionService.UpdateAdmissionApplicationAsync(ApplicationId, UserId, FullName, PhoneNumber, DateOfBirth, DepartmentId, Status);
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Update error: " + ex.Message;
            }

            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdmission(int id)
        {
            try
            {
                await _admissionService.DeleteAdmissionApplicationAsync(id);
                TempData["SuccessMessage"] = "Admissions record deleted.";
            }
            catch
            {
                TempData["ErrorMessage"] = "This record cannot be deleted.";
            }

            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}