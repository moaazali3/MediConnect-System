using Hospital.Application.DTOs.Receptionist;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;

        public ReceptionistController(IReceptionistService receptionistService)
        {
            _receptionistService = receptionistService;
        }

        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetReceptionistByDoctorId(string doctorId)
        {
            try
            {
                var receptionist = await _receptionistService.GetReceptionistByDoctorId(doctorId);
                return Ok(receptionist);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateReceptionist(CreateReceptionistDto model)
        {
            try
            {
                await _receptionistService.CreateReceptionist(model);
                return Ok("Receptionist created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("{receptionistId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateReceptionst(string receptionistId, UpdateReceptionistDto model)
        {
            try
            {
                await _receptionistService.UpdateReceptionst(receptionistId, model);
                return Ok("Receptionist updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("{receptionistId}/change-doctor/{doctorId}")]
        public async Task<IActionResult> ChangeDoctor(string receptionistId, string doctorId)
        {
            try
            {
                await _receptionistService.ChangeDoctor(receptionistId, doctorId);
                return Ok("Doctor changed successfully for the receptionist.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpDelete("{receptionistId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteReceptionist(string receptionistId)
        {
            try
            {
                await _receptionistService.DeleteReceptionist(receptionistId);
                return Ok("Receptionist deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
