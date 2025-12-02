using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<AcademicLevel> AcademicLevels { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ⚠️ هام جداً: هذا السطر ضروري لكي يعمل نظام الـ Identity بشكل صحيح
            base.OnModelCreating(builder);

            // =========================================================
            // 1. إعدادات AppUser (الرقم القومي)
            // =========================================================
            builder.Entity<AppUser>(entity =>
            {
                entity.HasIndex(u => u.NationalId).IsUnique(); // منع تكرار الرقم القومي
                entity.Property(u => u.NationalId)
                      .IsRequired()
                      .HasMaxLength(14)
                      .IsFixedLength(); // إجبار الحقل أن يكون 14 خانة بالضبط

                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(150);
            });

            // =========================================================
            // 2. علاقات الـ Composition (ربط الجداول بالمستخدم) One-to-One
            // ============================================================

            // Student -> User
            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany() // المستخدم الواحد لا يرتبط بطلاب كثر، لكن هنا لضبط اتجاه العلاقة
                .HasForeignKey(s => s.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // لو حذفنا المستخدم يُحذف ملف الطالب

            // Doctor -> User
            builder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Assistant -> User
            builder.Entity<Assistant>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================================
            // =============== 3. علاقات Department =====================
            // =========================================================
            builder.Entity<Department>(entity =>
            {
                // Department -> Students (One-to-Many)
                entity.HasMany(d => d.Students)
                      .WithOne(s => s.Department)
                      .HasForeignKey(s => s.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict); // منع حذف القسم إذا كان فيه طلاب

                // Department -> Subjects (One-to-Many)
                entity.HasMany(d => d.Subjects)
                      .WithOne(s => s.Department)
                      .HasForeignKey(s => s.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // 4. علاقات Many-to-Many (Doctors <-> Departments)
            // =========================================================
            // سنترك EF Core ينشئ جدول وسيط تلقائياً (DoctorDepartment)
            builder.Entity<Doctor>()
                   .HasMany(d => d.Departments)
                   .WithMany(dep => dep.Doctors)
                   .UsingEntity(j => j.ToTable("DoctorDepartments"));

            // =========================================================
            // 5. إعدادات Subject
            // =========================================================
            builder.Entity<Subject>(entity =>
            {
                // Subject -> Doctor (One-to-Many)
                entity.HasOne(s => s.Doctor)
                      .WithMany(d => d.Subjects)
                      .HasForeignKey(s => s.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict); // لا تحذف المادة بحذف الدكتور

                // Subject -> Assistant (One-to-Many) - Optional
                entity.HasOne(s => s.Assistant)
                      .WithMany(a => a.Subjects)
                      .HasForeignKey(s => s.AssistantId)
                      .IsRequired(false) // المعيد اختياري
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // 6. إعدادات Grade (الأهم)
            // =========================================================
            builder.Entity<Grade>(entity =>
            {
                // تحديد دقة الأرقام العشرية (Decimal Precision) لتجنب التحذيرات في EF Core

                entity.Property(g => g.TotalScore)
                      .HasPrecision(4, 2) // Total 4 digits, 2 after the decimal point
                      .IsRequired();
                // إذا كنت ستستخدم التفاصيل التي ذكرناها سابقاً:
                entity.Property(g => g.Attendance).HasPrecision(4, 2);
                entity.Property(g => g.Tasks).HasPrecision(4, 2);
                entity.Property(g => g.Practical).HasPrecision(4, 2);
                //entity.Property(g => g.FinalExam).HasPrecision(4, 2);

                // ⚠️ قيد هام جداً: منع تكرار الدرجة لنفس الطالب في نفس المادة
                entity.HasIndex(g => new { g.StudentId, g.SubjectId }).IsUnique();

                // العلاقات
                entity.HasOne(g => g.Student)
                      .WithMany(s => s.Grades)
                      .HasForeignKey(g => g.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(g => g.Subject)
                      .WithMany(s => s.Grades)
                      .HasForeignKey(g => g.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict); // لا تحذف درجات بحذف المادة
            });
        }

    }
}
