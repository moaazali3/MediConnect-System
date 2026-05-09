namespace Hospital.Application.DTOs.Profile
{
    public class GetDoctorProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecializationName { get; set; }
        public decimal ExperienceYears { get; set; }
        public decimal ConsultationFee { get; set; }
        public string Biography { get; set; }

    }
}
