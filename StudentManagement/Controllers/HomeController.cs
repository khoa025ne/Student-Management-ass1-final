using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services.Interfaces;
using StudentManagement.ViewModels;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Controller Trang chủ và Dashboard - Điều hướng theo Role
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public HomeController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
        }

        // Trang chủ - Điều hướng theo Role
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            // Nếu chưa đăng nhập, hiển thị trang chủ công khai
            if (string.IsNullOrEmpty(userIdStr))
                return View();

            // Parse userId và lấy thông tin user
            if (int.TryParse(userIdStr, out int userId))
            {
                var user = await _accountService.GetUserByIdAsync(userId);
                if (user != null)
                {
                    // Điều hướng đến Dashboard tương ứng với Role
                    return userRole switch
                    {
                        "Admin" => RedirectToAction("AdminDashboard"),
                        "Manager" => RedirectToAction("ManagerDashboard"),
                        "Teacher" => RedirectToAction("TeacherDashboard"),
                        "Student" => RedirectToAction("StudentDashboard"),
                        _ => View()
                    };
                }
            }

            return View();
        }

        // Admin Dashboard - Chỉ Admin mới truy cập được
        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.GetUserByIdAsync(userId);
            if (user?.RoleName != "Admin")
                return Forbid();

            // Tạo dữ liệu Dashboard cho Admin
            var viewModel = new AdminDashboardViewModel
            {
                User = user,
                WelcomeMessage = $"Xin chào {user.Name}, bạn đang quản trị hệ thống",
                Statistics = new List<DashboardStatistic>
                {
                    new() { Title = "Tổng người dùng", Value = "1,234", Icon = "👥", Color = "blue", Change = "+12%" },
                    new() { Title = "Lớp học đang hoạt động", Value = "45", Icon = "📚", Color = "green", Change = "+8%" },
                    new() { Title = "Doanh thu tháng", Value = "2.5B VND", Icon = "💰", Color = "yellow", Change = "+15%" },
                    new() { Title = "Tỷ lệ đậu", Value = "87.5%", Icon = "🎓", Color = "purple", Change = "+3%" }
                },
                Notifications = new List<DashboardNotification>
                {
                    new() { Title = "Hệ thống", Message = "Backup tự động đã hoàn tất", Type = "success", CreatedAt = DateTime.Now.AddHours(-2) },
                    new() { Title = "Cảnh báo", Message = "Server load cao hơn bình thường", Type = "warning", CreatedAt = DateTime.Now.AddMinutes(-15) }
                },
                QuickActions = new List<DashboardQuickAction>
                {
                    new() { Title = "Quản lý người dùng", Description = "Thêm, sửa, xóa tài khoản", Icon = "👤", ActionUrl = "/User/Index", Color = "blue" },
                    new() { Title = "Quản lý môn học", Description = "Cập nhật curriculum", Icon = "📖", ActionUrl = "/Subject/Index", Color = "green" },
                    new() { Title = "Báo cáo hệ thống", Description = "Thống kê và phân tích", Icon = "📊", ActionUrl = "/Report/Index", Color = "purple" },
                    new() { Title = "Cài đặt hệ thống", Description = "Cấu hình và bảo mật", Icon = "⚙️", ActionUrl = "/Settings/Index", Color = "gray" }
                }
            };

            return View(viewModel);
        }

        // Manager Dashboard - Chỉ Manager mới truy cập được
        [HttpGet]
        public async Task<IActionResult> ManagerDashboard()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.GetUserByIdAsync(userId);
            if (user?.RoleName != "Manager")
                return Forbid();

            // Tạo dữ liệu Dashboard cho Manager (Phòng Đào Tạo)
            var viewModel = new DashboardViewModel
            {
                User = user,
                WelcomeMessage = $"Xin chào {user.Name}, Phòng Đào Tạo",
                Statistics = new List<DashboardStatistic>
                {
                    new() { Title = "Lớp học học kỳ này", Value = "128", Icon = "🏫", Color = "blue", Change = "+5%" },
                    new() { Title = "Sinh viên đang học", Value = "3,456", Icon = "👨‍🎓", Color = "green", Change = "+12%" },
                    new() { Title = "Giảng viên", Value = "89", Icon = "👨‍🏫", Color = "purple", Change = "+2%" },
                    new() { Title = "Tỷ lệ hoàn thành", Value = "92.3%", Icon = "📈", Color = "yellow", Change = "+1.2%" }
                },
                Notifications = new List<DashboardNotification>
                {
                    new() { Title = "Thông báo", Message = "Đã cập nhật lịch thi cuối kỳ Fall 2025", Type = "info", CreatedAt = DateTime.Now.AddHours(-1) },
                    new() { Title = "Cần xử lý", Message = "15 đơn đăng ký môn học chờ duyệt", Type = "warning", CreatedAt = DateTime.Now.AddMinutes(-30) }
                },
                QuickActions = new List<DashboardQuickAction>
                {
                    new() { Title = "Quản lý lớp học", Description = "Mở lớp, phân giảng viên", Icon = "📚", ActionUrl = "/Class/Index", Color = "blue" },
                    new() { Title = "Quản lý học kỳ", Description = "Lịch trình và timeline", Icon = "📅", ActionUrl = "/Semester/Index", Color = "green" },
                    new() { Title = "Báo cáo học thuật", Description = "Thống kê kết quả học tập", Icon = "📊", ActionUrl = "/Report/Academic", Color = "purple" },
                    new() { Title = "Duyệt đăng ký", Description = "Xử lý đơn đăng ký môn học", Icon = "✅", ActionUrl = "/Enrollment/Pending", Color = "orange" }
                }
            };

            return View(viewModel);
        }

        // Teacher Dashboard - Chỉ Teacher mới truy cập được
        [HttpGet]
        public async Task<IActionResult> TeacherDashboard()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.GetUserByIdAsync(userId);
            if (user?.RoleName != "Teacher")
                return Forbid();

            // Tạo dữ liệu Dashboard cho Teacher
            var viewModel = new TeacherDashboardViewModel
            {
                User = user,
                WelcomeMessage = $"Xin chào Thầy/Cô {user.Name}",
                Statistics = new List<DashboardStatistic>
                {
                    new() { Title = "Lớp đang dạy", Value = "4", Icon = "👨‍🏫", Color = "blue", Change = "0%" },
                    new() { Title = "Tổng sinh viên", Value = "127", Icon = "👥", Color = "green", Change = "+8" },
                    new() { Title = "Bài tập chưa chấm", Value = "23", Icon = "📝", Color = "orange", Change = "+5" },
                    new() { Title = "Điểm trung bình lớp", Value = "7.8", Icon = "🎯", Color = "purple", Change = "+0.3" }
                },
                Notifications = new List<DashboardNotification>
                {
                    new() { Title = "Nhắc nhở", Message = "Hạn nộp điểm cuối kỳ: 3 ngày nữa", Type = "warning", CreatedAt = DateTime.Now.AddHours(-4) },
                    new() { Title = "Thông báo", Message = "Lớp PRN211-SE1801 có sinh viên mới", Type = "info", CreatedAt = DateTime.Now.AddHours(-8) }
                },
                QuickActions = new List<DashboardQuickAction>
                {
                    new() { Title = "Quản lý lớp học", Description = "Xem danh sách, điểm danh", Icon = "📚", ActionUrl = "/Teacher/Classes", Color = "blue" },
                    new() { Title = "Nhập điểm", Description = "Cập nhật điểm sinh viên", Icon = "✍️", ActionUrl = "/Grade/Input", Color = "green" },
                    new() { Title = "Thông báo lớp", Description = "Gửi announcement", Icon = "📢", ActionUrl = "/Teacher/Announcements", Color = "purple" },
                    new() { Title = "Báo cáo học thuật", Description = "Thống kê kết quả lớp", Icon = "📊", ActionUrl = "/Teacher/Reports", Color = "orange" }
                },
                TeachingClasses = new(), // TODO: Load từ service
                TotalStudents = 127,
                PendingGrades = new() // TODO: Load từ service
            };

            return View(viewModel);
        }

        // Student Dashboard - Chỉ Student mới truy cập được
        [HttpGet]
        public async Task<IActionResult> StudentDashboard()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.GetUserByIdAsync(userId);
            if (user?.RoleName != "Student")
                return Forbid();

            // Tạo dữ liệu Dashboard cho Student
            var viewModel = new StudentDashboardViewModel
            {
                User = user,
                WelcomeMessage = $"Xin chào {user.Name} - {user.RollNumber}",
                Statistics = new List<DashboardStatistic>
                {
                    new() { Title = "Môn đang học", Value = "6", Icon = "📚", Color = "blue", Change = "+2" },
                    new() { Title = "GPA hiện tại", Value = "7.8", Icon = "🎯", Color = "green", Change = "+0.2" },
                    new() { Title = "Tín chỉ tích lũy", Value = "45/120", Icon = "📈", Color = "purple", Change = "+9" },
                    new() { Title = "Số dư ví", Value = user.WalletBalance.ToString("N0") + " VND", Icon = "💰", Color = "yellow", Change = "0" }
                },
                Notifications = new List<DashboardNotification>
                {
                    new() { Title = "Học phí", Message = "Hạn đóng học phí Fall 2025: 5 ngày nữa", Type = "warning", CreatedAt = DateTime.Now.AddHours(-2) },
                    new() { Title = "Điểm số", Message = "Đã có điểm môn CSD201", Type = "success", CreatedAt = DateTime.Now.AddHours(-6) },
                    new() { Title = "AI Analysis", Message = "Phân tích học tập mới của bạn đã sẵn sàng", Type = "info", CreatedAt = DateTime.Now.AddHours(-12) }
                },
                QuickActions = new List<DashboardQuickAction>
                {
                    new() { Title = "Đăng ký môn học", Description = "Chọn lớp cho học kỳ mới", Icon = "📝", ActionUrl = "/Student/Enroll", Color = "blue" },
                    new() { Title = "Xem điểm", Description = "Bảng điểm và transcript", Icon = "📊", ActionUrl = "/Student/Grades", Color = "green" },
                    new() { Title = "Nạp tiền", Description = "Nạp tiền vào ví học phí", Icon = "💳", ActionUrl = "/Student/Wallet", Color = "yellow" },
                    new() { Title = "AI Phân tích", Description = "Gợi ý học tập cá nhân hóa", Icon = "🤖", ActionUrl = "/Student/AIAnalysis", Color = "purple" }
                },
                CurrentGPA = 7.8m,
                WalletBalance = user.WalletBalance,
                CurrentEnrollments = new(), // TODO: Load từ service
                RecentGrades = new(), // TODO: Load từ service  
                AvailableClasses = new() // TODO: Load từ service
            };

            return View(viewModel);
        }
    }
}

