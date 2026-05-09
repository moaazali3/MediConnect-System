using Hospital.Application.DTOs.Specialization;

namespace Hospital.Application.Services.Interfaces
{
    public interface ISpecializationService
    {
        Task CreateSpecialization(CreateSpecializationDto model);
        Task UpdateSpecialization(int id, UpdateSpecializationDto model);
        Task DeleteSpecialization(int id);
        Task<List<GetSpecializationDto>> GetAllSpecializations();
    }
}
