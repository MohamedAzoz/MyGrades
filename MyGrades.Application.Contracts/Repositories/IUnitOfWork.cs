using Microsoft.EntityFrameworkCore.Storage;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<AppUser> Users { get; }
        public IAssistantRepository Assistants { get; }
        public IGenericRepository<AcademicLevel> AcademicLevels { get; }
        public IGenericRepository<Subject> Subjects { get; }
        public IDepartmentRepository Departments { get; }
        public IGradeRepository Grades { get; }
        public IGenericRepository<Student> Students { get; }
        public IGenericRepository<Doctor> Doctors { get; }
        public Task<IDbContextTransaction> BeginTransactionAsync();

        public Task SaveChangesAsync();
    }
}
