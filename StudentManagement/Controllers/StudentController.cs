using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Bảng điều khiển - Sinh viên";
            return View();
        }

        public IActionResult Schedule()
        {
            ViewData["Title"] = "Thời khóa biểu";
            return View();
        }

        public IActionResult Subjects()
        {
            ViewData["Title"] = "Môn học đang học";
            return View();
        }

        public IActionResult Grades()
        {
            ViewData["Title"] = "Điểm số & Kết quả";
            return View();
        }

        public IActionResult Registration()
        {
            ViewData["Title"] = "Đăng ký môn học";
            return View();
        }

        public IActionResult Wallet()
        {
            ViewData["Title"] = "Ví điện tử";
            return View();
        }

        public IActionResult Payment()
        {
            ViewData["Title"] = "Thanh toán học phí";
            return View();
        }

        public IActionResult TransactionHistory()
        {
            ViewData["Title"] = "Lịch sử giao dịch";
            return View();
        }

        public IActionResult AIAnalysis()
        {
            ViewData["Title"] = "Phân tích AI";
            return View();
        }
    }
}