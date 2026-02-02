using System;

namespace StudentManagement.DAL.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; } // Nên để int (không null) vì giao dịch phải có chủ
        public int? SemesterId { get; set; }
        public decimal Amount { get; set; } // DTO dùng decimal, nên để decimal ở đây
        public string TransactionType { get; set; } = string.Empty;
        public int Status { get; set; } = 1; // 1: Success, 0: Failed...
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User? User { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
