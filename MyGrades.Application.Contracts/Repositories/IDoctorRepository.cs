using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.Projections_Models.User.Doctor;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Domain.Entities.Doctor>
    {
        public Task<Result<List<DoctorModel>>> FindAllDoctorsAsync();
        public Task<Result<DoctorModel>> FindDoctorByIdAsync(int doctorId);

        //public Task<Result<List<SubjectModel>>> GetDoctorSubjectsAsync(int doctorId);

    }
}
