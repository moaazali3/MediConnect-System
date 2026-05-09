using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Appointment
{
    public class GetReceptionistAppointmentsDto
    {
        public Guid AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int QueueNumber { get; set; }
        public string Status { get; set; }
    }
}
