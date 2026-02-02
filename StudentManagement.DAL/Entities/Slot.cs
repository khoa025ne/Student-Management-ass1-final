using System;
using System.Collections.Generic;

namespace StudentManagement.DAL.Entities
{
    public class Slot
    {
        public int SlotId { get; set; }
        
        public TimeSpan? StartTime { get; set; }
        
        public TimeSpan? EndTime { get; set; }
        
        public int? Status { get; set; } // 1: Active, 0: Inactive
        
        // Navigation Properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
