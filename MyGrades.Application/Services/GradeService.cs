using AutoMapper;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;
using MyGrades.Application.Helper;
using MyGrades.Application.Contracts.Projections_Models.Grades;

namespace MyGrades.Application.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ExcelReader _excelReader;

        public GradeService(IUnitOfWork unitOfWork, IMapper mapper, ExcelReader excelReader)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _excelReader = excelReader;
        }
        public async Task<Result> Create(Grade grade)
        {
            if (grade == null)
                return Result.Failure("Invalid grade");

            await _unitOfWork.Grades.AddAsync(grade);
            return Result.Success();
        }

        public async Task<Result> CreateRang(List<Grade> grades)
        {
            if (grades == null || grades.Count == 0)
                return Result.Failure("No grades provided");

            await _unitOfWork.Grades.AddRangeAsync(grades);
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            var existingGrade = await _unitOfWork.Grades.GetByIdAsync(id);
            if (!existingGrade.IsSuccess || existingGrade.Data == null)
                return Result.Failure("Grade not found");

            await _unitOfWork.Grades.DeleteAsync(existingGrade.Data);
            return Result.Success();
        }

        public async Task<Result<List<GradeModel>>> GetAll(int subjectId)
        {
            var grades = await _unitOfWork.Grades.GetAllBySubjectIdAsync(subjectId);
            if (!grades.IsSuccess || grades.Data == null)
                return Result<List<GradeModel>>.Failure(grades.Message, grades.StatusCode ?? 400);

            var gradeModels = (grades.Data);
            return Result<List<GradeModel>>.Success(gradeModels);
        }

        public async Task<Result<bool>> ImportGradesFromExcel(Stream fileStream, int subjectId)
        {
            var gradeResult = _excelReader.ReadGradesFromStream(fileStream);
            if (!gradeResult.IsSuccess)
                return Result<bool>.Failure(gradeResult.Message, gradeResult.StatusCode ?? 400);

            var newGradesData = gradeResult.Data;

            var subjectExists = await _unitOfWork.Subjects.GetByIdAsync(subjectId);

            if (!subjectExists.IsSuccess || newGradesData == null
                || newGradesData.Count == 0 || subjectExists.Data == null)
            {
                return Result<bool>.Failure("Subject not found");
            }
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var gradeData in newGradesData)
                {
                    // 1. تحويل البيانات وتعيين SubjectId
                    // نحن نفترض أن gradeData يحتوي على StudentId بعد تعديل الـ ExcelReader
                    Grade gradeToProcess = _mapper.Map<Grade>(gradeData);
                    gradeToProcess.SubjectId = subjectId;

                    // 2. حساب المجموع الكلي (TotalScore)
                    gradeToProcess.TotalScore = gradeToProcess.Attendance +
                                                 gradeToProcess.Tasks +
                                                 gradeToProcess.Practical;

                    // 3. البحث عن درجة موجودة لنفس الطالب في نفس المادة (للتحديث)
                    // ملاحظة: ستحتاج لإنشاء دالة البحث هذه في GradesRepository
                    var existingGradeResult = await _unitOfWork.Grades
                        .GetWithIncludeAsync(x => x.StudentId == gradeToProcess.StudentId 
                                        && x.SubjectId == subjectId);

                    if (existingGradeResult.Data != null)
                    {
                        // إذا وُجدت الدرجة: قم بالتحديث
                        var existingGrade = existingGradeResult.Data;
                        existingGrade.Attendance = gradeToProcess.Attendance;
                        existingGrade.Tasks = gradeToProcess.Tasks;
                        existingGrade.Practical = gradeToProcess.Practical;
                        existingGrade.TotalScore = gradeToProcess.TotalScore; // تحديث المجموع

                        await _unitOfWork.Grades.UpdateAsync(existingGrade); // ⚠️ افترضنا وجود دالة Update
                    }
                    else
                    {
                        // إذا لم توجد الدرجة: قم بالإضافة
                        await _unitOfWork.Grades.AddAsync(gradeToProcess);
                    }
                }

                // 4. الحفظ النهائي للتغييرات
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // سجل الخطأ بالتفصيل في الـ Logger 
                //_logger.LogError(ex, $"Grade import failed for subject ID {subjectId}");
                return Result<bool>.Failure($"Import failed: {ex.Message}");
            }
        }
        

        public async Task<Result<Grade>> Update(Grade grade)
        {
           throw new NotImplementedException();
        }

        public async Task<Result<List<Grade>>> UpdateRang(List<Grade> grades)
        {
            //if (grades == null || grades.Count == 0)
            //    return Result<List<Grade>>.Failure("No grades provided");

            //var updatedGrades = new List<Grade>();
            //foreach (var grade in grades)
            //{
            //    var existingGrade = await _unitOfWork.Grades.GetByIdAsync(grade.Id);
            //    if (!existingGrade.IsSuccess || existingGrade.Data == null)
            //    {
            //        return Result<List<Grade>>.Failure($"Grade not found: {grade.Id}");
            //    }

            //    existingGrade.Data.Attendance = grade.Attendance;
            //    existingGrade.Data.Tasks = grade.Tasks;
            //    existingGrade.Data.Practical = grade.Practical;
            //    existingGrade.Data.TotalScore = grade.TotalScore;

            //    await _unitOfWork.Grades.UpdateAsync(existingGrade.Data);
            //    updatedGrades.Add(existingGrade.Data);
            //}

            //await _unitOfWork.SaveChangesAsync();
            //return Result<List<Grade>>.Success(updatedGrades);
              throw new NotImplementedException();
        }
    }
}
