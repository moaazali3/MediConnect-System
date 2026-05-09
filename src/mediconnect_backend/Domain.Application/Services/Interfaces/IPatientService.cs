using Hospital.Application.DTOs.Patient;

namespace Hospital.Application.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<GetPatientDto>> GetAllPatients();
    }
}
