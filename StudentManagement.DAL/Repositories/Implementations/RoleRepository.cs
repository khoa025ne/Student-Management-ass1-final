using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Entities;

namespace StudentManagement.DAL.Repositories
{
    /// <summary>
    /// Repository implementation cho Role entity
    /// </summary>
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Lấy role theo tên
        /// </summary>
        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        /// <summary>
        /// Kiểm tra role đã tồn tại chưa
        /// </summary>
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _dbSet.AnyAsync(r => r.Name == roleName && r.Status == 1);
        }
    }
}
