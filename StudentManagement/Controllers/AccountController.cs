using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.DTOs;
using StudentManagement.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Hồ sơ cá nhân
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Cập nhật hồ sơ cá nhân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDTO updateDto)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Auth");

            var (success, data, message) = await _accountService.UpdateProfileAsync(userId, updateDto);

            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Profile");
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Profile");
        }

        // GET: Admin - Danh sách tài khoản
        [HttpGet]
        public async Task<IActionResult> ManageAccounts()
        {
            var users = await _accountService.GetAllUsersAsync();
            return View(users);
        }

        // GET: Admin - Form tạo tài khoản
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        // POST: Admin - Tạo tài khoản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountDTO createDto)
        {
            if (!ModelState.IsValid)
                return View(createDto);

            var (success, data, message) = await _accountService.CreateAccountAsync(createDto);

            if (!success)
            {
                ModelState.AddModelError("", message);
                return View(createDto);
            }

            TempData["SuccessMessage"] = $"{message}. Mã sinh viên: {data?.RollNumber}";
            return RedirectToAction("ManageAccounts");
        }

        // POST: Admin - Khóa/Mở khóa tài khoản
        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int userId)
        {
            var (success, message) = await _accountService.ToggleUserStatusAsync(userId);

            if (!success)
                TempData["ErrorMessage"] = message;
            else
                TempData["SuccessMessage"] = message;

            return RedirectToAction("ManageAccounts");
        }

        // POST: Admin - Xóa tài khoản
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var (success, message) = await _accountService.DeleteUserAsync(userId);

            if (!success)
                TempData["ErrorMessage"] = message;
            else
                TempData["SuccessMessage"] = message;

            return RedirectToAction("ManageAccounts");
        }
    }
}
