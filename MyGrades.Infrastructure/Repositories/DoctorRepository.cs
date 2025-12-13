
using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.Projections_Models.User.Doctor;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext _context) : base(_context)
        {
            this._context = _context;
        }

        public async Task<Result<List<DoctorModel>>> FindAllDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Select(d => new DoctorModel
                {
                    Id = d.Id,
                    AppUserId = d.AppUserId,
                    FullName = d.User.FullName,
                    NationalId = d.User.NationalId
                })
                .AsNoTracking()
                .ToListAsync();
            if (doctors == null || !doctors.Any())
            {
                return Result<List<DoctorModel>>.Failure("No doctors found.", 404);
            }

            return Result<List<DoctorModel>>.Success(doctors);
        }

        public async Task<Result<DoctorModel>> FindDoctorByIdAsync(int doctorId)
        {
            var doctor = await _context.Doctors
                .Where(d => d.Id == doctorId)
                .Select(d => new DoctorModel
                {
                    Id = d.Id,
                    AppUserId = d.AppUserId,
                    FullName = d.User.FullName,
                    NationalId = d.User.NationalId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return Result<DoctorModel>.Failure("Doctor not found.", 404);
            }

            return Result<DoctorModel>.Success(doctor);
        }

        public async Task<Result<DoctorModel>> GetDoctorByNationalIdAsync(string nationalId)
        {
            var doctor = await _context.Doctors
                .Where(d => d.User.NationalId == nationalId)
                .Select(d => new DoctorModel
                {
                    Id = d.Id,
                    AppUserId = d.AppUserId,
                    FullName = d.User.FullName,
                    NationalId = d.User.NationalId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return Result<DoctorModel>.Failure("Doctor not found.", 404);
            }

            return Result<DoctorModel>.Success(doctor);
        }

        //public async Task<Result<List<SubjectModel>>> GetDoctorSubjectsAsync(int doctorId)
        //{
        //    var subjects = await _context.Subjects
        //        .Where(s => s.DoctorId == doctorId)
        //        .Select(s => new SubjectModel
        //        {
        //            Id = s.Id,
        //            Name = s.Name
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    if (subjects == null || !subjects.Any())
        //    {
        //        return Result<List<SubjectModel>>.Failure("No subjects found for this doctor.", 404);
        //    }

        //    return Result<List<SubjectModel>>.Success(subjects);
        //}


    }
}

          

