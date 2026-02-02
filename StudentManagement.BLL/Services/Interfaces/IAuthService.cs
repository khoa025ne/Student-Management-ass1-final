using StudentManagement.BLL.Services.DTOs;

public interface IAuthService
{
    Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDto);
    Task<(bool Success, LoginResponseDTO? Data, string Message)> LoginAsync(LoginDTO loginDto);
    Task<UserDTO?> GetCurrentUserAsync(int userId);
    Task LogoutAsync(int userId);
}