namespace Hospital.Application.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public decimal ExperienceYears { get; set; }
        public decimal ConsultationFee { get; set; }
        public int SpecializationId { get; set; }

    }
}
