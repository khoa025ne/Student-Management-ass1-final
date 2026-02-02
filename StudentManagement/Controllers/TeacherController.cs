using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Bảng điều khiển - Giảng viên";
            return View();
        }

        public IActionResult Schedule()
        {
            ViewData["Title"] = "Lịch giảng dạy";
            return View();
        }

        public IActionResult Classes()
        {
            ViewData["Title"] = "Lớp học của tôi";
            return View();
        }

        public IActionResult StudentList()
        {
            ViewData["Title"] = "Danh sách sinh viên";
            return View();
        }

        public IActionResult Attendance()
        {
            ViewData["Title"] = "Điểm danh";
            return View();
        }

        public IActionResult GradeEntry()
        {
            ViewData["Title"] = "Nhập điểm";
            return View();
        }

        public IActionResult ClassStatistics()
        {
            ViewData["Title"] = "Thống kê lớp học";
            return View();
        }

        public IActionResult Reports()
        {
            ViewData["Title"] = "Báo cáo học tập";
            return View();
        }

        public IActionResult SubjectManagement()
        {
            ViewData["Title"] = "Quản lý môn học";
            return View();
        }

        public IActionResult SlotManagement()
        {
            ViewData["Title"] = "Quản lý ca học";
            return View();
        }

        public IActionResult AIAnalysis()
        {
            ViewData["Title"] = "Phân tích AI";
            return View();
        }
    }
}