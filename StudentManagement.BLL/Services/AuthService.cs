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
            // Validate dữ liệu
            if (string.IsNullOrWhiteSpace(registerDto.Email) || string.IsNullOrWhiteSpace(registerDto.Password))
                return (false, "Email và mật khẩu không được để trống");

            if (registerDto.Password != registerDto.ConfirmPassword)
                return (false, "Mật khẩu không trùng khớp");

            // Kiểm tra email đã tồn tại
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                return (false, "Email đã được đăng ký");

            // Lấy role Student
            var studentRole = await _roleRepository.FirstOrDefaultAsync(r => r.Name == "Student");
            if (studentRole == null)
                return (false, "Không tìm thấy role Student");

            // Hash mật khẩu
            var (passwordHash, passwordSalt) = HashPassword(registerDto.Password);

            // Tạo user mới
            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                DateOfBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = studentRole.RoleId,
                IsActive = true,
                IsDeleted = false,
                WalletBalance = 0
            };

            try
            {
                await _userRepository.AddAsync(newUser);
                await _userRepository.SaveChangesAsync();
                return (true, "Đăng ký thành công. Vui lòng đăng nhập.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi đăng ký: {ex.Message}");
            }
        }

        public async Task<(bool Success, LoginResponseDTO? Data, string Message)> LoginAsync(LoginDTO loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto?.Email) || string.IsNullOrWhiteSpace(loginDto?.Password))
                return (false, null, "Email và mật khẩu không được để trống");

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || user.IsDeleted)
                return (false, null, "Email hoặc mật khẩu không chính xác");

            if (!user.IsActive)
                return (false, null, "Tài khoản của bạn đã bị khóa");

            if (!VerifyPassword(loginDto.Password, user.PasswordHash ?? string.Empty, user.PasswordSalt ?? string.Empty))
                return (false, null, "Email hoặc mật khẩu không chính xác");

            var response = new LoginResponseDTO
            {
                UserId = user.UserId,
                Name = user.Name ?? string.Empty,
                Email = user.Email ?? string.Empty,
                RoleName = user.Role?.Name ?? string.Empty,
                RollNumber = user.RollNumber ?? string.Empty,
                ClassCode = user.ClassCode ?? string.Empty,
                WalletBalance = user.WalletBalance
            };

            return (true, response, "Đăng nhập thành công");
        }

        public async Task<UserDTO?> GetCurrentUserAsync(int userId)
        {
            var user = await _userRepository.GetWithRoleAsync(userId);
            if (user == null || user.IsDeleted)
                return null;

            return MapToUserDTO(user);
        }

        public async Task LogoutAsync(int userId)
        {
            await Task.CompletedTask;
        }

        // ===== PRIVATE METHODS =====

        private (string PasswordHash, string PasswordSalt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
            }
        }

        private bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            var saltBytes = Convert.FromBase64String(passwordSalt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var storedHash = Convert.FromBase64String(passwordHash);
                
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
                return true;
            }
        }

        private UserDTO MapToUserDTO(User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Phone = user.Phone ?? string.Empty,
                Address = user.Address ?? string.Empty,
                Gender = user.Gender ?? string.Empty,
                DateOfBirth = user.DateOfBirth,
                AvatarUrl = user.AvatarUrl ?? string.Empty,
                RollNumber = user.RollNumber ?? string.Empty,
                ClassCode = user.ClassCode ?? string.Empty,
                Batch = user.Batch,
                WalletBalance = user.WalletBalance,
                RoleName = user.Role?.Name ?? string.Empty,
                IsActive = user.IsActive
            };
        }
    }
}
