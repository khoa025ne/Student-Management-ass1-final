using System;

namespace StudentManagement.DAL.Entities
{
    /// <summary>
    /// Entity định nghĩa môn tiên quyết (Prerequisite)
    /// VD: Muốn học PRO192 phải hoàn thành PRF192 trước
    /// </summary>
    public class Prerequisite
    {
        /// <summary>
        /// ID môn học chính
        /// </summary>
        public int SubjectId { get; set; }

        /// <summary>
        /// ID môn tiên quyết (phải học trước)
        /// </summary>
        public int PreSubjectId { get; set; }

        /// <summary>
        /// Trạng thái: 1 = Active, 0 = Removed
        /// </summary>
        public int? Status { get; set; } = 1; // ✅ MỚI THÊM

        // 🔗 Navigation Properties
        public Subject? Subject { get; set; }
        public Subject? PreSubject { get; set; }
    }
}
