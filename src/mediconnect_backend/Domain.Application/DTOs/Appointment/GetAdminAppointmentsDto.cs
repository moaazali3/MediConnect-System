namespace Hospital.Application.DTOs.Appointment
{
    public class GetAdminAppointmentsDto
    {
        public Guid AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; }
    }
}
