using Hospital.Application.DTOs.Appointment;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = Role.Patient)]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId)
        {
            try
            {
                var appointments = await _appointmentService.GetPatientAppointments(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = Role.Doctor)]
        public async Task<IActionResult> GetAppointmentsByDoctorId(string doctorId)
        {
            try
            {
                var appointments = await _appointmentService.GetDoctorAppointments(doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("receptionist/{receptionistId}")]
        [Authorize(Roles = Role.Receptionist)]
        public async Task<IActionResult> GetAppointmentsByReceptionistId(string receptionistId)
        {
            try
            {
                var appointments = await _appointmentService.GetReceptionistAppointments(receptionistId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("expected-number")]
        public async Task<IActionResult> ExpectedNumber(string doctorId, DateTime appointmentDate)
        {
            try
            {
                var expectedNumber = await _appointmentService.ExpectedNumber(doctorId, appointmentDate);
                return Ok(expectedNumber);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = Role.Patient)]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDto model)
        {
            try
            {
                var id = await _appointmentService.CreateAppointment(model);
                return Ok(new { AppointmentId = id, Message = "Appointment created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("complete")]
        [Authorize(Roles = Role.Receptionist)]
        public async Task<IActionResult> CompleteAppointmentStatus(string appointmentId)
        {
            try
            {
                await _appointmentService.CompleteAppointmentStatus(appointmentId);
                return Ok("Appointment status updated to completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("cancel")]
        [Authorize(Roles = Role.Receptionist)]
        public async Task<IActionResult> CancelAppointmentStatus(string appointmentId)
        {
            try
            {
                await _appointmentService.CancelAppointmentStatus(appointmentId);
                return Ok("Appointment status updated to cancelled successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
