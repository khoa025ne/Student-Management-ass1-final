using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.BLL.Services.DTOs; // <--- Phải có dòng này!

namespace StudentManagement.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, UserDTO? Data, string Message)> CreateAccountAsync(CreateAccountDTO createDto);

        // Sau khi thêm DTO ở bước 1, dòng này sẽ hết lỗi đỏ
        Task<(bool Success, UserDTO? Data, string Message)> UpdateProfileAsync(int userId, UpdateProfileDTO updateDto);

        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task<(bool Success, string Message)> ToggleUserStatusAsync(int userId);
        Task<(bool Success, string Message)> DeleteUserAsync(int userId);
        Task<bool> EmailExistsAsync(string email);
    }
}