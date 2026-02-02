using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    public class Semester
    {
        public int SemesterId { get; set; }
        
        public string? SemesterName { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public int? Status { get; set; } // 1: Upcoming, 2: Ongoing, 3: Finished, 0: Closed
        
        // Navigation Properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
