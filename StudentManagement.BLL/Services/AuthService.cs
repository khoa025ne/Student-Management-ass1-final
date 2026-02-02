using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.BLL.Services.DTOs;
using StudentManagement.BLL.Services.Interfaces;
using StudentManagement.DAL.Entities;
using StudentManagement.DAL.Repositories;

namespace StudentManagement.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;

        public AuthService(IUserRepository userRepository, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
                return (false, "Mật khẩu xác nhận không khớp");

            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                return (false, "Email này đã được đăng ký");

            var studentRole = await _roleRepository.FirstOrDefaultAsync(r => r.Name == "Student");
            var (hash, salt) = HashPassword(registerDto.Password);

            var newUser = new User
            {
                Name = registerDto.Name,
                FullName = registerDto.Name,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = studentRole?.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _userRepository.AddAsync(newUser);
                await _userRepository.SaveChangesAsync();
                return (true, "Đăng ký thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi đăng ký: {ex.Message}");
            }
        }

        public async Task<(bool Success, LoginResponseDTO? Data, string Message)> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || user.IsDeleted) return (false, null, "Email hoặc mật khẩu không đúng");

            if (!user.IsActive || user.Status == 0) return (false, null, "Tài khoản của bạn đang bị khóa");

            bool isValid = VerifyPassword(loginDto.Password, user.PasswordHash!, user.PasswordSalt!);
            if (!isValid) return (false, null, "Email hoặc mật khẩu không đúng");

            return (true, new LoginResponseDTO
            {
                UserId = user.UserId,
                FullName = user.FullName ?? user.Name,
                Email = user.Email,
                RoleName = user.Role?.Name ?? "User",
                RollNumber = user.RollNumber ?? "",
                WalletBalance = user.WalletBalance
            }, "Đăng nhập thành công");
        }

        public async Task<UserDTO?> GetCurrentUserAsync(int userId)
        {
            var user = await _userRepository.GetWithRoleAsync(userId);
            return (user == null || !user.IsActive) ? null : MapToUserDTO(user);
        }

        public async Task LogoutAsync(int userId) => await Task.CompletedTask;

        private (string Hash, string Salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            return (Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))),
                    Convert.ToBase64String(hmac.Key));
        }

        private bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == hash;
        }

        private UserDTO MapToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.UserId,
                Name = user.Name ?? user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                RoleName = user.Role?.Name,
                IsActive = user.IsActive
            };
        }
    }
}