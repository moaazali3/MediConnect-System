using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Receptionist
{
    public class UpdateReceptionistDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
