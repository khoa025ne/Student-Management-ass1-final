using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    public class Class
    {
        public int ClassId { get; set; }
        
        public string? ClassCode { get; set; }
        
        public int? SubjectId { get; set; }
        
        public int? TeacherId { get; set; }
        
        public int? SemesterId { get; set; }
        
        public int? SlotId { get; set; }
        
        public int? FirstDay { get; set; } // 2-7 (Thứ 2 - Thứ 7)
        
        public int? SecondDay { get; set; }
        
        public string? Room { get; set; }
        
        public int Capacity { get; set; } = 30;
        
        public int? Status { get; set; } // 1: Open, 2: Ongoing, 3: Ended, 0: Cancelled
        
        // Navigation Properties
        public virtual Subject? Subject { get; set; }
        
        public virtual User? Teacher { get; set; }
        
        public virtual Semester? Semester { get; set; }
        
        public virtual Slot? Slot { get; set; }
        
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
