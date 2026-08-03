using Microsoft.AspNetCore.Mvc;
using SIMS.Models.DTOs;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    public class SendAdmissionController : Controller
    {
        private readonly ISendAdmissionService _sendAdmissionService;

        public SendAdmissionController(ISendAdmissionService sendAdmissionService)
        {
            _sendAdmissionService = sendAdmissionService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Admission/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admission/Register.cshtml", request);
            }

            try
            {
                await _sendAdmissionService.RegisterApplicantAsync(request);
                TempData["SuccessMessage"] = "Admission request submitted successfully!";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("~/Views/Admission/Register.cshtml", request);
            }
        }
    }
}
