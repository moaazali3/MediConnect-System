using Hospital.Application.DTOs.Doctor;
using Hospital.Application.DTOs.DoctorSchedule;

namespace Hospital.Application.Services.Interfaces
{
    public interface IDoctorScheduleService
    {
        Task CreateDoctorSchedule(CreateDoctorScheduleDto model, string doctorId);
        Task UpdateDoctorSchedule(CreateDoctorScheduleDto model, string doctorId);
        Task DeleteDoctorSchedule(string doctorId);
        Task<List<GetDoctorScheduleDto>> GetDoctorSchedule(string doctorId);
    }
}
