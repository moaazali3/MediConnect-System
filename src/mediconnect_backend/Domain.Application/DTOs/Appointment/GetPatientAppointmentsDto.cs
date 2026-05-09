namespace Hospital.Application.DTOs.Appointment
{
    public class GetPatientAppointmentsDto
    {
        public Guid AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int QueueNumber { get; set; }
        public string Status { get; set; }

    }
}
