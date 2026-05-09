using Hospital.Application.DTOs.MedicalRecord;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = Role.Patient)]
        public async Task<IActionResult> GetByPatientId(string patientId)
        {
            try
            {
                var records = await _medicalRecordService.GetByPatientId(patientId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(Guid appointmentId)
        {
            try
            {
                var record = await _medicalRecordService.GetByAppointmentId(appointmentId);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = Role.Doctor)]
        public async Task<IActionResult> CreateMedicalRecord(CreateMedicalRecordDto model)
        {
            try
            {
                await _medicalRecordService.CreateMedicalRecord(model);
                return Ok("Medical record created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("{medicalRecordId}")]
        [Authorize(Roles = Role.Doctor)]
        public async Task<IActionResult> UpdateMedicalRecord(Guid medicalRecordId, UpdateMedicalRecordDto model)
        {
            try
            {
                await _medicalRecordService.UpdateMedicalRecord(medicalRecordId, model);
                return Ok("Medical record updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
