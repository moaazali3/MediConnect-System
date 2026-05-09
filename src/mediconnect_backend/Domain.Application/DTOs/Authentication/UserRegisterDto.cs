namespace Hospital.Application.DTOs.Authentication
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContact { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string BloodType { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
    }
}
