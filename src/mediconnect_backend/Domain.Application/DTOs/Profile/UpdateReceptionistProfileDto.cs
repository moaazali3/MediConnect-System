using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Profile
{
    public class UpdateReceptionistProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
