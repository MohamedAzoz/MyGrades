using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.DTOs.User.Doctor;
using MyGrades.Application.Contracts.Projections_Models.User.Doctor;

namespace MyGrades.Application.Contracts.Services
{
    public interface IDoctorService
    {
        Task<Result> AddDoctor(DoctorCreateModel doctorCreate);
        Task<Result<List<DoctorModel>>> GetAllDoctors();
        Task<Result<DoctorModel>> GetDoctorById(int doctorId);
        Task<Result<DoctorModel>> GetDoctorByNationalId(string nationalId);
        //Task<Result> UpdateDoctor(Guid doctorId, DoctorUpdateModel doctorUpdate);
        //Task<Result<List<SubjectModel>>> GetDoctorSubjectsAsync(int doctorId);

        Task<Result> DeleteDoctor(int doctorId);
        Task<Result> ImportDoctorsFromExcel(Stream stream, string defaultPassword);
    }
}
