using System;

namespace StudentManagement.BLL.Services.DTOs
{
    // Đăng ký
    public class RegisterDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }

    // Đăng nhập
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }

    // Response đăng nhập thành công
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public decimal WalletBalance { get; set; }
    }

    // Tạo tài khoản (Admin)
    public class CreateAccountDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Batch { get; set; }
    }

    // Cập nhật hồ sơ
    public class UpdateProfileDTO
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
    }

    // Thông tin user
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
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
