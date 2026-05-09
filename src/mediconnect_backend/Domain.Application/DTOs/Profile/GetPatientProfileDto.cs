namespace Hospital.Application.DTOs.Profile
{
    public class GetPatientProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string BloodType { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string EmergencyContact { get; set; }
        public string PhoneNumber { get; set; }
    }
}
