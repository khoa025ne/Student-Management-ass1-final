using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    public class Subject
    {
        public int SubjectId { get; set; }
        
        public string? SubjectCode { get; set; }
        
        public string? SubjectName { get; set; }
        
        public int Credits { get; set; } = 3;
        
        public int? Status { get; set; } // 1: Active, 2: Deprecated
        
        // Navigation Properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        
        public virtual ICollection<Prerequisite> PrerequisitesAsSubject { get; set; } = new List<Prerequisite>();
        
        public virtual ICollection<Prerequisite> PrerequisitesAsPrerequisite { get; set; } = new List<Prerequisite>();
    }
}
