using Hospital.Application.DTOs.Doctor;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetDoctorNames()
        {
            try
            {
                var doctorNames = await _doctorService.GetDoctorNames();
                return Ok(doctorNames);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors(string? specializationName = null)
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctors(specializationName);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("{doctorId}/{patientId}")]
        public async Task<IActionResult> GetDoctor(string doctorId, string patientId)
        {
            var doctor = await _doctorService.GetDoctor(doctorId, patientId);

            return Ok(doctor);
        }

        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetDoctor(string doctorId)
        {
            var doctor = await _doctorService.GetDoctor(doctorId);

            return Ok(doctor);
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateDoctor(CreateDoctorDto model)
        {
            try
            {
                await _doctorService.CreateDoctor(model);
                return Ok("Doctor created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost("{doctorId}/upload-profile-picture")]
        [Authorize(Roles = Role.Doctor)]
        public async Task<IActionResult> UploadProfilePicture(string doctorId, [FromForm] UploadDoctorImageDto model)
        {
            try 
            { 
                var result = await _doctorService.UploadProfilePictureAsync(doctorId, model.File);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("{doctorId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateDoctor(string doctorId, UpdateDoctorDto model)
        {
            try
            {
                await _doctorService.UpdateDoctor(doctorId, model);
                return Ok("Doctor updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("Activate/{doctorId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ActiveDoctor(string doctorId)
        {
            try
            {
                await _doctorService.ActiveDoctor(doctorId);
                return Ok("Doctor activated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("Inactivate/{doctorId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> InactiveDoctor(string doctorId)
        {
            try
            {
                await _doctorService.InactiveDoctor(doctorId);
                return Ok("Doctor inactivated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
