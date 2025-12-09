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
        public DbSet<StudentSubject> StudentSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ⚠️ هام جداً: هذا السطر ضروري لكي يعمل نظام الـ Identity بشكل صحيح
            base.OnModelCreating(builder);

            // =========================================================
            // 1. إعدادات AppUser (الرقم القومي)
            // =========================================================
            builder.Entity<AppUser>(entity =>
            {
                entity.HasIndex(u => u.NationalId).IsUnique();
                entity.Property(u => u.NationalId)
                      .IsRequired()
                      .HasMaxLength(14)
                      .IsFixedLength();

                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(150);
            });

            // =========================================================
            // 2. علاقات الـ Composition (ربط الجداول بالمستخدم) One-to-One
            // (لا يوجد تغيير هنا)
            // ============================================================
            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Assistant>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================================
            // =============== 3. علاقات Department =====================
            // (لا يوجد تغيير هنا)
            // =========================================================
            builder.Entity<Department>(entity =>
            {
                entity.HasMany(d => d.Students)
                      .WithOne(s => s.Department)
                      .HasForeignKey(s => s.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Subjects)
                      .WithOne(s => s.Department)
                      .HasForeignKey(s => s.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // 4. علاقات Many-to-Many (Doctors <-> Departments)
            // (لا يوجد تغيير هنا)
            // =========================================================
            builder.Entity<Doctor>()
                   .HasMany(d => d.Departments)
                   .WithMany(dep => dep.Doctors)
                   .UsingEntity(j => j.ToTable("DoctorDepartments"));

            // =========================================================
            // 5. إعدادات StudentSubject (جدول الربط الجديد) 🆕
            // =========================================================
            builder.Entity<StudentSubject>(entity =>
            {
                // 🚨 تحديد المفتاح الأساسي المركب (Primary Key)
                // هذا يضمن أن الطالب لا يمكن أن يكون مسجلاً في المادة نفسها مرتين
                entity.HasIndex(ss => new { ss.StudentId, ss.SubjectId }).IsUnique();

                // العلاقة Student -> StudentSubject (One-to-Many)
                entity.HasOne(ss => ss.Student)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(ss => ss.StudentId)
                      .OnDelete(DeleteBehavior.Cascade); // حذف الطالب يحذف تسجيلاته

                // العلاقة Subject -> StudentSubject (One-to-Many)
                entity.HasOne(ss => ss.Subject)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(ss => ss.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict); // منع حذف مادة مرتبطة بتسجيلات
            });

            // =========================================================
            // 6. إعدادات Subject
            // (لا يوجد تغيير في علاقاتها مع Doctor/Assistant)
            // =========================================================
            builder.Entity<Subject>(entity =>
            {
                entity.HasOne(s => s.Doctor)
                      .WithMany(d => d.Subjects)
                      .HasForeignKey(s => s.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Assistant)
                      .WithMany(a => a.Subjects)
                      .HasForeignKey(s => s.AssistantId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                // ❌ تم حذف العلاقة القديمة (Subject -> Grades) هنا لأنها أصبحت عبر StudentSubject
            });

            // =========================================================
            // 7. إعدادات Grade (الأهم) 🚀
            // =========================================================
            builder.Entity<Grade>(entity =>
            {
                // تحديد دقة الأرقام العشرية (Decimal Precision)
                entity.Property(g => g.TotalScore).HasPrecision(4, 2).IsRequired();
                entity.Property(g => g.Attendance).HasPrecision(4, 2);
                entity.Property(g => g.Tasks).HasPrecision(4, 2);
                entity.Property(g => g.Practical).HasPrecision(4, 2);

                // ⚠️ قيد هام جداً: منع تكرار الدرجة لنفس الطالب في نفس المادة
                // تم استبداله بـ StudentSubjectId
                entity.HasIndex(g => g.StudentSubjectId).IsUnique();

                // تحديد العلاقة بين Grade و StudentSubject
                entity.HasOne(g => g.StudentSubject) // تبدأ من Grade وتحدد خاصية التنقل StudentSubject
                      .WithOne(ss => ss.Grade)       // تربطها بالخاصية التنقلية Grade الموجودة في StudentSubject
                      .HasForeignKey<Grade>(g => g.StudentSubjectId) // تحدد المفتاح الخارجي (FK) في جدول Grade
                      .OnDelete(DeleteBehavior.Cascade);
                // ❌ تم حذف العلاقات القديمة (Grade -> Student) و (Grade -> Subject)
            });
        }


    }
}
