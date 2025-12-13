using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.User.Doctor;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
         private readonly ISubjectService _subjectService;
        public DoctorController(IDoctorService doctorService, ISubjectService subjectService)
        {
            _doctorService = doctorService;
            _subjectService = subjectService;
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        { 
            var result = await _doctorService.GetAllDoctors();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves a doctor by its ID.
        /// </summary>
        /// <param name="id">The doctor ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctorById(int id)
        { 
            var result = await _doctorService.GetDoctorById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Adds a new doctor.
        /// </summary>
        /// <param name="doctor">The doctor data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorCreateModel doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _doctorService.AddDoctor(doctor);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            
            return Created();
        }

        // GetDoctor
        [HttpGet("ByNationalId/{nationalId:long}")]
        public async Task<IActionResult> GetDoctorByNationalId(long nationalId)
        {
            var result = await _doctorService.GetDoctorByNationalId(nationalId.ToString());
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }
        // GetDoctorSubjectsAsync by doctor id
        //[HttpGet("{doctorId:int}/subjects")]
        //public async Task<IActionResult> GetDoctorSubjectsAsync(int doctorId)
        //{
        //    var result = await _subjectService.GetUserSubjectsAsync(s => s.DoctorId == doctorId);
        //    if (!result.IsSuccess)
        //        return StatusCode(result.StatusCode ?? 400, result.Message);
        //    return Ok(result.Data);
        //}

        /// <summary>
        /// Deletes a doctor by its ID.
        /// </summary>
        /// <param name="id">The doctor ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        { 
            var result = await _doctorService.DeleteDoctor(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Doctor deleted successfully.";
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all subjects taught by a specific doctor.
        /// </summary>
        /// <param name="id">The doctor ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("{id:int}/subjects")]
        public async Task<IActionResult> GetDoctorSubjects(int id)
        {
            var result = await _subjectService.GetUserSubjectsAsync(s => s.DoctorId == id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

    }

}
