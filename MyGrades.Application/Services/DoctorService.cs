using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyGrades.Application.Contracts.Const;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.User.Doctor;
using MyGrades.Application.Contracts.Projections_Models.User.Doctor;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Application.Helper;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork; // Standard naming convention with underscore
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IExcelReader _excelReader;

        public DoctorService(IUnitOfWork unitOfWork, IMapper _mapper
            , UserManager<AppUser> userManager, IExcelReader excelReader)
        {
            _unitOfWork = unitOfWork; // Fixed naming
            mapper = _mapper;
            _userManager = userManager;
            _excelReader = excelReader;
        }

        public async Task<Result> AddDoctor(DoctorCreateModel doctorCreate)
        {
            var doctor = mapper.Map<Doctor>(doctorCreate);
            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteDoctor(int doctorId)
        {
            var doctor = await _unitOfWork.Doctors.FindAsync(x=>x.Id == doctorId);
            if (doctor == null||doctor.Data == null)
                return Result.Failure("Doctor not found.");

            await _unitOfWork.Doctors.DeleteAsync(doctor.Data);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<List<DoctorModel>>> GetAllDoctors()
        {
            var doctors = await _unitOfWork.Doctors.FindAllDoctorsAsync();
            if (doctors == null || doctors.Data == null || !doctors.Data.Any())
                return Result<List<DoctorModel>>.Failure("No doctors found.", 404);

            var doctorModels = (doctors.Data);
            return Result<List<DoctorModel>>.Success(doctorModels);
        }

        public async Task<Result<DoctorModel>> GetDoctorById(int doctorId)
        {
            var doctor = await _unitOfWork.Doctors.FindDoctorByIdAsync(doctorId);
            if (doctor == null || doctor.Data == null)
                return Result<DoctorModel>.Failure("Doctor not found.");

            var doctorModel = (doctor.Data);
            return Result<DoctorModel>.Success(doctorModel);
        }

        public async Task<Result<DoctorModel>> GetDoctorByNationalId(string nationalId)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByNationalIdAsync(nationalId);
            if (doctor == null || doctor.Data == null)
                return Result<DoctorModel>.Failure("Doctor not found.");

            var doctorModel = (doctor.Data);
            return Result<DoctorModel>.Success(doctorModel);
        }


        public async Task<Result> ImportDoctorsFromExcel(Stream stream, string defaultPassword)
        {
            
            var doctorsResult = _excelReader.ReadUsersFromStream(stream);
            if (!doctorsResult.IsSuccess)
                return Result<bool>.Failure(doctorsResult.Message, doctorsResult.StatusCode ?? 400);

            var newDoctorsData = doctorsResult.Data;

            if (newDoctorsData == null
                || newDoctorsData.Count == 0)
            {
                return Result<bool>.Failure("not found");
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var doctorData in newDoctorsData)
                {
                    // التحقق من الوجود (تجنب التكرار)
                    var existingUser = await _userManager.FindByNameAsync(doctorData.NationalId);
                    if (existingUser != null) continue;

                    // إنشاء المستخدم
                    AppUser newUser = mapper.Map<AppUser>(doctorData);

                    // كلمة المرور هي الرقم القومي
                    var createResult = await _userManager.CreateAsync(newUser, defaultPassword);

                    if (!createResult.Succeeded)
                    {
                        // يمكن تجميع الأخطاء وإرجاعها، أو التوقف
                        //createResult.Errors.First().Description;
                        continue;
                    }

                    // إضافة دور "Doctor" للمستخدم (خطوة مهمة جداً نسيته)
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Doctor);

                    // إنشاء بروفايل الطبيب
                    var newDoctorProfile = new Doctor
                    {
                        AppUserId = newUser.Id
                    };

                    await _unitOfWork.Doctors.AddAsync(newDoctorProfile);
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
        
    }
}