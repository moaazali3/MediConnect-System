namespace Hospital.Domain.Entities
{
    public class DoctorSchedule
    {
        public Guid ScheduleId { get; set; }
        public Doctor Doctor { get; set; }
        public string DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
