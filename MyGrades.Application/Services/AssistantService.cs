using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts.Const;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Application.Contracts.Projections_Models.User.Assistants;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Application.Helper;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Services
{
    public class AssistantService : IAssistantService
    {
        private readonly IUnitOfWork _unitOfWork; // Standard naming convention with underscore
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IExcelReader _excelReader;

        public AssistantService(IUnitOfWork unitOfWork, IMapper _mapper
            , UserManager<AppUser> userManager, IExcelReader excelReader)
        {
            _unitOfWork = unitOfWork; // Fixed naming
            mapper = _mapper;
            _userManager = userManager;
            _excelReader = excelReader;
        }

        public async Task<Result<AssistantModel>> AddAsync(AssistantExcelDto assistant)
        {
            var assistantEntity = mapper.Map<Assistant>(assistant);
            await _unitOfWork.Assistants.AddAsync(assistantEntity);
            await _unitOfWork.SaveChangesAsync();
            return Result<AssistantModel>.Success(mapper.Map<AssistantModel>(assistantEntity));
        }

        public async Task<Result<List<AssistantModel>>> AddRangeAsync(List<AssistantExcelDto> assistants)
        {
            var assistantEntities = mapper.Map<List<Assistant>>(assistants);
            await _unitOfWork.Assistants.AddRangeAsync(assistantEntities);
            await _unitOfWork.SaveChangesAsync();
            var assistantModels = mapper.Map<List<AssistantModel>>(assistantEntities);
            return Result<List<AssistantModel>>.Success(assistantModels);
        }

        public async Task<Result> DeleteAsync(int Id)
        {
            var assistant = await _unitOfWork.Assistants.GetByIdAsync(Id);
            if (assistant == null || assistant.Data == null)
                return Result.Failure("Assistant not found");

            await _unitOfWork.Assistants.DeleteAsync(assistant.Data);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<AssistantModelData>> GetByNationalIdAsync(string nationalId)
        {
            var assistant = await _unitOfWork.Assistants.GetByNationalIdAsync(nationalId);
            if (assistant == null || assistant.Data == null)
                return Result<AssistantModelData>.Failure("Assistant not found");
            var assistantModel = mapper.Map<AssistantModelData>(assistant.Data);
            return Result<AssistantModelData>.Success(assistantModel);
        }

        public async Task<Result<List<AssistantProjection>>> FindAllAsync(int departmentId)
        {
            var result = await _unitOfWork.Assistants.FindAllByDepartmentIdAsync(departmentId);
            if (result == null || result.Data == null)
                return Result<List<AssistantProjection>>.Failure("Assistant not found");
            result.Message = "Assistants retrieved successfully";
            return Result<List<AssistantProjection>>.Success(result.Data);
        }

        public async Task<Result<List<AssistantModel>>> GetAllAsync()
        {
            var assistants = await _unitOfWork.Assistants.FindAllAssistantsAsync();
            if (assistants == null || assistants.Data == null)
                return Result<List<AssistantModel>>.Failure("No assistants found");

            return Result<List<AssistantModel>>.Success(assistants.Data);
        }

        //public async Task<Result<List<SubjectModel>>> GetAllSubjectsAsync(int assistantId)
        //{
        //    var subjects = await _unitOfWork.Subjects.FindAllByAssistantIdAsync(assistantId);
        //    if (subjects == null || subjects.Data == null)
        //        return Result<List<SubjectModel>>.Failure("No subjects found");

        //    return Result<List<SubjectModel>>.Success(subjects.Data);
        //}

        public async Task<Result<AssistantModelData>> GetByIdAsync(int id)  
        {
            var assistant = await _unitOfWork.Assistants.GetAssistantByIdAsync(id);
            if (assistant == null)
                return Result<AssistantModelData>.Failure("Assistant not found");

            //var assistantModel = mapper.Map<AssistantModel>(assistant.Data);
            return Result<AssistantModelData>.Success(assistant.Data);
        }

        //public async Task<Result<bool>> ImportAssistantsFromExcel(Stream fileStream, string defaultPassword)
        //{
        //    var assistantsResult = _excelReader.ReadAssistantFromStream(fileStream);
        //    if (!assistantsResult.IsSuccess||assistantsResult.Data==null)
        //        return Result<bool>.Failure(assistantsResult.Message, assistantsResult.StatusCode ?? 400);

        //    var newAssistantsData = assistantsResult.Data;

        //    using var transaction = await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        foreach (var assistantData in newAssistantsData)
        //        {
        //            // التحقق من الوجود (تجنب التكرار)
        //            var existingUser = await _userManager.FindByNameAsync(assistantData.NationalId);
        //            if (existingUser != null) continue;

        //            var departmentExists = await _unitOfWork.Departments.AnyAsync(d => d.Id == assistantData.DepartmentId);

        //            if (!departmentExists.IsSuccess || newAssistantsData == null
        //                || newAssistantsData.Count == 0 || !departmentExists.Data)
        //            {
        //                //return Result<bool>.Failure("Department not found");
        //                continue;
        //            }
        //            // إنشاء المستخدم
        //            AppUser newUser = mapper.Map<AppUser>(assistantData);

        //            var createResult = await _userManager.CreateAsync(newUser, defaultPassword);

        //            if (!createResult.Succeeded)
        //            {
        //                // يمكن تجميع الأخطاء وإرجاعها، أو التوقف
        //                //createResult.Errors.First().Description;
        //                continue;
        //            }

        //            // إضافة دور "Assistant" للمستخدم (خطوة مهمة جداً نسيته)
        //            await _userManager.AddToRoleAsync(newUser, "Assistant");

        //            var newAssistantProfile = new Assistant
        //            {
        //                AppUserId = newUser.Id,
        //                DepartmentId = assistantData.DepartmentId
        //            };

        //            await _unitOfWork.Assistants.AddAsync(newAssistantProfile);
        //        }

        //        await _unitOfWork.SaveChangesAsync();

        //        // اعتماد التغييرات
        //        await transaction.CommitAsync(); // أو transaction.Commit() حسب الـ implementation

        //        return Result<bool>.Success(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        // في حالة حدوث أي خطأ، يتم التراجع عن كل شيء (حتى إنشاء المستخدمين)
        //        await transaction.RollbackAsync();
        //        return Result<bool>.Failure($"Import failed: {ex.Message}");
        //    }
        //}

        public async Task<Result<bool>> ImportAssistantsFromExcel(Stream fileStream, string defaultPassword)
        {
            var assistantsResult = _excelReader.ReadAssistantFromStream(fileStream);
            if (!assistantsResult.IsSuccess || assistantsResult.Data == null)
                return Result<bool>.Failure(assistantsResult.Message, assistantsResult.StatusCode ?? 400);

            var newAssistantsData = assistantsResult.Data;
             
            var existingDepartmentIds = (await _unitOfWork.Departments.getAllIdsAsync())
                                            .Data
                                            .ToHashSet();
             
            var existingNationalIds = (await _userManager.Users.Select(u => u.UserName).ToListAsync())
                                        .ToHashSet();


            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // قائمة لتخزين ملفات تعريف المساعدين الجديدة لإضافتها بشكل مجمع لاحقاً
                var newAssistantProfiles = new List<Assistant>();

                foreach (var assistantData in newAssistantsData)
                {
                    // التحقق من الوجود باستخدام القوائم التي جلبناها مسبقاً (سريع جداً في الذاكرة)
                    if (existingNationalIds.Contains(assistantData.NationalId))
                    {
                        continue; // تخطي المستخدم الموجود بالفعل
                    }

                    if (existingDepartmentIds.FirstOrDefault(x=>x.Id == assistantData.DepartmentId)==null)
                    {
                        continue; // تخطي إذا كان القسم غير موجود
                    }

                    // إنشاء المستخدم (لا يزال يجب أن يكون متتالياً)
                    AppUser newUser = mapper.Map<AppUser>(assistantData);

                    // قد يكون هذا هو الجزء الأبطأ المتبقي نظراً لطبيعة Identity
                    var createResult = await _userManager.CreateAsync(newUser, defaultPassword);

                    if (!createResult.Succeeded)
                    {
                        // سجل الخطأ وتجاوز هذا السطر
                        //Console.WriteLine($"Error creating user {assistantData.NationalId}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                        continue;
                    }

                    // إضافة دور "Assistant" (لا يمكن تجنبه، هو استدعاء DB آخر)
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Assistant);

                    // أضف ملف التعريف إلى القائمة بدلاً من إضافته إلى الـ UnitOfWork مباشرة
                    newAssistantProfiles.Add(new Assistant
                    {
                        AppUserId = newUser.Id,
                        DepartmentId = assistantData.DepartmentId
                    });

                    // *** إزالة _unitOfWork.SaveChangesAsync() من داخل الحلقة! ***
                }

                // *** التحسين 3: إضافة جميع ملفات التعريف مرة واحدة (Batch Add) ***
                await _unitOfWork.Assistants.AddRangeAsync(newAssistantProfiles);

                // *** التحسين 4: حفظ جميع التغييرات في DB مرة واحدة خارج الحلقة ***
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

        public async Task<Result> UpdateAsync(AssistantModel assistantModel)
        {

            throw new NotImplementedException();
        }
    }
}
