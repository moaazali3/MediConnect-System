namespace Hospital.Domain.Entities
{
    public class Receptionist
    {
        public AppUser AppUser { get; set; }
        public string UserId { get; set; }
        public Doctor Doctor { get; set; }
        public string DoctorId { get; set; }
    }
}
