using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _petientService;
        private readonly IDoctorService _doctorService;

        public AdminController(IAdminService adminService, IAppointmentService appointmentService, IPatientService petientService, IDoctorService doctorService)
        {
            _adminService = adminService;
            _appointmentService = appointmentService;
            _petientService = petientService;
            _doctorService = doctorService;
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _adminService.GetDashboard();
            return Ok(dashboard);
        }

        [HttpGet("revenue/doctor/{doctorId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetRevenueByDoctor(string doctorId)
        {
            var revenue = await _adminService.GetRevenueByDoctor(doctorId);
            return Ok(revenue);
        }

        [HttpGet("revenue/specialization/{specializationName}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetRevenueBySpecialization(string specializationName)
        {
            var revenue = await _adminService.GetRevenueBySpecialization(specializationName);
            return Ok(revenue);
        }

        [HttpGet("all-doctors")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllDoctorsForAdmin()
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorsForAdmin();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("all-appointments")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointments();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("today-appointments")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetTodayAppointments()
        {
            var appointments = await _appointmentService.GetTodayAppointments();
            return Ok(appointments);
        }

        [HttpGet("today-appointments/specialization/{specializationName}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAppointmentsBySpecialization(string specializationName)
        {
            var appointments = await _appointmentService.GetTodayAppointmentsBySpecialization(specializationName);
            return Ok(appointments);
        }

        [HttpGet("today-appointments/doctor/{doctorId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAppointmentsByDoctor(string doctorId)
        {
            var appointments = await _appointmentService.GetTodayAppointmentsByDoctor(doctorId);
            return Ok(appointments);
        }

        [HttpGet("patients")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _petientService.GetAllPatients();
            return Ok(patients);
        }

        [HttpGet("doctors-working-today")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetDoctorsThatHasWorkToday()
        {
            var doctors = await _doctorService.GetDoctorsThatHasWorkToday();
            return Ok(doctors);
        }
    }
}
