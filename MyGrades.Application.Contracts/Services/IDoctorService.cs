namespace MyGrades.Application.Contracts.Services
{
    public interface IDoctorService
    {
        //public Task<Result> Get(int id);
        Task<Result> ImportDoctorsFromExcel(Stream stream, string defaultPassword);
    }
}
