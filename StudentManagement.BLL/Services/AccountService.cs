using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.BLL.Services.DTOs;
using StudentManagement.BLL.Services.Interfaces;
using StudentManagement.DAL.Entities;
using StudentManagement.DAL.Repositories;

namespace StudentManagement.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;

        public AccountService(IUserRepository userRepository, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<(bool Success, UserDTO? Data, string Message)> CreateAccountAsync(CreateAccountDTO createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.Email) || string.IsNullOrWhiteSpace(createDto.Password))
                return (false, null, "Email và mật khẩu không được để trống");

            if (await _userRepository.EmailExistsAsync(createDto.Email))
                return (false, null, "Email đã được sử dụng");

            if (createDto.RoleId == 1)
                return (false, null, "Không thể tạo thêm tài khoản Admin hệ thống");

            var role = await _roleRepository.GetByIdAsync(createDto.RoleId);
            if (role == null)
                return (false, null, "Quyền hạn (Role) không tồn tại");

            var (passwordHash, passwordSalt) = HashPassword(createDto.Password);

            var newUser = new User
            {
                Name = createDto.Name,
                FullName = createDto.Name,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Gender = createDto.Gender,
                DateOfBirth = createDto.DateOfBirth,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = createDto.RoleId,
                Batch = createDto.Batch,
                IsActive = true,
                IsDeleted = false,
                WalletBalance = 0,
                CreatedAt = DateTime.UtcNow
            };

            // Logic dành riêng cho sinh viên
            if (role.Name == "Student")
            {
                newUser.RollNumber = await GenerateRollNumberAsync(createDto.Batch ?? DateTime.Now.Year);
                newUser.ClassCode = $"SE{createDto.Batch ?? DateTime.Now.Year}01";
            }

            try
            {
                await _userRepository.AddAsync(newUser);
                await _userRepository.SaveChangesAsync();
                return (true, MapToUserDTO(newUser), "Tạo tài khoản thành công");
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, UserDTO? Data, string Message)> UpdateProfileAsync(int userId, UpdateProfileDTO updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return (false, null, "Người dùng không tồn tại");

            user.Name = updateDto.Name ?? user.Name;
            user.Phone = updateDto.Phone ?? user.Phone;
            user.Address = updateDto.Address ?? user.Address;
            user.Gender = updateDto.Gender ?? user.Gender;
            user.DateOfBirth = updateDto.DateOfBirth ?? user.DateOfBirth;
            user.AvatarUrl = updateDto.AvatarUrl ?? user.AvatarUrl;
            user.UpdatedAt = DateTime.UtcNow;

            try
            {
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                var userWithRole = await _userRepository.GetWithRoleAsync(userId);
                return (true, MapToUserDTO(userWithRole), "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi cập nhật: {ex.Message}");
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.FindAsync(u => !u.IsDeleted);
            return users.Select(MapToUserDTO).ToList();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetWithRoleAsync(userId);
            return (user == null || user.IsDeleted) ? null : MapToUserDTO(user);
        }

        public async Task<(bool Success, string Message)> ToggleUserStatusAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return (false, "Không tìm thấy người dùng");

            if (user.RoleId == 1) return (false, "Không thể thao tác trên tài khoản Admin");

            user.IsActive = !user.IsActive;
            user.Status = user.IsActive ? 1 : 0;

            await _userRepository.SaveChangesAsync();
            return (true, user.IsActive ? "Đã mở khóa tài khoản" : "Đã khóa tài khoản");
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return (false, "Người dùng không tồn tại");

            if (user.RoleId == 1) return (false, "Không thể xóa tài khoản Admin duy nhất");

            user.IsDeleted = true;
            await _userRepository.SaveChangesAsync();
            return (true, "Xóa tài khoản thành công");
        }

        public async Task<bool> EmailExistsAsync(string email) => await _userRepository.EmailExistsAsync(email);

        private async Task<string> GenerateRollNumberAsync(int batch)
        {
            var existing = await _userRepository.FindAsync(u => u.RollNumber != null && u.RollNumber.Contains(batch.ToString()));
            return $"SE{batch}{(existing.Count() + 1):D4}";
        }

        private (string Hash, string Salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            return (Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))),
                    Convert.ToBase64String(hmac.Key));
        }

        private UserDTO MapToUserDTO(User user)
        {
            if (user == null) return new UserDTO();
            return new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name ?? user.FullName ?? string.Empty,
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
                RoleName = user.Role?.Name ?? "N/A",
                IsActive = user.IsActive
            };
        }
    }
}