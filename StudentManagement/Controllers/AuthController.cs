using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.DTOs;
using StudentManagement.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var (success, data, message) = await _authService.LoginAsync(loginDto);

            if (success && data != null)
            {
                HttpContext.Session.SetString("UserId", data.UserId.ToString());
                HttpContext.Session.SetString("UserName", data.Name);
                HttpContext.Session.SetString("UserRole", data.RoleName);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", message);
            return View(loginDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var (success, message) = await _authService.RegisterAsync(registerDto);

            if (!success)
            {
                ModelState.AddModelError("", message);
                return View(registerDto);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdStr, out int userId))
            {
                await _authService.LogoutAsync(userId);
            }

            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Ðang xu?t thành công!";
            return RedirectToAction("Login", "Auth");
        }
    }
}
