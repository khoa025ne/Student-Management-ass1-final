using System;

namespace StudentManagement.DAL.Entities
{
    /// <summary>
    /// Entity AcademicAnalysis - Phân tích học tập từ AI
    /// </summary>
    public class AcademicAnalysis
    {
        public int AnalysisId { get; set; }
        
        public int GradeId { get; set; }
        
        /// <summary>
        /// Nhận xét từ AI về kết quả học tập
        /// </summary>
        public string? AI_Feedback { get; set; }
        
        /// <summary>
        /// 1: New, 2: Read by student
        /// </summary>
        public int? Status { get; set; }
        
        public DateTime AnalysisDate { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual Grade? Grade { get; set; }
    }
}
