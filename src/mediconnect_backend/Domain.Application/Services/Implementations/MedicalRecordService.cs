using FluentValidation;
using Hospital.Application.DTOs.MedicalRecord;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateMedicalRecordDto> _createMedicalRecordValidator;
        private readonly IValidator<UpdateMedicalRecordDto> _updateMedicalRecordValidator;

        public MedicalRecordService(IUnitOfWork unitOfWork, IValidator<CreateMedicalRecordDto> createMedicalRecordValidator, IValidator<UpdateMedicalRecordDto> updateMedicalRecordValidator)
        {
            _unitOfWork = unitOfWork;
            _createMedicalRecordValidator = createMedicalRecordValidator;
            _updateMedicalRecordValidator = updateMedicalRecordValidator;
        }

        public async Task CreateMedicalRecord(CreateMedicalRecordDto model)
        {
            var result = _createMedicalRecordValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var appointment = await _unitOfWork.Appointments
                .GetAsync(a => a.AppointmentId == model.AppointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status != Status.Completed)
                throw new Exception("Medical record can only be created after appointment is completed");

            var existing = await _unitOfWork.MedicalRecords
                .GetAsync(m => m.AppointmentId == model.AppointmentId);

            if (existing != null)
                throw new Exception("Medical record already exists for this appointment");

            var record = new MedicalRecord
            {
                AppointmentId = model.AppointmentId,
                Diagnosis = model.Diagnosis,
                Prescription = model.Prescription,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.MedicalRecords.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetMedicalRecordDto> GetByAppointmentId(Guid appointmentId)
        {
            var record = await _unitOfWork.MedicalRecords
                .GetAsync(
                filter: m => m.AppointmentId == appointmentId,
                selector: m => new GetMedicalRecordDto
                {
                    MedicalRecordId = m.MedicalRecordId,
                    AppointmentId = m.AppointmentId,
                    Diagnosis = m.Diagnosis,
                    Prescription = m.Prescription,
                    CreatedDate = m.CreatedDate
                });

            if (record == null)
                throw new Exception("Medical record not found");

            return record;
        }

        public async Task<List<GetMedicalRecordDto>> GetByPatientId(string patientId)
        {
            var records = await _unitOfWork.MedicalRecords
                .GetAllAsync(
                filter: m => m.Appointment.PatientId == patientId,
                selector: m => new GetMedicalRecordDto
                {
                    MedicalRecordId = m.MedicalRecordId,
                    AppointmentId = m.AppointmentId,
                    Diagnosis = m.Diagnosis,
                    Prescription = m.Prescription,
                    CreatedDate = m.CreatedDate
                });

            if (records == null)
                throw new Exception("No medical records found for this patient");

            return records;
        }

        public async Task UpdateMedicalRecord(Guid medicalRecordId, UpdateMedicalRecordDto model)
        {
            var result = _updateMedicalRecordValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var record = await _unitOfWork.MedicalRecords
                .GetAsync(m => m.MedicalRecordId == medicalRecordId);

            if (record == null)
                throw new Exception("Medical record not found");

            record.Diagnosis = model.Diagnosis;
            record.Prescription = model.Prescription;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
