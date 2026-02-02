using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Entities;

namespace StudentManagement.DAL
{
    /// <summary>
    /// DbContext chính của ứng dụng quản lý sinh viên FPT (Đã tối ưu hoàn toàn cho MySQL)
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 🔗 DbSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<AcademicAnalysis> AcademicAnalyses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ CẤU HÌNH ROLE
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Status)
                    .HasDefaultValue(1);

                // ĐÃ SỬA: Thêm (6) để khớp với độ chính xác datetime(6) của MySQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");
            });

            // ✅ CẤU HÌNH USER
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("UX_Users_Email");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RollNumber)
                    .HasMaxLength(20);

                // ĐÃ SỬA: MySQL cho phép nhiều giá trị NULL trong Unique Index nên không cần HasFilter
                entity.HasIndex(e => e.RollNumber)
                    .IsUnique()
                    .HasDatabaseName("UX_Users_RollNumber");

                entity.Property(e => e.WalletBalance)
                    .HasPrecision(15, 2)
                    .HasDefaultValue(0);

                entity.Property(e => e.Status)
                    .HasDefaultValue(1);

                // ĐÃ SỬA: Thêm (6) cho MySQL
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt");

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            // ✅ CẤU HÌNH PREREQUISITE (Đã khớp với Subject.cs)
            modelBuilder.Entity<Prerequisite>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.PreSubjectId });

                entity.Property(e => e.Status)
                    .HasDefaultValue(1);

                entity.HasOne(p => p.Subject)
                    .WithMany(s => s.PrerequisitesAsSubject)
                    .HasForeignKey(p => p.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Prerequisites_Subjects");

                entity.HasOne(p => p.PreSubject)
                    .WithMany(s => s.PrerequisitesAsPrerequisite)
                    .HasForeignKey(p => p.PreSubjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Prerequisites_PreSubjects");
            });

            // ✅ CẤU HÌNH CLASS
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassId);
                entity.Property(e => e.ClassCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Capacity).HasDefaultValue(30);
                entity.Property(e => e.Status).HasDefaultValue(1);

                entity.HasOne(c => c.Subject)
                    .WithMany(s => s.Classes)
                    .HasForeignKey(c => c.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Teacher)
                    .WithMany(u => u.ClassesTeaching)
                    .HasForeignKey(c => c.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ✅ CẤU HÌNH ACADEMICANALYSIS
            modelBuilder.Entity<AcademicAnalysis>(entity =>
            {
                entity.HasKey(e => e.AnalysisId);
                entity.Property(e => e.AI_Feedback).HasColumnType("longtext");
                entity.Property(e => e.Status).HasDefaultValue(1);

                // ĐÃ SỬA: Thêm (6) cho MySQL
                entity.Property(e => e.AnalysisDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            });

            // ✅ CẤU HÌNH TRANSACTION
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.Amount).HasPrecision(15, 2);
                entity.Property(e => e.Status).HasDefaultValue(1);

                // ĐÃ SỬA: Thêm (6) cho MySQL
                entity.Property(e => e.TransactionDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}