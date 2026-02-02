using System;

namespace StudentManagement.BLL.Services.DTOs
{
    // Phục vụ đăng ký tài khoản mới (AuthService)
    public class RegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
    }

    public class LoginDTO // Bắt buộc phải có từ khóa public
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public decimal WalletBalance { get; set; }
    }
    public class UpdateProfileDTO
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
    }

    // Sửa lỗi: Thêm Name để khớp với logic AccountService
    public class CreateAccountDTO
    {
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int? Batch { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? RollNumber { get; set; }
        public string? ClassCode { get; set; }
        public int? Batch { get; set; }
        public decimal WalletBalance { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}