using Hospital.Application.DTOs.MedicalRecord;

namespace Hospital.Application.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task CreateMedicalRecord(CreateMedicalRecordDto model);
        Task UpdateMedicalRecord(Guid medicalRecordId, UpdateMedicalRecordDto model);
        Task<GetMedicalRecordDto> GetByAppointmentId(Guid appointmentId);
        Task<List<GetMedicalRecordDto>> GetByPatientId(string patientId);
    }
}
