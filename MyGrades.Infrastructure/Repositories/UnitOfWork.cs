using Microsoft.EntityFrameworkCore.Storage;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
            Users = new GenericRepository<AppUser>(_context);
            Assistants = new AssistantRepository(_context);
            AcademicLevels = new GenericRepository<AcademicLevel>(_context);
            Subjects = new SubjectRepository(_context);
            Departments = new DepartmentRepository(_context);
            Grades = new GradeRepository(_context);
            Students = new StudentRepository(_context);
            Doctors = new DoctorRepository(_context);
            StudentSubjects = new GenericRepository<StudentSubject>(_context);
        }
        public IGenericRepository<AppUser> Users { get; private set; }

        public IAssistantRepository Assistants { get; private set; }

        public IGenericRepository<AcademicLevel> AcademicLevels { get; private set; }

        public ISubjectRepository Subjects { get; private set; }

        public IDepartmentRepository Departments { get; private set; }

        public IGradeRepository Grades { get; private set; }

        public IStudentRepository Students { get; private set; }

        public IDoctorRepository Doctors { get; private set; }

        public IGenericRepository<StudentSubject> StudentSubjects { get; private set; }


        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }
        public async Task SaveChangesAsync()
        {
             await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
        }
        

    }
}
