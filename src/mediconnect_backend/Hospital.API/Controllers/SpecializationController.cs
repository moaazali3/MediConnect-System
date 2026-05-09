using Hospital.Application.DTOs.Specialization;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _SpecializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _SpecializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specializations = await _SpecializationService.GetAllSpecializations();

            return Ok(specializations);
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateSpecialization(CreateSpecializationDto model)
        {
            try
            {
                await _SpecializationService.CreateSpecialization(model);
                return Ok("Specialization created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateSpecialization(int id, UpdateSpecializationDto model)
        {
            try
            {
                await _SpecializationService.UpdateSpecialization(id, model);
                return Ok("Specialization updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            try
            {
                await _SpecializationService.DeleteSpecialization(id);
                return Ok("Specialization deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
