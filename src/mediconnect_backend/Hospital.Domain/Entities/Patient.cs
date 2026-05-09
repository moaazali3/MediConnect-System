namespace Hospital.Domain.Entities
{
    public class Patient
    {
        public AppUser AppUser { get; set; }
        public string UserId { get; set; }
        public string BloodType { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string EmergencyContact { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
