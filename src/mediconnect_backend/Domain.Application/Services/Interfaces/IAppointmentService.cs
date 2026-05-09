using Hospital.Application.DTOs.Appointment;
using System.Globalization;

namespace Hospital.Application.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Guid> CreateAppointment(CreateAppointmentDto model);
        Task<List<GetPatientAppointmentsDto>> GetPatientAppointments(string patientId);
        Task<List<GetDoctorAppointmentsDto>> GetDoctorAppointments(string doctorId);
        Task<List<GetReceptionistAppointmentsDto>> GetReceptionistAppointments(string receptionistId);
        Task<List<GetAdminAppointmentsDto>> GetTodayAppointments();
        Task<List<GetAdminAppointmentsDto>> GetTodayAppointmentsBySpecialization(string specializationName);
        Task<List<GetAdminAppointmentsDto>> GetTodayAppointmentsByDoctor(string doctorId);
        Task CompleteAppointmentStatus(string appointmentId);
        Task CancelAppointmentStatus(string appointmentId);
        Task<int> ExpectedNumber(string doctorId, DateTime appointmentDate);
        Task<List<GetAllAppointmentsDto>> GetAllAppointments();

    }
}
