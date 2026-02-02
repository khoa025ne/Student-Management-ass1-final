namespace StudentManagement.BLL.Services.DTOs
{
    /// <summary>
    /// DTO cho Subject
    /// </summary>
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int Status { get; set; }
    }

    /// <summary>
    /// DTO cho Class
    /// </summary>
    public class ClassDTO
    {
        public int ClassId { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int SlotId { get; set; }
        public string SlotTime { get; set; } = string.Empty;
        public int FirstDay { get; set; }
        public int SecondDay { get; set; }
        public string Room { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int CurrentEnrollment { get; set; }
        public int Status { get; set; }
    }

    /// <summary>
    /// DTO cho Enrollment
    /// </summary>
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentRollNumber { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
    }

    /// <summary>
    /// DTO cho Grade
    /// </summary>
    public class GradeDTO
    {
        public int GradeId { get; set; }
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentRollNumber { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public decimal? MidtermScore { get; set; }
        public decimal? FinalScore { get; set; }
        public decimal? GPA { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; }
    }

    /// <summary>
    /// DTO cho Semester
    /// </summary>
    public class SemesterDTO
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho Transaction
    /// </summary>
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
    }
}