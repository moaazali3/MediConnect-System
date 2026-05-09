using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Doctor
{
    public class GetDoctorWorkingTodayDto
    {
        public string Id { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SpecializationName { get; set; }
    }
}
