using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.DTOs;
using StudentManagement.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class AuthController : Controller
    {
        // 1. Mở comment khai báo biến service
        private readonly IAuthService _authService;

        // 2. Mở comment và sửa Constructor để nhận IAuthService
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
                return RedirectToDashboard(userRole);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);

            // 3. Sử dụng service thay vì test cứng (Nếu bạn muốn dùng logic thật)
            var (success, user, message) = await _authService.LoginAsync(loginDto);

            if (success && user != null)
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.RoleName);

                return RedirectToDashboard(user.RoleName);
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
            if (!ModelState.IsValid) return View(registerDto);

            // Lỗi của bạn biến mất ở đây vì _authService đã được định nghĩa
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
            TempData["SuccessMessage"] = "Đăng xuất thành công!";
            return RedirectToAction("Login", "Auth");
        }

        // Hàm phụ giúp chuyển hướng dashboard
        private IActionResult RedirectToDashboard(string role)
        {
            return role switch
            {
                "Admin" => RedirectToAction("AdminDashboard", "Home"),
                "Manager" => RedirectToAction("ManagerDashboard", "Home"),
                "Teacher" => RedirectToAction("TeacherDashboard", "Home"),
                "Student" => RedirectToAction("StudentDashboard", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}