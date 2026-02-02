using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Entities;

namespace StudentManagement.DAL.Repositories
{
    /// <summary>
    /// Repository implementation for User entity
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Lấy user theo email
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Lấy user theo mã sinh viên
        /// </summary>
        public async Task<User?> GetByRollNumberAsync(string rollNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RollNumber == rollNumber);
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa (không tính đã xóa)
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email && !u.IsDeleted);
        }

        /// <summary>
        /// Lấy user kèm thông tin Role
        /// </summary>
        public async Task<User?> GetWithRoleAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
