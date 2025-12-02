using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;
using MyGrades.Application.Helper;
using System.Linq.Expressions;

namespace MyGrades.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork; // Standard naming convention with underscore
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ExcelReader _excelReader;

        public StudentService(IUnitOfWork unitOfWork,IMapper _mapper
            , UserManager<AppUser> userManager, ExcelReader excelReader)
        {
            _unitOfWork = unitOfWork; // Fixed naming
            mapper = _mapper;
            _userManager = userManager;
            _excelReader = excelReader;
        }

        public async Task<Result<bool>> ImportStudentsFromExcel(Stream fileStream
            , int academicYearId,string defaultPassword, int departmentId)
        {
            // 1. القراءة
            var studentsResult = _excelReader.ReadUsersFromStream(fileStream);
            if (!studentsResult.IsSuccess)
                return Result<bool>.Failure(studentsResult.Message, studentsResult.StatusCode ?? 400);

            var newStudentsData = studentsResult.Data;

            // 2. التحقق من القسم
            // (تأكد أن AnyAsync تدعم الـ Async، إذا لم تكن تدعمها استخدم Any فقط أو عدل الـ Repository)
            // افترض هنا أن النتيجة ترجع Result<bool>
            var departmentExists = await _unitOfWork.Departments.AnyAsync(d => d.Id == departmentId);

            if (!departmentExists.IsSuccess || newStudentsData == null
                || newStudentsData.Count == 0 || !departmentExists.Data)
            {
                return Result<bool>.Failure("Department not found");
            }

            // 3. بدء Transaction لضمان سلامة البيانات
            // ملاحظة: هذا يتطلب أن UserManager و UnitOfWork يتشاركون نفس الـ DbContext
            // أو استخدام TransactionScope
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var studentData in newStudentsData)
                {
                    // التحقق من الوجود (تجنب التكرار)
                    var existingUser = await _userManager.FindByNameAsync(studentData.NationalId);
                    if (existingUser != null) continue;

                    // إنشاء المستخدم
                    AppUser newUser = mapper.Map<AppUser>(studentData);

                    // كلمة المرور هي الرقم القومي
                    var createResult = await _userManager.CreateAsync(newUser, defaultPassword);

                    if (!createResult.Succeeded)
                    {
                        // يمكن تجميع الأخطاء وإرجاعها، أو التوقف
                        //createResult.Errors.First().Description;
                        continue;
                    }

                    // إضافة دور "Student" للمستخدم (خطوة مهمة جداً نسيته)
                    await _userManager.AddToRoleAsync(newUser, "Student");

                    // إنشاء بروفايل الطالب
                    var newStudentProfile = new Student
                    {
                        AppUserId = newUser.Id,
                        DepartmentId = departmentId,
                        AcademicYearId = academicYearId
                    };

                    await _unitOfWork.Students.AddAsync(newStudentProfile);
                }

                await _unitOfWork.SaveChangesAsync();

                // اعتماد التغييرات
                await transaction.CommitAsync(); // أو transaction.Commit() حسب الـ implementation

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // في حالة حدوث أي خطأ، يتم التراجع عن كل شيء (حتى إنشاء المستخدمين)
                await transaction.RollbackAsync();
                return Result<bool>.Failure($"Import failed: {ex.Message}");
            }
        }


        public async Task<Result> Create(StudentCreateModel student)
        {
            var studentEntity = mapper.Map<Student>(student);
            await _unitOfWork.Students.AddAsync(studentEntity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> CreateRang(List<StudentCreateModel> students)
        {
            var studentEntities = mapper.Map<List<Student>>(students);
            await _unitOfWork.Students.AddRangeAsync(studentEntities);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            var existingStudent = await _unitOfWork.Students.GetByIdAsync(id);
            if (existingStudent == null || existingStudent.Data == null)
                return Result.Failure("Student not found");

            await _unitOfWork.Students.DeleteAsync(existingStudent.Data);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<StudentModel>> GetById(int id)
        {
            var existingStudent = await _unitOfWork.Students.GetByIdAsync(id);
            if (existingStudent == null || existingStudent.Data == null)
                return Result<StudentModel>.Failure("Student not found");

            var studentModel = mapper.Map<StudentModel>(existingStudent.Data);
            return Result<StudentModel>.Success(studentModel);
        }
        

        public async Task<Result<List<StudentModel>>> GetAll()
        {
            var existingStudents = await _unitOfWork.Students
                .FindAllWithIncludeAsync(x=>x.AppUserId!=null,x=>x.User,x=>x.Grades);
            if (existingStudents == null || existingStudents.Data == null)
                return Result<List<StudentModel>>.Failure("No students found");

            var studentModels = mapper.Map<List<StudentModel>>(existingStudents.Data);
            return Result<List<StudentModel>>.Success(studentModels);
        }
        

        public async Task<Result> Update(StudentModel student)
        {
            //var existingStudent = await _unitOfWork.Students.GetByIdAsync(student.Id);
            //if (existingStudent == null || existingStudent.Data == null)
            //    return Result.Failure("Student not found");

            //mapper.Map(student, existingStudent);
            //await _unitOfWork.SaveChangesAsync();
            //return Result.Success();
            throw new NotImplementedException();
        }
        

        public async Task<Result> UpdateRang(List<StudentModel> students)
        {
            //var existingStudents = await _unitOfWork.Students.GetAllAsync();
            //if (existingStudents == null || existingStudents.Count == 0)
            //    return Result.Failure("No students found");

            //foreach (var student in students)
            //{
            //    var existingStudent = existingStudents.FirstOrDefault(s => s.Id == student.Id);
            //    if (existingStudent == null)
            //        continue;

            //    mapper.Map(student, existingStudent);
            //}

            //await _unitOfWork.SaveChangesAsync();
            //return Result.Success(existingStudents);
            throw new NotImplementedException();
        }

        public async Task<Result<StudentModel>> Find(Expression<Func<Student, bool>> expression, params Expression<Func<Student, object>>[] includes)
        {
            var student = await _unitOfWork.Students.FindAsync(expression, includes);
            if (!student.IsSuccess || student == null || student.Data == null)
                return Result<StudentModel>.Failure("Student not found");

            var studentModel = mapper.Map<StudentModel>(student.Data);
            return Result<StudentModel>.Success(studentModel);
        }

        public async Task<Result<List<StudentModel>>> FindAll(Expression<Func<Student, bool>> expression, params Expression<Func<Student, object>>[] includes)
        {
            var students = await _unitOfWork.Students.FindAllWithIncludeAsync(expression, includes);
            if (!students.IsSuccess || students == null || students.Data == null)
                return Result<List<StudentModel>>.Failure("No students found");

            var studentModels = mapper.Map<List<StudentModel>>(students.Data.ToList());
            return Result<List<StudentModel>>.Success(studentModels);
        }

        public async Task<Result> ClearAsync(Expression<Func<Student, bool>> expression)
        {
            if (expression == null)
                return Result.Failure("Expression cannot be null");
            var result = await _unitOfWork.Students.ClearAsync(expression);
            if (!result.IsSuccess)
                return Result.Failure("Failed to clear students");
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<Stream>> ExportStudentsTemplateToExcel(int departmentId)
        {
            var students = await _unitOfWork.Students.FindAllWithIncludeAsync(s => s.DepartmentId == departmentId
                                ,x=>x.User);
            if (!students.IsSuccess || students.Data == null)
                return Result<Stream>.Failure("No students found");

            var excelWriter = new ExcelWriter();

            var studentsList = mapper.Map<List<UserExcelDto>>(students.Data.ToList());
            var stream = excelWriter.WriteStudentsTemplateToStream(studentsList);
            stream.Position = 0;

            //await stream.CopyToAsync(outputStream);
            return Result<Stream>.Success(stream);
        }
    }
}
