using AutoMapper;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> Create(SubjectDto subject)
        {
            var subjectEntity = mapper.Map<Subject>(subject);
            await unitOfWork.Subjects.AddAsync(subjectEntity);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> CreateRange(List<SubjectDto> subjects)
        {
            var subjectEntities = mapper.Map<List<Subject>>(subjects);
            // Call the repository to add the subjects
            await unitOfWork.Subjects.AddRangeAsync(subjectEntities);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            var subject = await unitOfWork.Subjects.GetByIdAsync(id);
            if (subject == null || subject.Data == null)
            {
                return Result.Failure("Subject not found.");
            }
            await unitOfWork.Subjects.DeleteAsync(subject.Data);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<Subject>> GetById(int id)
        {
            var subject = await unitOfWork.Subjects.GetByIdAsync(id);
            if (subject == null || subject.Data == null)
            {
                return Result<Subject>.Failure("Subject not found.");
            }

            return Result<Subject>.Success(subject.Data);
        
        }

        public async Task<Result<List<SubjectDto>>> GetAll()
        {
            var subjects = await unitOfWork.Subjects.GetAllAsync();
            var subjectDtos = mapper.Map<List<SubjectDto>>(subjects.Data);
            return Result<List<SubjectDto>>.Success(subjectDtos);
        }

        public async Task<Result<Subject>> Update(UpdateSubjectDto subjectDto)
        {
            var subject = await unitOfWork.Subjects.GetByIdAsync(subjectDto.Id);
            if (subject == null||subject.Data==null)
            {
                return Result<Subject>.Failure("Subject not found.");
            }
            await unitOfWork.SaveChangesAsync();
            return Result<Subject>.Success(subject.Data);
        }

        public async Task<Result<List<Subject>>> UpdateRange(List<UpdateSubjectDto> subjects)
        {
            //var subjectEntities = await unitOfWork.Subjects.GetRangeAsync(subjects.Select(s => s.Id));
            //if (subjectEntities.Count != subjects.Count)
            //{
            //    return Result<List<Subject>>.Failure("Some subjects not found.");
            //}

            //// Map the updated properties from the DTOs to the entities
            //for (int i = 0; i < subjects.Count; i++)
            //{
            //    mapper.Map(subjects[i], subjectEntities[i]);
            //}

            //await unitOfWork.SaveChangesAsync();
            //var updatedSubjectDtos = mapper.Map<List<Subject>>(subjectEntities);
            //return Result<List<Subject>>.Success(updatedSubjectDtos);
            throw new NotImplementedException();
        }

        public async Task<Result<List<UpdateSubjectDto>>> FindAllAsync(
            Expression<Func<Subject, bool>> predicate)
        {
            var subjects = await unitOfWork.Subjects.FindAllAsync(predicate);
            var subjectDtos = mapper.Map<List<UpdateSubjectDto>>(subjects.Data);
            return Result<List<UpdateSubjectDto>>.Success(subjectDtos);
        }

        public async Task<Result> ClearAsync(Expression<Func<Subject, bool>> predicate)
        {
            var result= await unitOfWork.Subjects.ClearAsync(predicate);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Message);
            }
            await unitOfWork.SaveChangesAsync();
            return Result.Success();

        }

        public async Task<Result<List<SubjectModel>>> GetUserSubjectsAsync(Expression<Func<Subject, bool>> predicate)
        {
            var subjects = await unitOfWork.Subjects.GetUserSubjectsAsync(predicate);
            if (!subjects.IsSuccess || subjects.Data == null)
            {
                return Result<List<SubjectModel>>.Failure(subjects.Message);
            }
            return Result<List<SubjectModel>>.Success(subjects.Data);
        }
    }
}
