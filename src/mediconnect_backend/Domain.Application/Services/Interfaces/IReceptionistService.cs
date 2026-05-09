using Hospital.Application.DTOs.Receptionist;

namespace Hospital.Application.Services.Interfaces
{
    public interface IReceptionistService
    {
        Task CreateReceptionist(CreateReceptionistDto model);
        Task<GetReceptionistDto> GetReceptionistByDoctorId(string doctorId);
        Task UpdateReceptionst(string receptionstId, UpdateReceptionistDto model);
        Task DeleteReceptionist(string receptionistId);
        Task ChangeDoctor(string receptionistId, string doctorId);
    }
}
