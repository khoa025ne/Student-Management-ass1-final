using System.Threading.Tasks;
using StudentManagement.BLL.Services.DTOs;

namespace StudentManagement.BLL.Interfaces
{
    public interface IAuthService
    {
        // Đăng ký tài khoản sinh viên
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDto);
        
        // Đăng nhập
        Task<(bool Success, LoginResponseDTO? Data, string Message)> LoginAsync(LoginDTO loginDto);
        
        // Lấy thông tin user hiện tại
        Task<UserDTO?> GetCurrentUserAsync(int userId);
        
        // Đăng xuất
        Task LogoutAsync(int userId);
    }
}
