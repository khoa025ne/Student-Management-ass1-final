using System.Threading.Tasks;
using StudentManagement.DAL.Entities;

namespace StudentManagement.DAL.Repositories
{
    /// <summary>
    /// Interface cho User Repository
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRollNumberAsync(string rollNumber);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetWithRoleAsync(int userId);
    }
}
