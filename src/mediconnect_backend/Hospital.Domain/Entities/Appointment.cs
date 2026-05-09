using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public Patient Patient { get; set; }
        public string PatientId { get; set; }
        public Doctor Doctor { get; set; }
        public string DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int QueueNumber { get; set; }
        public Status Status { get; set; }
        public Payment Payment { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }
}
