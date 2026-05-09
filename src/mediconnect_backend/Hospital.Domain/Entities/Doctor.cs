namespace Hospital.Domain.Entities
{
    public class Doctor
    {
        public AppUser AppUser { get; set; }
        public string UserId { get; set; }
        public Specialization specialization { get; set; }
        public int SpecializationId { get; set; }
        public decimal ExperienceYears { get; set; }
        public string Biography { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public Receptionist Receptionist { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    }
}
