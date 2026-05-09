namespace Hospital.Domain.Entities
{
    public class MedicalRecord
    {
        public Guid MedicalRecordId { get; set; }
        public Appointment Appointment { get; set; }
        public Guid AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
