namespace Hospital.Application.DTOs.MedicalRecord
{
    public class CreateMedicalRecordDto
    {
        public Guid AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
    }
}
