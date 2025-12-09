using Microsoft.EntityFrameworkCore.Storage;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<AppUser> Users { get; }
        public IAssistantRepository Assistants { get; }
        public IGenericRepository<AcademicLevel> AcademicLevels { get; }
        public ISubjectRepository Subjects { get; }
        public IDepartmentRepository Departments { get; }
        public IGradeRepository Grades { get; }
        public IStudentRepository Students { get; }
        public IGenericRepository<StudentSubject> StudentSubjects { get; }
        public IDoctorRepository Doctors { get; }
        public Task<IDbContextTransaction> BeginTransactionAsync();

        public Task SaveChangesAsync();
    }
}
