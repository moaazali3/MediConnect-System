using Hospital.Application.DTOs.Patient;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetPatientDto>> GetAllPatients()
        {
            var patients = await _unitOfWork.Patients
                .GetAllAsync(
                filter: p => true,
                selector: p => new GetPatientDto
                {
                    PatientId = p.UserId,
                    PatientName = p.AppUser.FirstName + " " + p.AppUser.LastName
                }
                );

            return patients;
        }
    }
}
