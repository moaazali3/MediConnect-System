using Hospital.Application.DTOs.Profile;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("patient/{id}")]
        public async Task<IActionResult> GetPatientProfile(string id)
        {
            var profile = await _profileService.GetPatientProfile(id);

            return Ok(profile);
        }

        [HttpGet("doctor/{id}")]
        public async Task<IActionResult> GetDoctorProfile(string id)
        {
            var profile = await _profileService.GetDoctorProfile(id);

            return Ok(profile);
        }

        [HttpGet("receptionist/{id}")]
        public async Task<IActionResult> GetReceptionistProfile(string id)
        {
            var profile = await _profileService.GetReceptionistProfile(id);

            return Ok(profile);
        }

        [HttpPut("patient/{id}")]
        [Authorize(Roles = Role.Patient)]
        public async Task<IActionResult> UpdatePatientProfile(string id, UpdatePatientProfileDto model)
        {
            try
            {
                await _profileService.UpdatePatientProfile(id, model);
                return Ok("Patient profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("doctor/{id}")]
        [Authorize(Roles = Role.Doctor)]
        public async Task<IActionResult> UpdateDoctorProfile(string id, UpdateDoctorProfileDto model)
        {
            try
            {
                await _profileService.UpdateDoctorProfile(id, model);
                return Ok("Doctor profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("receptionist/{id}")]
        [Authorize(Roles = Role.Receptionist)]
        public async Task<IActionResult> UpdateReceptionistProfile(string id, UpdateReceptionistProfileDto model)
        {
            try
            {
                await _profileService.UpdateReceptionistProfile(id, model);
                return Ok("Receptionist profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto model)
        {
            try
            {
                await _profileService.ChangePassword(id, model);
                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
