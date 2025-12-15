using AutoMapper;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;
using MyGrades.Application.Helper;
using MyGrades.Application.Contracts.Projections_Models.Grades;
using MyGrades.Application.Contracts.DTOs.Grade;

namespace MyGrades.Application.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExcelReader _excelReader;
        private readonly IExcelWriter excelWriter;
        private readonly IStudentSubjectService studentSubject;

        public GradeService(IUnitOfWork unitOfWork, IMapper mapper, 
            IExcelReader excelReader ,IExcelWriter _excelWriter,IStudentSubjectService _studentSubject)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _excelReader = excelReader;
            excelWriter = _excelWriter;
            studentSubject = _studentSubject;
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

        // تصدير قالب الدرجات إلى ملف Excel
        public async Task<Result<Stream>> ExportGradesToExcel(int subjectId)
        {
            var grades = await _unitOfWork.Grades.GetAllStudentsIdsAsync(subjectId);
            if (!grades.IsSuccess || grades.Data == null)
                return Result<Stream>.Failure(grades.Message, grades.StatusCode ?? 400);
             
            var excelStream = excelWriter.WriteGradesTemplateToStream(grades.Data);
            excelStream.Position = 0; // Reset stream position

            return Result<Stream>.Success(excelStream);
        }

        public async Task<Result<List<GradeModelData>>> GetAll(int subjectId)
        {
            var grades = await _unitOfWork.Grades.GetAllBySubjectIdAsync(subjectId);
            if (!grades.IsSuccess || grades.Data == null)
                return Result<List<GradeModelData>>.Failure(grades.Message, grades.StatusCode ?? 400);

            return Result<List<GradeModelData>>.Success(grades.Data);
        }

        public async Task<Result<Stream>> GetAllStudentsGrades(int subjectId)
        {
            var grades = await _unitOfWork.Grades.GetAllStudentsGradesAsync(subjectId);
            if (!grades.IsSuccess || grades.Data == null)
                return Result<Stream>.Failure(grades.Message, grades.StatusCode ?? 400);
             
            var excelStream = excelWriter.WriteGradesToStream(grades.Data);
            excelStream.Position = 0; // Reset stream position

            return Result<Stream>.Success(excelStream);
        }

        // في MyGrades.Application.Services.GradeService
        public async Task<Result<bool>> ImportGradesFromExcel(Stream fileStream, int subjectId)
        {
            var gradeResult = _excelReader.ReadGradesFromStream(fileStream);
            if (!gradeResult.IsSuccess)
                return Result<bool>.Failure(gradeResult.Message, gradeResult.StatusCode ?? 400);

            var newGradesData = gradeResult.Data;
            var subjectExists = await _unitOfWork.StudentSubjects.FindAsync(x=>x.SubjectId==subjectId);

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
                    int studentId = gradeData.StudentId;
                    var studentSubjectResult = await _unitOfWork.StudentSubjects
                        .GetWithIncludeAsync(ss => ss.StudentId == studentId && ss.SubjectId == subjectId, ss => ss.Grade);

                    if (!studentSubjectResult.IsSuccess || studentSubjectResult.Data == null)
                    {
                        // إذا لم يكن الطالب مسجلاً في هذه المادة، نتخطاه.
                        continue;
                    }

                    var studentSubject = studentSubjectResult.Data;

                    // 3. البحث عن درجة موجودة باستخدام StudentSubjectId
                    var existingGradeResult = await _unitOfWork.Grades
                        .GetWithIncludeAsync(g => g.StudentSubjectId == studentSubject.Id);

                    // 4. حساب المجموع الكلي (TotalScore)
                    double totalScore = gradeData.Attendance + gradeData.Tasks + gradeData.Practical;

                    if (existingGradeResult.Data != null)
                    {
                        // إذا وُجدت الدرجة: قم بالتحديث
                        var existingGrade = existingGradeResult.Data;
                        existingGrade.Attendance = gradeData.Attendance;
                        existingGrade.Tasks = gradeData.Tasks;
                        existingGrade.Practical = gradeData.Practical;
                        existingGrade.TotalScore = totalScore; // تحديث المجموع

                        await _unitOfWork.Grades.UpdateAsync(existingGrade);
                    }
                    else
                    {
                        // إذا لم توجد الدرجة: قم بالإضافة
                        var newGrade = new Grade
                        {
                            StudentSubjectId = studentSubject.Id,
                            Attendance = gradeData.Attendance,
                            Tasks = gradeData.Tasks,
                            Practical = gradeData.Practical,
                            TotalScore = totalScore
                        };
                        await _unitOfWork.Grades.AddAsync(newGrade);
                    }
                }

                // 5. الحفظ النهائي والتأكيد
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
