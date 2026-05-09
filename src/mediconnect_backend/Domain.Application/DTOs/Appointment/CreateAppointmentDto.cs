namespace Hospital.Application.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string DayOfWeek { get; set; }
        public DateOnly AppointmentDate { get; set; }
    }
}
