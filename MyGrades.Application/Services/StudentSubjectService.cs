using AutoMapper;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Services
{
    public class StudentSubjectService : IStudentSubjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;    
        public StudentSubjectService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<Result> AddRangeStudentSubject(List<CreateStudentSubjectDto> createStudentSubjects)
        {
            var studentSubjects = mapper.Map<List<StudentSubject>>(createStudentSubjects);
            await unitOfWork.StudentSubjects.AddRangeAsync(studentSubjects);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> CreateStudentSubject(CreateStudentSubjectDto createStudentSubject)
        {
            var studentSubject = mapper.Map<StudentSubject>(createStudentSubject);
            await unitOfWork.StudentSubjects.AddAsync(studentSubject);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteStudentSubject(CreateStudentSubjectDto createStudentSubject)
        {
            var studentSubject = mapper.Map<StudentSubject>(createStudentSubject);
            await unitOfWork.StudentSubjects.DeleteAsync(studentSubject);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> EnrollStudentsInSubjects(CreateYearDepartmentDto createYearDepartment)
        {
            var students = await unitOfWork.Students.FindAllAsync(x=>x.AcademicLevelId == createYearDepartment.YearId&&
            x.DepartmentId == createYearDepartment.DepartmentId);
            var subjects = await unitOfWork.Subjects.FindAllAsync(x=>x.AcademicLevelId == createYearDepartment.YearId && 
            x.DepartmentId == createYearDepartment.DepartmentId);
            if(!students.IsSuccess && !subjects.IsSuccess && students.Data.Count > 0 && subjects.Data.Count > 0)
            {
                return Result.Failure("No students or subjects found for the specified year and department.",404);
            }
            var studentSubjects = new List<StudentSubject>();
            foreach(var student in students.Data)
            {
                foreach(var subject in subjects.Data)
                {
                    studentSubjects.Add(new StudentSubject
                    {
                        StudentId = student.Id,
                        SubjectId = subject.Id
                    });
                }
            }
            await unitOfWork.StudentSubjects.AddRangeAsync(studentSubjects);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
