using System;
using System.Collections.Generic;
namespace StudentManagement.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty; // Khớp với AccountService
        public string FullName { get; set; } = string.Empty; // Khớp với LoginResponse
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Cho login đơn giản
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? RollNumber { get; set; }
        public string? ClassCode { get; set; }
        public int? Batch { get; set; }
        public decimal WalletBalance { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? Status { get; set; } = 1; // 1: Active, 0: Inactive
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public virtual ICollection<Class> ClassesTeaching { get; set; } = new HashSet<Class>();
    }
}