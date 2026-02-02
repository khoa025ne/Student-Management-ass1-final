using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.BLL.Services.DTOs;

namespace StudentManagement.BLL.Interfaces
{
    public interface IAccountService
    {
        // Admin tạo tài khoản (Manager, Student, Teacher)
        Task<(bool Success, UserDTO Data, string Message)> CreateAccountAsync(CreateAccountDTO createDto);
        
        // Cập nhật hồ sơ cá nhân
        Task<(bool Success, UserDTO Data, string Message)> UpdateProfileAsync(int userId, UpdateProfileDTO updateDto);
        
        // Lấy danh sách tất cả tài khoản (Admin)
        Task<List<UserDTO>> GetAllUsersAsync();
        
        // Lấy thông tin user theo ID
        Task<UserDTO> GetUserByIdAsync(int userId);
        
        // Khóa/Mở khóa tài khoản (Admin)
        Task<(bool Success, string Message)> ToggleUserStatusAsync(int userId);
        
        // Xóa tài khoản (Admin - soft delete)
        Task<(bool Success, string Message)> DeleteUserAsync(int userId);
        
        // Kiểm tra email đã tồn tại
        Task<bool> EmailExistsAsync(string email);
    }
}
