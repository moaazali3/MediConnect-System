using Hospital.Application.DTOs.Admin;

namespace Hospital.Application.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboard();
        Task<RevenueByDoctorDto> GetRevenueByDoctor(string doctorId);
        Task<RevenueBySpecializationDto> GetRevenueBySpecialization(string sepecializationName);
    }
}
