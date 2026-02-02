using StudentManagement.BLL.Services.DTOs;

namespace StudentManagement.ViewModels
{
    /// <summary>
    /// ViewModel cho các trang Dashboard - Chứa thông tin tổng quan theo Role
    /// </summary>
    public class DashboardViewModel
    {
        public UserDTO User { get; set; } = null!;
        public string WelcomeMessage { get; set; } = string.Empty;
        public List<DashboardStatistic> Statistics { get; set; } = new();
        public List<DashboardNotification> Notifications { get; set; } = new();
        public List<DashboardQuickAction> QuickActions { get; set; } = new();
    }

    /// <summary>
    /// Thống kê hiển thị trên Dashboard (Số liệu tổng quan)
    /// </summary>
    public class DashboardStatistic
    {
        public string Title { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Change { get; set; } = string.Empty;
        public bool IsPositive { get; set; } = true;
    }

    /// <summary>
    /// Thông báo nhanh trên Dashboard
    /// </summary>
    public class DashboardNotification
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; } = "info"; // info, warning, success, error
        public string ActionUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Các hành động nhanh theo Role
    /// </summary>
    public class DashboardQuickAction
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel riêng cho Student Dashboard
    /// </summary>
    public class StudentDashboardViewModel : DashboardViewModel
    {
        public List<EnrollmentDTO> CurrentEnrollments { get; set; } = new();
        public List<GradeDTO> RecentGrades { get; set; } = new();
        public decimal CurrentGPA { get; set; }
        public decimal WalletBalance { get; set; }
        public List<ClassDTO> AvailableClasses { get; set; } = new();
    }

    /// <summary>
    /// ViewModel riêng cho Teacher Dashboard  
    /// </summary>
    public class TeacherDashboardViewModel : DashboardViewModel
    {
        public List<ClassDTO> TeachingClasses { get; set; } = new();
        public int TotalStudents { get; set; }
        public List<GradeDTO> PendingGrades { get; set; } = new();
    }

    /// <summary>
    /// ViewModel riêng cho Admin/Manager Dashboard
    /// </summary>
    public class AdminDashboardViewModel : DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalEnrollments { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<UserDTO> RecentUsers { get; set; } = new();
        public List<SystemLog> SystemLogs { get; set; } = new();
    }

    /// <summary>
    /// Log hệ thống (Tạm thời - sau này có thể tách ra Entity riêng)
    /// </summary>
    public class SystemLog
    {
        public string Action { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}