namespace Hospital.Application.DTOs.DoctorSchedule
{
    public class DoctorScheduleDto
    {
        public string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
