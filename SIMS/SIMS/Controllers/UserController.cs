using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user, string RawPassword)
        {
            try
            {
                await _userService.CreateUserAsync(user, RawPassword);
                TempData["SuccessMessage"] = "Account created successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creating account: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(User user)
        {
            try
            {
                await _userService.UpdateUserAsync(user);
                TempData["SuccessMessage"] = "Account updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating account: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                TempData["SuccessMessage"] = "Account deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}