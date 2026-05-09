using Hospital.Application.DTOs.Admin;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Enums;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminDashboardDto> GetDashboard()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var totalDoctors = await _unitOfWork.Doctors.CountAsync();

            var totalPatients = await _unitOfWork.Patients.CountAsync();

            var totalAppointments = await _unitOfWork.Appointments.CountAsync();

            var totalAppointmentsToday = await _unitOfWork.Appointments
                .CountAsync(a => a.AppointmentDate == today);

            var totalPendingAppointmentsToday = await _unitOfWork.Appointments
                .CountAsync(a => a.AppointmentDate == today && a.Status == Status.Pending);

            var totalCompletedAppointmentsToday = await _unitOfWork.Appointments
                .CountAsync(a => a.AppointmentDate == today && a.Status == Status.Completed);

            var totalCancelledAppointmentsToday = await _unitOfWork.Appointments
                .CountAsync(a => a.AppointmentDate == today && a.Status == Status.Cancelled);

            var totalPendingAppointments = await _unitOfWork.Appointments.CountAsync(a => a.Status == Status.Pending);

            var totalCompletedAppointments = await _unitOfWork.Appointments.CountAsync(a => a.Status == Status.Completed);

            var totalCancelledAppointments = await _unitOfWork.Appointments.CountAsync(a => a.Status == Status.Cancelled);

            var totalRevenue = await _unitOfWork.Appointments
                .SumAsync(
                filter: a => a.Status == Status.Completed,
                selector: a => a.Doctor.ConsultationFee
                );

            var totalRevenueToday = await _unitOfWork.Appointments
                .SumAsync(
                filter: a => a.AppointmentDate == today && a.Status == Status.Completed,
                selector: a => a.Doctor.ConsultationFee
                );

            var dashoard = new AdminDashboardDto
            {
                TotalDoctors = totalDoctors,
                TotalPatients = totalPatients,
                TotalAppointments = totalAppointments,
                TotalAppointmentsToday = totalAppointmentsToday,
                TotalPendingAppointmentsToday = totalPendingAppointmentsToday,
                TotalCompletedAppointmentsToday = totalCompletedAppointmentsToday,
                TotalCancelledAppointmentsToday = totalCancelledAppointmentsToday,
                totalCompletedAppointments = totalCompletedAppointments,
                totalCancelledAppointments = totalCancelledAppointments,
                totalpendingAppointments = totalPendingAppointments,
                TotalRevenue = totalRevenue,
                TotalRevenueToday = totalRevenueToday
            };

            return dashoard;
        }

        public async Task<RevenueByDoctorDto> GetRevenueByDoctor(string doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var totalRevenue = await _unitOfWork.Appointments
                .SumAsync(
                filter: a => a.DoctorId == doctorId && a.Status == Status.Completed,
                selector: a => a.Doctor.ConsultationFee
                );

            var totalRevenueToday = await _unitOfWork.Appointments
                .SumAsync(
                filter: a => a.DoctorId == doctorId && a.AppointmentDate == today && a.Status == Status.Completed,
                selector: a => a.Doctor.ConsultationFee
                );

            var doctor = await _unitOfWork.Doctors.GetAsync(
                filter: d => d.UserId == doctorId,
                selector: d => new
                {
                    d.AppUser.FirstName,
                    d.AppUser.LastName
                }
                );

            var revenueByDoctor = new RevenueByDoctorDto
            {
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                TotalRevenue = totalRevenue,
                TotalRevenueToday = totalRevenueToday
            };

            return revenueByDoctor;
        }

        public async Task<RevenueBySpecializationDto> GetRevenueBySpecialization(string sepecializationName)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var totalRevenue = await _unitOfWork.Appointments
                .SumAsync(
                filter: a => a.Doctor.specialization.Name == sepecializationName && a.Status == Status.Completed,
                selector: a => a.Doctor.ConsultationFee
                );

            var totalRevenueToday = await _unitOfWork.Appointments
                .SumAsync(
                filter: a => a.Doctor.specialization.Name == sepecializationName && a.AppointmentDate == today && a.Status == Status.Completed,
                selector: a => a.Doctor.ConsultationFee
                );

            var revenueBySpecialization = new RevenueBySpecializationDto
            {
                TotalRevenue = totalRevenue,
                TotalRevenueToday = totalRevenueToday
            };

            return revenueBySpecialization;
        }
    }
}
