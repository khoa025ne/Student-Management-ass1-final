using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    /// <summary>
    /// Định nghĩa các vai trò trong hệ thống (Admin, Manager, Student, Teacher)
    /// 
    /// ⚠️ CHÚ Ý: Phân quyền được quản lý bằng LOGIC (RolePermissionConfig), 
    /// KHÔNG lưu trữ trong database
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }

        /// <summary>
        /// Tên vai trò: Admin, Manager, Student, Teacher
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả vai trò
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Trạng thái: 1 = Active, 0 = Inactive
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// Ngày cập nhật lần cuối
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Navigation: Danh sách người dùng có vai trò này
        /// </summary>
        public ICollection<User> Users { get; set; } = new HashSet<User>();

        // ❌ LOẠI BỎ: Phân quyền không lưu trong DB nữa
        // public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}