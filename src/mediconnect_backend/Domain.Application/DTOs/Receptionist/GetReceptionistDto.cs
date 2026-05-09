using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.DTOs.Receptionist
{
    public class GetReceptionistDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string doctorName { get; set; }
    }
}
