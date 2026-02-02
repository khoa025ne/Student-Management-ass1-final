using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.DTOs;
using StudentManagement.BLL.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Controller quản lý người dùng - Admin & Manager
    /// Sử dụng IAccountService thay vì IUserManagementService
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Xem danh sách người dùng - Chỉ Admin & Manager
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _accountService.GetAllUsersAsync();
            return View(users);
        }

        /// <summary>
        /// Trang tạo người dùng - Chỉ Admin & Manager
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Xử lý tạo người dùng
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountDTO createUserDto)
        {
            if (!ModelState.IsValid)
                return View(createUserDto);

            var (success, data, message) = await _accountService.CreateAccountAsync(createUserDto);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = message;
            return View(createUserDto);
        }

        /// <summary>
        /// Chỉnh sửa người dùng
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        /// <summary>
        /// Xóa người dùng - CHỈ ADMIN
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _accountService.DeleteUserAsync(id);

            if (success)
                TempData["SuccessMessage"] = message;
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Khóa/Mở khóa tài khoản - CHỈ ADMIN
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int userId)
        {
            var (success, message) = await _accountService.ToggleUserStatusAsync(userId);

            if (success)
                TempData["SuccessMessage"] = message;
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }
    }
}
