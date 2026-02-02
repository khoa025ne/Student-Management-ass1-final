using System.Threading.Tasks;
using StudentManagement.DAL.Entities;

namespace StudentManagement.DAL.Repositories
{
    /// <summary>
    /// Interface cho Role Repository
    /// </summary>
    public interface IRoleRepository : IGenericRepository<Role>
    {
        /// <summary>
        /// Lấy role theo tên
        /// </summary>
        Task<Role?> GetByNameAsync(string roleName);
        
        /// <summary>
        /// Kiểm tra role đã tồn tại chưa
        /// </summary>
        Task<bool> RoleExistsAsync(string roleName);
    }
}
