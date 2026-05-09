using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Doctor
{
    public class GetAllDoctorForAdminDto
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SpecializationName { get; set; }
        public string Gender { get; set; }
        public decimal ExperienceYears { get; set; }
    }
}
