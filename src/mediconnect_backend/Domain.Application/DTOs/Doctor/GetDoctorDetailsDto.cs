using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Doctor
{
    public class GetDoctorDetailsDto
    {
        public string Id { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SpecializationName { get; set; }
        public decimal ExperienceYears { get; set; }
        public string Biography { get; set; }
        public decimal ConsultationFee { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public List<GetDoctorScheduleDto> DoctorSchedules { get; set; } = new List<GetDoctorScheduleDto>();
    }
}
