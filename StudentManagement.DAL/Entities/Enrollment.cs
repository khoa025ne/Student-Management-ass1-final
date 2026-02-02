using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        
        public int? StudentId { get; set; }
        
        public int? ClassId { get; set; }
        
        public int? SemesterId { get; set; }
        
        public int? Status { get; set; } // 1: Enrolled, 2: Paid, 3: Completed, 4: Dropped, 5: Failed
        
        // Navigation Properties
        public virtual User? Student { get; set; }
        
        public virtual Class? Class { get; set; }
        
        public virtual Semester? Semester { get; set; }
        
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
