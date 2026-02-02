using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    public class Grade
    {
        public int GradeId { get; set; }
        
        public int? EnrollmentId { get; set; }
        
        public decimal? MidtermScore { get; set; }
        
        public decimal? FinalScore { get; set; }
        
        public decimal? GPA { get; set; }
        
        public int? Status { get; set; } // 1: Draft, 2: Published, 3: Re-evaluated
        
        // Navigation Properties
        public virtual Enrollment? Enrollment { get; set; }
        
        public virtual ICollection<AcademicAnalysis> AcademicAnalyses { get; set; } = new List<AcademicAnalysis>();
    }
}
